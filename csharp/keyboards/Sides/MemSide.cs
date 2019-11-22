using System;
using System.Collections.Generic;
using System.IO;
using keyboards.Monitors;

namespace keyboards.Sides
{
    public class MemSide : MonitorSide
    {
        private readonly MovingAverage _average;
        private readonly Memory _memory;

        public MemSide(string filename, double red = 90, double yellow = 70, double green = 50) : base(filename, red, yellow, green)
        {
            _average = new MovingAverage();
            _memory = new Memory();
        }

        protected override double GetValue()
        {
            var total = _memory.Percentage;
            return _average.GetAverage(total);

        }
    }
}