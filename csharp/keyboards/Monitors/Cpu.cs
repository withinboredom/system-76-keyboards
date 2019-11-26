using System.Linq;
using System.Threading.Tasks;

namespace keyboards.Monitors
{
    public class Cpu : Monitor
    {
        /// <summary>
        ///     The instance
        /// </summary>
        private static IMonitor? _instance;

        /// <summary>
        ///     The file to monitor
        /// </summary>
        private readonly IFile _file;

        /// <summary>
        ///     The last idle calculation
        /// </summary>
        private long _lastIdle;

        /// <summary>
        ///     The last sum calculation
        /// </summary>
        private long _lastSum;

        /// <summary>
        ///     Create a new CPU monitor
        /// </summary>
        /// <param name="container"></param>
        private Cpu(IControlContainer container) : base(container)
        {
            _file = Container.File("/proc/stat");
            Container.RegisterActiveMonitor(this);
            UpdateMode = Mode.PercentageSmooth;
        }

        /// <summary>
        ///     Get the instance of this monitor
        /// </summary>
        /// <param name="container"></param>
        /// <returns></returns>
        public static IMonitor Instance(IControlContainer container)
        {
            return _instance ??= new Cpu(container);
        }

        /// <summary>
        ///     Compile the cpu reading into a percentage
        /// </summary>
        /// <param name="reading">The last raw reading</param>
        /// <returns></returns>
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

        /// <summary>
        ///     Take a raw reading from proc
        /// </summary>
        /// <returns></returns>
        private async Task<string[]> TakeReading()
        {
            var lines = await _file.Lines();
            return lines.ToList()[0].Split(' ').Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
        }

        /// <summary>
        ///     Get the latest reading from the sensor
        /// </summary>
        /// <returns></returns>
        protected override async Task<double> GetReading()
        {
            return CompileReading(await TakeReading());
        }
    }
}