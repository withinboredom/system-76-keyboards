using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;

namespace keyboards.Monitors
{
    public class Cpu : IMonitor
    {
        private long _lastSum = 0;
        private long _lastIdle = 0;
        private const string Filename = "/proc/stat";
        
        private double CompileReading(string[] reading)
        {
            var sum = reading.Skip(1).Select(long.Parse).Sum();
            var idle = long.Parse(reading[4]);
            var notIdle = (idle - _lastIdle) / (double) (sum - _lastSum);
            _lastIdle = idle;
            _lastSum = sum;
            var result = (1 - notIdle) * 100D;

            return result;
        }

        private static string[] TakeReading()
        {
            return File.ReadLines(Filename).ToList()[0].Split(' ').Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
        }

        public double Percentage => CompileReading(TakeReading());
    }
}