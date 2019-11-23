using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using keyboards.ColorSpace;
using keyboards.Monitors;
using keyboards.Sides;

namespace keyboards.Filters
{
    public class HeartFilter : IFilter
    {
        private DateTime _lastUpdate;
        private DateTime _currentUpdate;
        private readonly IMonitor _monitor;
        private readonly MovingAverage _average;
        private double _lastMeasure;
        private readonly double[] _steps;
        private int _currentStep;

        private readonly double[] _waitTable = new[]
        {
            0.05,
            0.04, 
            0.03, 
            0.02, 
            0.01, 
            0.009,
            0.008,
            0.005,
            0.003,
            0.001,
            0.0007,
            0.0005,
            0.0003,
            0.0003,
            0.0002,
            0.0002,
            0.0001,
        };

        public HeartFilter()
        {
            _lastUpdate = DateTime.Now;
            _monitor = new Cpu();
            _average = new MovingAverage(40);

            var steps = new List<double>();
            var theta = 0D;
            var amplitude = 0.75D;
            var midline = 0.23D;
            while (theta < Math.PI)
            {
                steps.Add(amplitude * Math.Sin(theta) + midline);
                theta += 0.05;
            }

            while (theta > 0)
            {
                steps.Add(amplitude * Math.Sin(theta) + midline);
                theta -= 0.1;
            }

            _steps = steps.Where(x => x > 0 && x < 1).ToArray();

            _currentStep = 0;
        }

        public Task PreApply(long time)
        {
            _lastMeasure = _average.GetAverage(_monitor.Percentage) / 100D;
            _currentUpdate = DateTime.Now;

            _lastMeasure = _lastMeasure >= 1 ? .99D : _lastMeasure;
            var currentWait = _waitTable[(int) (_waitTable.Length * _lastMeasure)];
            var frames = (_currentUpdate - _lastUpdate).Seconds / currentWait;
            if (_lastUpdate + TimeSpan.FromSeconds(currentWait) <= _currentUpdate)
            {
                _currentStep = (_currentStep + (int) Math.Max(frames, 1)) % _steps.Length;
                _lastUpdate = _currentUpdate;
            }

            return Task.CompletedTask;
        }

        public async Task<Rgb> ApplyFilter(Rgb color)
        {
            var brightness = _steps[_currentStep];
            
            var c = new Hsb(color);

            var newBrightness = c.Brightness * brightness;
            
            var next = new Rgb(c.SetBrightness(newBrightness));
         
            return next;
        }
    }
}