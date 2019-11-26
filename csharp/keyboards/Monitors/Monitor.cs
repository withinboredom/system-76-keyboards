using System;
using System.Threading.Tasks;
using keyboards.Sides;

namespace keyboards.Monitors
{
    public abstract class Monitor : IMonitor
    {
        protected readonly IControlContainer Container;
        private readonly MovingAverage _movingAverage;

        protected enum Mode
        {
            PercentageRaw,
            PercentageSmooth,
            Incremental,
        }

        protected Mode UpdateMode = Mode.PercentageSmooth;

        protected Monitor(IControlContainer container)
        {
            Container = container;
            _movingAverage = new MovingAverage();
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
            var handler = Changed;
            switch (UpdateMode)
            {
                case Mode.PercentageRaw:
                    if (Math.Abs(reading - Percentage) > 1D)
                    {
                        if (handler == null)
                            Container.DeregisterActiveMonitor(this);
                        else
                            handler(this, Percentage = reading);
                    }

                    break;
                case Mode.PercentageSmooth:
                    var smooth = _movingAverage.GetAverage(reading);
                    if (Math.Abs(smooth - Percentage) > 1D)
                    {
                        if (handler == null)
                        {
                            Container.DeregisterActiveMonitor(this);
                        }
                        else
                        {
                            handler(this, Percentage = smooth);
                        }
                    }

                    break;
                        case Mode.Incremental:
                    if (handler == null)
                    {
                        Container.DeregisterActiveMonitor(this);
                    }
                    else
                    {
                        handler(this, Percentage = reading);
                    }

                    break;
            }
        }

        /// <summary>
        ///     Get the latest reading from the sensor
        /// </summary>
        /// <returns></returns>
        protected abstract Task<double> GetReading();
    }
}