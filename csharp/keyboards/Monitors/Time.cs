using System;
using System.Threading.Tasks;

namespace keyboards.Monitors
{
    public class Time : Monitor
    {
        private Time(IControlContainer container) : base(container)
        {
            UpdateMode = Mode.Incremental;
            Container.RegisterActiveMonitor(this);
        }

        private static IMonitor? _instance;
        public static IMonitor Instance(IControlContainer container) => _instance ??= new Time(container);

        private double _lastReading;
        
        protected override Task<double> GetReading()
        {
            var reading = DateTime.Now.Ticks / (double)TimeSpan.TicksPerMillisecond;;
            _lastReading = reading;
            return Task.FromResult(reading);
        }
    }
}