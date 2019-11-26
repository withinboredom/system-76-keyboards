using System.Collections.Generic;
using System.Threading.Tasks;

namespace keyboards.Monitors
{
    public class Memory : Monitor
    {
        /// <summary>
        ///     The current instance
        /// </summary>
        private static IMonitor? _instance;

        /// <summary>
        ///     The file
        /// </summary>
        private readonly IFile _file;

        /// <summary>
        ///     Create a new memory monitor and register as active
        /// </summary>
        /// <param name="container"></param>
        private Memory(IControlContainer container) : base(container)
        {
            _file = Container.File("/proc/meminfo");
            Container.RegisterActiveMonitor(this);
            UpdateMode = Mode.PercentageSmooth;
        }

        /// <summary>
        ///     Get the active instance or create a new one
        /// </summary>
        /// <param name="container">The IoC container</param>
        /// <returns></returns>
        public static IMonitor Instance(IControlContainer container)
        {
            return _instance ??= new Memory(container);
        }

        /// <summary>
        ///     Get the reading from the proc filesystem
        /// </summary>
        /// <returns></returns>
        protected override async Task<double> GetReading()
        {
            var data = await _file.Lines();
            var memoryInfo = new Dictionary<string, double>();
            foreach (var line in data)
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                var keyValue = line.Split(':');
                if (keyValue.Length < 2 || string.IsNullOrWhiteSpace(keyValue[0]) ||
                    string.IsNullOrWhiteSpace(keyValue[1])) continue;
                memoryInfo[keyValue[0]] = double.Parse(keyValue[1].Trim().Split(' ')[0]);
            }

            if (!memoryInfo.ContainsKey("MemTotal") || !memoryInfo.ContainsKey("MemFree") ||
                !memoryInfo.ContainsKey("Buffers") || !memoryInfo.ContainsKey("Cached")) return 0;

            var used = memoryInfo["MemTotal"] - memoryInfo["MemFree"] - memoryInfo["Buffers"] -
                       memoryInfo["Cached"];
            var total = used / memoryInfo["MemTotal"] * 100.0;

            return total;
        }
    }
}