using keyboards.ColorSpace;
using keyboards.Monitors;

namespace keyboards.Sides
{
    public class HueSide : Side
    {
        private readonly double _degreesPerSecond;
        private readonly double _timeShift;
        private double lastTime;

        public HueSide(IMonitor monitor, double timeShift, double degreesPerSecond = 5D)
        {
            _timeShift = timeShift;
            monitor.Changed += MonitorOnChanged;
            CurrentColor = Rgb.FromHex("FF0000");
            _degreesPerSecond = degreesPerSecond;
        }

        private void MonitorOnChanged(object? sender, double e)
        {
            var time = e + _timeShift;
            var timeSince = time - lastTime;

            var next = new Hsb(CurrentColor);
            var hue = next.Hue * 360D;
            hue = (hue + _degreesPerSecond * (timeSince / 100D)) % 360D;
            next = next.SetHue(hue / 360D);

            CurrentColor = new Rgb(next);

            lastTime = time;
        }
    }
}