using System;
using System.Threading.Tasks;

namespace keyboards.Monitors
{
    public interface IMonitor
    {
        /// <summary>
        ///     The last reading from 0 → 100 from the monitor
        /// </summary>
        double Percentage { get; }

        /// <summary>
        ///     Subscribe to changes from the monitor
        /// </summary>
        event EventHandler<double> Changed;

        /// <summary>
        ///     Update reading if changed
        /// </summary>
        /// <returns></returns>
        Task CheckForChanges();
    }
}