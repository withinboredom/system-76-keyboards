using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using keyboards.Monitors;

namespace keyboards.Sides
{
    public class CpuSide : MonitorSide
    {
        private readonly MovingAverage _average;
        private readonly Cpu _monitor;
        
        public CpuSide(string filename, double red = 90, double yellow = 70, double green = 50) : base(filename, red, yellow, green)
        {
            _average = new MovingAverage();
            _monitor = new Cpu();
        }

        protected override double GetValue()
        {
            var usage = _monitor.Percentage;

            usage = _average.GetAverage(usage);

            return usage;
        }
    }
}