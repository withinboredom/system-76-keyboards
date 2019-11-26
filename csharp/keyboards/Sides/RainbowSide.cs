using System;
using System.Threading.Tasks;
using keyboards.ColorSpace;
using keyboards.Monitors;

namespace keyboards.Sides
{
    /// <summary>
    ///     A component that shifts colors over time
    /// </summary>
    public class RainbowSide : Side
    {
        /// <summary>
        ///     The phase of blue changes
        /// </summary>
        private readonly double _bluePhase;

        /// <summary>
        ///     The phase of green changes
        /// </summary>
        private readonly double _greenPhase;

        /// <summary>
        ///     The phase of red changes
        /// </summary>
        private readonly double _redPhase;

        /// <summary>
        ///     A time shift
        /// </summary>
        private readonly long _timeShift;

        /// <summary>
        ///     Create a rainbow side
        /// </summary>
        /// <param name="monitor"></param>
        /// <param name="redPhase"></param>
        /// <param name="greenPhase"></param>
        /// <param name="bluePhase"></param>
        /// <param name="timeShift"></param>
        /// <param name="file"></param>
        public RainbowSide(IMonitor monitor, double redPhase, double greenPhase, double bluePhase, long timeShift)
        {
            _redPhase = redPhase;
            _bluePhase = bluePhase;
            _greenPhase = greenPhase;
            _timeShift = timeShift;
            monitor.Changed += MonitorOnChanged;
        }

        private void MonitorOnChanged(object? sender, double e)
        {
            IsDirty = true;
            var time = e + _timeShift;

            CurrentColor = new Rgb(
                (byte) ((Math.Sin(time * _redPhase) + 1) * 255 / 2),
                (byte) ((Math.Sin(time * _greenPhase) + 1) * 255 / 2),
                (byte) ((Math.Sin(time * _bluePhase) + 1) * 255 / 2)
            );
        }
    }
}