using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using keyboards.ColorSpace;
using keyboards.Monitors;
using keyboards.Sides;

namespace keyboards.Filters
{
    /// <summary>
    ///     Tries to emulate a heartbeat based on cpu usage
    /// </summary>
    public class HeartFilter : IFilter
    {
        /// <summary>
        ///     Keep a moving average
        /// </summary>
        private readonly MovingAverage _average;

        /// <summary>
        ///     The monitor to use
        /// </summary>
        private readonly IMonitor _monitor;

        /// <summary>
        ///     The precalculated steps
        /// </summary>
        private readonly double[] _steps;

        /// <summary>
        ///     A table of update frames
        /// </summary>
        private readonly double[] _waitTable =
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
            0.0001
        };

        /// <summary>
        ///     The current step we're on
        /// </summary>
        private int _currentStep;

        /// <summary>
        ///     The current update time
        /// </summary>
        private DateTime _currentUpdate;

        /// <summary>
        ///     Our last measurement
        /// </summary>
        private double _lastMeasure;

        /// <summary>
        ///     The time of the last update
        /// </summary>
        private DateTime _lastUpdate;

        /// <summary>
        ///     Initialize the filter
        /// </summary>
        public HeartFilter()
        {
            _lastUpdate = DateTime.Now;
            _monitor = new Cpu();
            _average = new MovingAverage(40);

            var steps = new List<double>();
            var theta = 0D;
            const double amplitude = 0.75D;
            const double midline = 0.23D;
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

        /// <summary>
        ///     Called after render but before filtering, exactly once
        /// </summary>
        /// <param name="time">The time of the render</param>
        /// <returns></returns>
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

        /// <summary>
        ///     Applies the filter to a color, called per side
        /// </summary>
        /// <param name="color">The color to filter</param>
        /// <returns></returns>
        public Task<Rgb> ApplyFilter(Rgb color)
        {
            var brightness = _steps[_currentStep];

            var c = new Hsb(color);

            var newBrightness = c.Brightness * brightness;

            var next = new Rgb(c.SetBrightness(newBrightness));

            return Task.FromResult(next);
        }
    }
}