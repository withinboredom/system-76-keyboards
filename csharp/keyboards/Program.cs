using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CommandLine;
using keyboards.ColorSpace;
using keyboards.Filters;
using keyboards.Keyboards;
using Monitor = keyboards.Keyboards.Monitor;

namespace keyboards
{
    class Program
    {
        private static string Version => "0.1";

        internal class Options
        {
            public enum Filters
            {
                Heartbeat,
            }
            
            [Option('f', "filter", Required = false, HelpText = "Specify a filter to use")]
            public IEnumerable<Filters> Filter { get; set; }
            
            [Option('h', "frequency", Required = false, Default = 0.016, HelpText = "Determine the delay between frames")]
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
            public string Color { get; set; }
        }

        [Verb("monitor", HelpText = "Keep an eye on the machine")]
        internal class MonitorOptions : Options
        {
            
        }

        private static IFilter[] GetFilters(IEnumerable<Options.Filters> filters)
        {
            var arr = new List<IFilter>();
            foreach (var filter in filters)
            {
                switch (filter)
                {
                    case Options.Filters.Heartbeat:
                        arr.Add(new HeartFilter());
                        break;
                }
            }

            return arr.ToArray();
        }

        private static int RunOrInstall(string[] args, Options options, Keyboard kb)
        {
            if (!options.Install) return kb.Run(new CancellationToken()).Result;
            
            Installer.CreateParametersFromOptions(args);
            Installer.PutMeInRightSpot();
            Installer.CreateService();
            return 0;

        }
        
        static int Main(string[] args)
        {
            return Parser.Default.ParseArguments<RainbowOptions, SolidOptions, MonitorOptions>(args)
                .MapResult(
                    (MonitorOptions o) => RunOrInstall(args, o, new Monitor {Frequency = o.Frequency, Filters = GetFilters(o.Filter)}),
                    (RainbowOptions o) => RunOrInstall(args, o, new Rainbow {Frequency = o.Frequency, Filters = GetFilters(o.Filter)}),
                    (SolidOptions o ) => RunOrInstall(args, o, new SolidColor(Rgb.FromHex(o.Color)) { Frequency = o.Frequency, Filters = GetFilters(o.Filter)}),
                    errs => 1
                );
        }
    }
}