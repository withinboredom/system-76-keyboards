using System;
using System.Threading.Tasks;

namespace keyboards.Monitors
{
    public class Time : Monitor
    {
        private static IMonitor? _instance;

        private double _lastReading;

        private Time(IControlContainer container) : base(container)
        {
            UpdateMode = Mode.Incremental;
            Container.RegisterActiveMonitor(this);
        }

        public static IMonitor Instance(IControlContainer container)
        {
            return _instance ??= new Time(container);
        }

        protected override Task<double> GetReading()
        {
            var reading = DateTime.Now.Ticks / (double) TimeSpan.TicksPerMillisecond;
            ;
            _lastReading = reading;
            return Task.FromResult(reading);
        }
    }
}