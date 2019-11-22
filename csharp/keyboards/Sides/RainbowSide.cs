using System;

namespace keyboards
{
    /// <summary>
    /// A component that shifts colors over time
    /// </summary>
    public class RainbowSide : Side
    {
        /// <summary>
        /// The phase of red changes
        /// </summary>
        private readonly double _redPhase;
        
        /// <summary>
        /// The phase of blue changes
        /// </summary>
        private readonly double _bluePhase;
        
        /// <summary>
        /// The phase of green changes
        /// </summary>
        private readonly double _greenPhase;
        
        /// <summary>
        /// A time shift
        /// </summary>
        private readonly long _timeShift;

        /// <summary>
        /// Create a rainbow side
        /// </summary>
        /// <param name="redPhase"></param>
        /// <param name="greenPhase"></param>
        /// <param name="bluePhase"></param>
        /// <param name="timeShift"></param>
        /// <param name="filename"></param>
        public RainbowSide(double redPhase, double greenPhase, double bluePhase, long timeShift, string filename) : base(filename)
        {
            _redPhase = redPhase;
            _bluePhase = bluePhase;
            _greenPhase = greenPhase;
            _timeShift = timeShift;

            Load().Wait();
        }
        
        /// <summary>
        /// Render the rainbow!
        /// </summary>
        /// <param name="time"></param>
        public override void Render(long time)
        {
            time += _timeShift;
            if (CurrentColor == null) return;
            
            CurrentColor.Red = (byte) ((Math.Sin(time * _redPhase) + 1) * 255 / 2);
            CurrentColor.Green = (byte) ((Math.Sin(time * _greenPhase) + 1) * 255 / 2);
            CurrentColor.Blue = (byte) ((Math.Sin(time * _bluePhase) + 1) * 255 / 2);
        }
    }
}