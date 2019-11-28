using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using CommandLine;
using keyboards.Filters;
using keyboards.Keyboards;
using keyboards.Monitors;
using Microsoft.Extensions.Configuration;
using Monitor = keyboards.Keyboards.Monitor;

namespace keyboards
{
    internal class Program
    {
        private static readonly IFile PidFile = new SpecialFile("/run/keyboard-colors.pid");
        private static CancellationToken _token;

        private static IFilter[] GetFilters(IConfiguration configuration, bool dim, IControlContainer container)
        {
            var arr = new List<IFilter>();

            if (dim && Installer.RootHasPermission())
                arr.Add(new PowerFilter(container, Display.Instance(container)));
            else if (dim && !Installer.RootHasPermission())
                Console.Error.WriteLine(
                    "Root doesn't have permission to see your DPMS state, to give it permission run as your user: xhost si:localuser:root");

            foreach (var setting in configuration.GetChildren())
                switch (setting.Value)
                {
                    case "WashedOut":
                        arr.Add(new WashedOut());
                        break;
                    case "BlackWhite":
                        arr.Add(new BlackWhite());
                        break;
                    default:
                        ParseError("Filters",
                            "Filter contains an invalid filter. Please check your spelling and try again.");
                        break;
                }

            return arr.ToArray();
        }

        public static bool ParseError(string key, string reason)
        {
            Console.Error.WriteLine($"Unable to parse {key}: {reason}");
            Environment.Exit(1);
            return true;
        }

        private static Keyboard GetKeyboard(IControlContainer container, IConfiguration configuration)
        {
            var options = configuration.GetSection("Options");
            if (!double.TryParse(configuration.GetSection("FPS").Value, out var fps))
                ParseError("FPS", "FPS must be a number");

            if (!bool.TryParse(configuration.GetSection("DimOnSleep").Value, out var dim))
                ParseError("DimOnSleep", "DimOnSleep must be a boolean");

            var filters = GetFilters(configuration.GetSection("Filters"), dim, container);
            Keyboard keyboard = null;

            switch (configuration.GetSection("Mode").Value)
            {
                case "SolidColor":
                    keyboard = new SolidColor(container, options);
                    break;
                case "Rainbow":
                    keyboard = new Rainbow(container, options);
                    break;
                case "Monitor":
                    keyboard = new Monitor(container, options);
                    break;
                default:
                    ParseError("Mode", $"{configuration.GetSection("Mode")} is not a valid mode.");
                    return null;
            }

            keyboard.Filters = filters;
            keyboard.Frequency = FromFps(fps);

            return keyboard;
        }

        private static int RunOrInstall(string[] args, Options options)
        {
            var configuration = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .AddJsonFile(options.configFileName)
                .Build();

            var kb = GetKeyboard(new ControlContainer(), configuration);

            if (!PidFile.HasPermission)
            {
                Console.WriteLine("You need to run this as root!");
                Environment.Exit(1);
            }

            if (PidFile.Exists)
                try
                {
                    var process = Process.GetProcessById(int.Parse(PidFile.Contents));
                    if (!process.HasExited)
                    {
                        Console.WriteLine(
                            "The service is already running, did you mean to start it again? Hint: `keyboard-color stop`");
                        Environment.Exit(1);
                    }
                }
                catch (ArgumentException)
                {
                    // not running
                }

            Installer.Install();

            PidFile.Commit(Process.GetCurrentProcess().Id.ToString()).Wait();

            try
            {
                Task.WaitAll(new[] {kb.Run(_token)}, _token);
                return 0;
            }
            catch (OperationCanceledException)
            {
                return 1;
            }
        }

        private static double FromFps(double fps)
        {
            return 1D / fps;
        }

        private static int Main(string[] args)
        {
            var source = new CancellationTokenSource();
            _token = source.Token;
            Console.CancelKeyPress += (sender, eventArgs) =>
            {
                source.Cancel();
                if (PidFile.Contents == Process.GetCurrentProcess().Id.ToString())
                    PidFile.Delete();
                eventArgs.Cancel = true;
            };

            AppDomain.CurrentDomain.ProcessExit += (sender, eventArgs) =>
            {
                source.Cancel();
                if (PidFile.Contents == Process.GetCurrentProcess().Id.ToString())
                    PidFile.Delete();
            };

            var container = new ControlContainer();

            return Parser.Default
                .ParseArguments<RunOptions, StopOptions>(args)
                .MapResult(
                    (RunOptions o) => RunOrInstall(args, o),
                    (StopOptions o) =>
                    {
                        Process.Start("systemctl", "stop keyboard-colors.service")?.WaitForExit();
                        return 1;
                    },
                    errs => 1
                );
        }

        internal class Options
        {
            [Option('c', "config", Default = "/etc/keyboard-colors.json",
                HelpText = "Set the configuration file to load")]
            public string configFileName { get; set; }
        }

        [Verb("run", HelpText = "Run the keyboard colors service")]
        internal class RunOptions : Options
        {
        }

        [Verb("stop", HelpText = "Stop the currently running service")]
        internal class StopOptions : Options
        {
        }
    }
}