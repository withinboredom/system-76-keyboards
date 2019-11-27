using System;

namespace keyboards.Sides
{
    public class MovingAverage
    {
        private double _average;
        private double _c;
        private bool _ready;

        public double GetAverage(double newValue)
        {
            var now = DateTime.Now.Ticks / (double) TimeSpan.TicksPerSecond;
            var c = now - _c;
            _c = now;

            if (!_ready)
            {
                _ready = true;
                return 0;
            }

            var nextAverage = (1 - c) * _average + c * newValue;
            _average = nextAverage;

            if (!(c < 1D)) return newValue;

            return Math.Max(0, Math.Min(100, _average));
        }
    }
}