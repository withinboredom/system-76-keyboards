using System;
using System.Collections.Generic;
using System.IO;

namespace keyboards.Monitors
{
    public class Memory : IMonitor
    {
        private const string Filename = "/proc/meminfo";

        public double Percentage
        {
            get
            {
                var data = File.ReadLines(Filename);
                var memoryInfo = new Dictionary<string, double>();
                foreach (var line in data)
                {
                    if (string.IsNullOrWhiteSpace(line))
                        continue;

                    var keyValue = line.Split(':');
                    if (keyValue.Length < 2 || string.IsNullOrWhiteSpace(keyValue[0]) || string.IsNullOrWhiteSpace(keyValue[1])) continue;
                    memoryInfo[keyValue[0]] = double.Parse(keyValue[1].Trim().Split(' ')[0]);
                }

                if (!memoryInfo.ContainsKey("MemTotal") || !memoryInfo.ContainsKey("MemFree") ||
                    !memoryInfo.ContainsKey("Buffers") || !memoryInfo.ContainsKey("Cached")) return 0;
                
                var used = memoryInfo["MemTotal"] - memoryInfo["MemFree"] - memoryInfo["Buffers"] - memoryInfo["Cached"];
                var total = used / memoryInfo["MemTotal"] * 100.0;

                return total;
            }
        }
    }
}