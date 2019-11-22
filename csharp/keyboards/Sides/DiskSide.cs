using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using keyboards.Monitors;

namespace keyboards.Sides
{
    public class DiskSide : MonitorSide
    {
        private readonly MovingAverage _average;
        private readonly Disk _disk;
        
        public DiskSide(string filename, double red = 90, double yellow = 70, double green = 50) : base(filename, red, yellow, green)
        {
            _average = new MovingAverage();
            _disk = new Disk();
        }

        protected override double GetValue()
        {
            var usage = _disk.Percentage;
            usage = _average.GetAverage(usage);

            return usage;
        }
    }
}