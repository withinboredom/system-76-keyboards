using System;
using System.Threading.Tasks;

namespace keyboards.Monitors
{
    public abstract class Monitor : IMonitor
    {
        protected IControlContainer Container;

        protected Monitor(IControlContainer container)
        {
            Container = container;
        }

        /// <summary>
        ///     The last reading from 0 → 100 from the monitor
        /// </summary>
        public double Percentage { get; private set; }

        /// <summary>
        ///     Subscribe to changes from the monitor
        /// </summary>
        public event EventHandler<double>? Changed;

        /// <summary>
        ///     Update reading if changed by at least 1%
        /// </summary>
        /// <returns></returns>
        public async Task CheckForChanges()
        {
            var reading = await GetReading();
            if (Math.Abs(reading - Percentage) > 1D)
            {
                var handler = Changed;

                if (handler == null)
                    Container.DeregisterActiveMonitor(this);
                else
                    handler(this, Percentage = reading);
            }
        }

        /// <summary>
        ///     Get the latest reading from the sensor
        /// </summary>
        /// <returns></returns>
        protected abstract Task<double> GetReading();
    }
}