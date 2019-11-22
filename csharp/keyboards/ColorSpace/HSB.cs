using System;

namespace keyboards.ColorSpace
{
    public struct Hsb
    {
        public static readonly Hsb Empty = new Hsb();

        internal const double Tolerance = 0.0000001;

        public static bool operator ==(Hsb i1, Hsb i2)
        {
            return Math.Abs(i1.Hue - i2.Hue) < Tolerance &&
                   Math.Abs(i1.Saturation - i2.Saturation) < Tolerance &&
                   Math.Abs(i1.Brightness - i2.Brightness) < Tolerance;
        }

        public static bool operator !=(Hsb i1, Hsb i2)
        {
            return Math.Abs(i1.Hue - i2.Hue) > Tolerance ||
                   Math.Abs(i1.Saturation - i2.Saturation) > Tolerance ||
                   Math.Abs(i1.Brightness - i2.Brightness) > Tolerance;
        }

        public double Hue { get; }

        public double Saturation { get; }

        public double Brightness { get; }

        public Hsb SetBrightness(double b)
        {
            return new Hsb(Hue, Saturation, b);
        }

        public Hsb(double h, double s, double b)
        {
            Hue = h % 360D;
            Saturation = s > 1D ? 1D : s < 0 ? 0 : s;
            Brightness = b > 1D ? 1D : b < 0 ? 0 : b;
        }

        public Hsb(Hsb other)
        {
            Hue = other.Hue;
            Saturation = other.Saturation;
            Brightness = other.Brightness;
        }

        public Hsb(Rgb other)
        {
            var r = other.Red / 255D;
            var g = other.Green / 255D;
            var b = other.Blue / 255D;
            
            var max = Math.Max(r, Math.Max(g, b));
            var min = Math.Min(r, Math.Min(g, b));
            var delMax = max - min;

            Brightness = max;

            if (Math.Abs(delMax) < Tolerance)
            {
                Hue = 0;
                Saturation = 0;
            }
            else
            {
                Saturation = delMax / max;

                var hue = 0D;

                var delR = ((max - r) / 6D + delMax / 2D) / delMax;
                var delG = ((max - g) / 6D + delMax / 2D) / delMax;
                var delB = ((max - b) / 6D + delMax / 2D) / delMax;

                if (r == max) hue = delB - delG;
                else if (g == max) hue = 1D / 3D + delR - delB;
                else if (b == max) hue = 2D / 3D + delG - delR;
                else
                {
                    hue = 0;
                    Brightness = 0;
                    Saturation = 0;
                }

                if (hue < 0) hue += 1;
                if (hue > 1) hue -= 1;

                Hue = hue;
            }
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;

            return this == (Hsb) obj;
        }

        public override int GetHashCode()
        {
            return Hue.GetHashCode() ^ Saturation.GetHashCode() ^ Brightness.GetHashCode();
        }
    }
}