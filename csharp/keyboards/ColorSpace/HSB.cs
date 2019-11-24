using System;

namespace keyboards.ColorSpace
{
    /// <summary>
    ///     Represents the HSB color space
    /// </summary>
    public struct Hsb
    {
        /// <summary>
        ///     Undefined color / black
        /// </summary>
        public static readonly Hsb Empty = new Hsb();

        /// <summary>
        ///     Tolerance for comparing two colors
        /// </summary>
        private const double Tolerance = 0.0000001;

        /// <summary>
        ///     True if two colors are the same.
        /// </summary>
        /// <param name="i1">The left</param>
        /// <param name="i2">The right</param>
        /// <returns>True if the colors are the same, false otherwise</returns>
        public static bool operator ==(Hsb i1, Hsb i2)
        {
            return Math.Abs(i1.Hue - i2.Hue) < Tolerance &&
                   Math.Abs(i1.Saturation - i2.Saturation) < Tolerance &&
                   Math.Abs(i1.Brightness - i2.Brightness) < Tolerance;
        }

        /// <summary>
        ///     True if two colors are not the same
        /// </summary>
        /// <param name="i1">The left</param>
        /// <param name="i2">The right</param>
        /// <returns>True if the colors are not the same, false otherwise</returns>
        public static bool operator !=(Hsb i1, Hsb i2)
        {
            return Math.Abs(i1.Hue - i2.Hue) > Tolerance ||
                   Math.Abs(i1.Saturation - i2.Saturation) > Tolerance ||
                   Math.Abs(i1.Brightness - i2.Brightness) > Tolerance;
        }

        /// <summary>
        ///     The hue of the color
        /// </summary>
        public double Hue { get; }

        /// <summary>
        ///     The saturation of the color
        /// </summary>
        public double Saturation { get; }

        /// <summary>
        ///     The brightness of the color
        /// </summary>
        public double Brightness { get; }

        /// <summary>
        ///     Get a new color after adjusting the brightness
        /// </summary>
        /// <param name="brightness">The new brightness</param>
        /// <returns>A new color with the brightness adjusted</returns>
        public Hsb SetBrightness(double brightness)
        {
            return new Hsb(Hue, Saturation, brightness);
        }

        /// <summary>
        ///     Get a new color after adjusting the saturation
        /// </summary>
        /// <param name="saturation">The new saturation</param>
        /// <returns>A new color with the saturation adjusted</returns>
        public Hsb SetSaturation(double saturation)
        {
            return new Hsb(Hue, saturation, Brightness);
        }

        /// <summary>
        ///     Get a new color after adjusting the hue
        /// </summary>
        /// <param name="hue">The new hue</param>
        /// <returns>A new color with the hue adjusted</returns>
        public Hsb SetHue(double hue)
        {
            return new Hsb(hue, Saturation, Brightness);
        }

        /// <summary>
        ///     Create a new HSB color
        /// </summary>
        /// <param name="hue">The hue</param>
        /// <param name="saturation">The saturation</param>
        /// <param name="brightness">The brightness</param>
        public Hsb(double hue, double saturation, double brightness)
        {
            Hue = hue % 360D;
            Saturation = saturation > 1D ? 1D : saturation < 0 ? 0 : saturation;
            Brightness = brightness > 1D ? 1D : brightness < 0 ? 0 : brightness;
        }

        /// <summary>
        ///     Create a new HSB color from another HSB color
        /// </summary>
        /// <param name="other">The color to copy</param>
        public Hsb(Hsb other)
        {
            Hue = other.Hue;
            Saturation = other.Saturation;
            Brightness = other.Brightness;
        }

        /// <summary>
        ///     Create a new HSB color from an RGB color space
        /// </summary>
        /// <param name="other">The color to copy</param>
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

                if (r == max)
                {
                    hue = delB - delG;
                }
                else if (g == max)
                {
                    hue = 1D / 3D + delR - delB;
                }
                else if (b == max)
                {
                    hue = 2D / 3D + delG - delR;
                }
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

        /// <summary>
        ///     True if two colors are identical
        /// </summary>
        /// <param name="obj">The object to compare to</param>
        /// <returns>True if the object is a color and the same color</returns>
        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;

            return this == (Hsb) obj;
        }

        /// <summary>
        ///     Calculate the hash code of this color
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return Hue.GetHashCode() ^ Saturation.GetHashCode() ^ Brightness.GetHashCode();
        }

        public override string ToString()
        {
            return Hue + "°, " + Saturation * 100D + "%, " + Brightness * 100D + "%";
        }
    }
}