using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace keyboards.Monitors
{
    public class Disk : IMonitor
    {
        private const string Filename = "/proc/diskstats";
        private int _lastUpdate;
        private double _max;
        
        public double Percentage
        {
            get
            {
                var data = File.ReadLines(Filename);
                var info = 0;
                foreach (var line in data)
                {
                    var device = Regex.Split(line, "\\s+").Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
                    if (device.Length != 0 && int.TryParse(device[1], out var nun) && nun == 0)
                    {
                        info += int.Parse(device[3]) + int.Parse(device[7]);
                    }
                }

                var diff = 0;
            
                if (_lastUpdate == 0)
                {
                    diff = 0;
                } else if (info > _lastUpdate)
                {
                    diff = info - _lastUpdate;
                }
                else
                {
                    diff = 0;
                }

                _lastUpdate = info;
                
                // decay max over time
                _max = Math.Max(_max, diff) - 0.5;

                var usage = 0.0;
            
                if (_max > 0)
                {
                    usage = diff / _max * 100.0;
                }

                return usage;
            }
        }
    }
}