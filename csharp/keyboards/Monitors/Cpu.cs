using System.IO;
using System.Linq;

namespace keyboards.Monitors
{
    public class Cpu : IMonitor
    {
        private const string Filename = "/proc/stat";
        private long _lastIdle;
        private long _lastSum;

        public double Percentage => CompileReading(TakeReading());

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
    }
}