using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace keyboards.Monitors
{
    public class Disk : Monitor
    {
        /// <summary>
        ///     The instance of this disk monitor
        /// </summary>
        private static IMonitor? _instance;

        /// <summary>
        ///     The file from proc
        /// </summary>
        private readonly IFile _file;

        /// <summary>
        ///     The last update
        /// </summary>
        private int _lastUpdate;

        /// <summary>
        ///     Maximum reading
        /// </summary>
        private double _max;


        /// <summary>
        ///     Create a new disk monitor
        /// </summary>
        /// <param name="container"></param>
        private Disk(IControlContainer container) : base(container)
        {
            _file = Container.File("/proc/diskstats");
            Container.RegisterActiveMonitor(this);
        }

        /// <summary>
        ///     Get the disk monitor instance
        /// </summary>
        /// <param name="container"></param>
        /// <returns></returns>
        public static IMonitor Instance(IControlContainer container)
        {
            return _instance ??= new Disk(container);
        }

        /// <summary>
        ///     Get the latest reading from the sensor
        /// </summary>
        /// <returns></returns>
        protected override async Task<double> GetReading()
        {
            var data = await _file.Lines();
            var info = 0;
            foreach (var line in data)
            {
                var device = Regex.Split(line, "\\s+").Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
                if (device.Length != 0 && int.TryParse(device[1], out var nun) && nun == 0)
                    info += int.Parse(device[3]) + int.Parse(device[7]);
            }

            var diff = 0;

            if (_lastUpdate == 0)
                diff = 0;
            else if (info > _lastUpdate)
                diff = info - _lastUpdate;
            else
                diff = 0;

            _lastUpdate = info;

            // decay max over time
            _max = Math.Max(_max, diff) - 0.5;

            var usage = 0.0;

            if (_max > 0) usage = diff / _max * 100.0;

            return usage;
        }
    }
}