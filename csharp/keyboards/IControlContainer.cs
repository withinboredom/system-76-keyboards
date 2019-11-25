using System.Collections.Generic;
using keyboards.Monitors;

namespace keyboards
{
    public interface IControlContainer
    {
        HashSet<IMonitor> Monitors { get; set; }

        /// <summary>
        ///     Get an IFile for a File
        /// </summary>
        /// <param name="filename">The file to use</param>
        /// <returns></returns>
        IFile File(string filename);

        /// <summary>
        ///     Registers an active monitor
        /// </summary>
        /// <param name="monitor">The monitor to register</param>
        void RegisterActiveMonitor(IMonitor monitor)
        {
            Monitors.Add(monitor);
        }

        /// <summary>
        ///     Remove an active monitor from the registry
        /// </summary>
        /// <param name="monitor">The monitor to deregister</param>
        void DeregisterActiveMonitor(IMonitor monitor)
        {
            Monitors.Remove(monitor);
        }
    }
}