using System.Linq;
using System.Threading.Tasks;

namespace keyboards.Monitors
{
    public class Cpu : IMonitor
    {
        private readonly IFile _file = new SpecialFile("/proc/stat");
        private long _lastIdle;
        private long _lastSum;

        public async Task<double> Percentage()
        {
            return CompileReading(await TakeReading());
        }

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

        private async Task<string[]> TakeReading()
        {
            var lines = await _file.Lines();
            return lines.ToList()[0].Split(' ').Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
        }
    }
}