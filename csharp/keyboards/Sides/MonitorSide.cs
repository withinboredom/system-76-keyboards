using System;
using System.Threading.Tasks;
using keyboards.ColorSpace;
using keyboards.Monitors;

namespace keyboards.Sides
{
    public class MonitorSide : Side
    {
        private readonly double _green;
        private readonly double _red;
        private readonly double _yellow;

        private readonly MovingAverage _movingAverage;

        public MonitorSide(IMonitor monitor, double red = 90, double yellow = 70, double green = 50)
        {
            _red = red;
            _yellow = yellow;
            _green = green;
            _movingAverage = new MovingAverage();
            monitor.Changed += MonitorOnChanged;
        }

        private double Value { get; set; }

        private void MonitorOnChanged(object? sender, double e)
        {
            Value = e;
        }

        public override Task Render(long time, long deltaTime)
        {
            var value = _movingAverage.GetAverage(Value);
            if (value < 0) value = 0;
            else if (value >= 100) value = 100;

            if (value < _green)
                CurrentColor = new Rgb(
                    0,
                    (byte) Math.Floor(value / _green * 255),
                    (byte) Math.Floor((1 - value / _green) * 255)
                );
            else if (value >= _green && value < _yellow)
                CurrentColor = new Rgb(
                    (byte) Math.Floor((value - _green) / (_yellow - _green) * 255),
                    255,
                    0
                );
            else if (value >= _yellow && value < _red)
                CurrentColor = new Rgb(
                    255,
                    (byte) Math.Floor(1 - (value - _yellow) / (_red - _yellow) * 255),
                    0
                );
            else if (value >= _red) CurrentColor = new Rgb(255, 0, 0);

            return Task.CompletedTask;
        }
    }
}