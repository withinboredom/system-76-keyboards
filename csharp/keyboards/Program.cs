using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using CommandLine;
using keyboards.ColorSpace;
using keyboards.Filters;
using keyboards.Keyboards;
using Monitor = keyboards.Keyboards.Monitor;

namespace keyboards
{
    internal class Program
    {
        private static readonly IFile PidFile = new SpecialFile("/run/keyboard-colors.pid");
        private static CancellationToken _token;

        private static IFilter[] GetFilters(IEnumerable<Options.Filters>? filters, IControlContainer container)
        {
            var arr = new List<IFilter>();
            if (filters == null) return arr.ToArray();
            foreach (var filter in filters)
                switch (filter)
                {
                    case Options.Filters.Heartbeat:
                        arr.Add(new HeartFilter(container));
                        break;
                    case Options.Filters.WashedOut:
                        arr.Add(new WashedOut());
                        break;
                    case Options.Filters.BlackWhite:
                        arr.Add(new BlackWhite());
                        break;
                }

            return arr.ToArray();
        }

        private static int RunOrInstall(string[] args, Options options, Keyboard kb)
        {
            if (!PidFile.HasPermission)
            {
                Console.WriteLine("You need to run this as root!");
                Environment.Exit(1);
            }

            if (PidFile.Exists)
            {
                Console.WriteLine(
                    "The service is already running, did you mean to start it again? Hint: `keyboard-color stop`");
                Environment.Exit(1);
            }

            if (options.Install)
            {
                Installer.CreateParametersFromOptions(args);
                Installer.PutMeInRightSpot();
                Installer.CreateService();
                return 0;
            }

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

            return Parser.Default.ParseArguments<RainbowOptions, SolidOptions, MonitorOptions, StopOptions>(args)
                .MapResult(
                    (MonitorOptions o) => RunOrInstall(args, o,
                        new Monitor(container) {Frequency = FromFps(o.Frequency), Filters = GetFilters(o.Filter, container)}),
                    (RainbowOptions o) => RunOrInstall(args, o,
                        new Rainbow(container) {Frequency = FromFps(o.Frequency), Filters = GetFilters(o.Filter, container)}),
                    (SolidOptions o) => RunOrInstall(args, o,
                        new SolidColor(container, o.Color != null ? Rgb.FromHex(o.Color) : Rgb.Empty)
                            {Frequency = FromFps(o.Frequency), Filters = GetFilters(o.Filter, container)}),
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
            public enum Filters
            {
                Heartbeat,
                WashedOut,
                BlackWhite
            }

            [Option('f', "filter", Required = false, HelpText = "Specify a filter to use", Separator = ',')]
            public IEnumerable<Filters>? Filter { get; set; }

            [Option('s', "fps", Required = false, Default = 10,
                HelpText = "Determine the delay between frames")]
            public double Frequency { get; set; }

            [Option('i', "install", Required = false, HelpText = "Install the active command")]
            public bool Install { get; set; }
        }

        [Verb("rainbow", HelpText = "Turn on the rainbow!")]
        internal class RainbowOptions : Options
        {
        }

        [Verb("solidcolor", HelpText = "Keep it simple")]
        internal class SolidOptions : Options
        {
            [Option('c', "color", Default = "FFFFFF", HelpText = "Specify the color to become")]
            public string? Color { get; set; }
        }

        [Verb("monitor", HelpText = "Keep an eye on the machine")]
        internal class MonitorOptions : Options
        {
        }

        [Verb("stop", HelpText = "Stop the currently running service")]
        internal class StopOptions : Options
        {
        }
    }
}