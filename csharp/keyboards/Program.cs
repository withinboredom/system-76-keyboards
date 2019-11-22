using System;
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

        private class Options
        {
            [Option('v', "version", Required = false, HelpText = "Print the current version")]
            public bool Version { get; set; }
        }
        
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(options =>
                {
                    if (options.Version)
                    {
                        Console.WriteLine(Version);
                    }

                    //var kb = new Rainbow {Frequency = 0.05, Filters = new IFilter[] {new BreatheFilter()}};
                    var kb = new SolidColor(Rgb.FromHex("FFFFFF")) {Frequency = 0.016, Filters = new []{new HeartFilter(), }};
                    //var kb = new Monitor {Frequency = 0.05};
                    kb.Run(new CancellationToken()).Wait();
                });
        }
    }
}