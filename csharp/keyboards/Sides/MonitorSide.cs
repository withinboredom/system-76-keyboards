using System;
using System.Threading.Tasks;
using keyboards.ColorSpace;

namespace keyboards.Sides
{
    public abstract class MonitorSide : Side
    {
        private readonly double _red;
        private readonly double _yellow;
        private readonly double _green;

        protected MonitorSide(string filename, double red = 90, double yellow = 70, double green = 50) : base(filename)
        {
            _red = red;
            _yellow = yellow;
            _green = green;
        }

        protected abstract double GetValue();

        public override async Task Render(long time, long deltaTime)
        {
            var value = GetValue();
            if (value < 0) value = 0;
            else if (value >= 100) value = 100;

            if (CurrentColor == null) return;

            if (value < _green)
            {
                CurrentColor = new Rgb(
                    0,
                    (byte) Math.Floor(value / _green * 255),
                    (byte) Math.Floor((1 - value / _green) * 255)
                );

            }
            else if (value >= _green && value < _yellow)
            {
                CurrentColor = new Rgb(
                    (byte) Math.Floor((value - _green) / (_yellow - _green) * 255),
                    255,
                    0
                );
            } 
            else if (value >= _yellow && value < _red)
            {
                CurrentColor = new Rgb(
                    255,
                    (byte) Math.Floor(1 - (value - _yellow) / (_red - _yellow) * 255),
                    0
                );
            } 
            else if (value >= _red)
            {
                CurrentColor = new Rgb(255, 0, 0);
            }
        }
    }
}