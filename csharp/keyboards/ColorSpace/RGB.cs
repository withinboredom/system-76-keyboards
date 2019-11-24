using System;

namespace keyboards.ColorSpace
{
    /// <summary>
    ///     Represents a color in the RGB Space
    /// </summary>
    public struct Rgb
    {
        /// <summary>
        ///     An undefined color / black
        /// </summary>
        public static readonly Rgb Empty = new Rgb();

        /// <summary>
        ///     True if two colors are the same color
        /// </summary>
        /// <param name="i1">Left</param>
        /// <param name="i2">Right</param>
        /// <returns>True if left and right are the same color</returns>
        public static bool operator ==(Rgb i1, Rgb i2)
        {
            return i1.Red == i2.Red &&
                   i1.Green == i2.Green &&
                   i1.Blue == i2.Blue;
        }

        /// <summary>
        ///     True if two colors are not the same
        /// </summary>
        /// <param name="i1">Left</param>
        /// <param name="i2">Right</param>
        /// <returns>True if left and right are not the same color</returns>
        public static bool operator !=(Rgb i1, Rgb i2)
        {
            return i1.Red != i2.Red ||
                   i1.Green != i2.Green ||
                   i1.Blue != i2.Blue;
        }

        /// <summary>
        ///     The red component
        /// </summary>
        public byte Red { get; }

        /// <summary>
        ///     The green component
        /// </summary>
        public byte Green { get; }

        /// <summary>
        ///     The blue component
        /// </summary>
        public byte Blue { get; }

        /// <summary>
        ///     Create a new color from individual components
        /// </summary>
        /// <param name="red">Red</param>
        /// <param name="green">Green</param>
        /// <param name="blue">Blue</param>
        public Rgb(byte red, byte green, byte blue)
        {
            Red = red;
            Green = green;
            Blue = blue;
        }

        /// <summary>
        ///     Copy an existing color
        /// </summary>
        /// <param name="rgb">The color to copy</param>
        public Rgb(Rgb rgb)
        {
            Red = rgb.Red;
            Green = rgb.Green;
            Blue = rgb.Blue;
        }

        /// <summary>
        ///     Convert an HSB color space to RBB color space
        /// </summary>
        /// <param name="other">The color to convert</param>
        public Rgb(Hsb other)
        {
            double r = 0, b = 0, g = 0;

            if (other.Brightness < 0)
            {
                r = b = g = 0;
            }
            else if (other.Saturation < 0)
            {
                r = g = b = other.Brightness;
            }
            else
            {
                var h = other.Hue * 6D;
                if (h == 6D) h = 0;
                var i = (int) Math.Floor(h);
                var v1 = other.Brightness * (1 - other.Saturation);
                var v2 = other.Brightness * (1 - other.Saturation * (h - i));
                var v3 = other.Brightness * (1 - other.Saturation * (1 - (h - i)));

                switch (i)
                {
                    case 0:
                        r = other.Brightness;
                        g = v3;
                        b = v1;
                        break;
                    case 1:
                        r = v2;
                        g = other.Brightness;
                        b = v1;
                        break;
                    case 2:
                        r = v1;
                        g = other.Brightness;
                        b = v3;
                        break;
                    case 3:
                        r = v1;
                        g = v2;
                        b = other.Brightness;
                        break;
                    case 4:
                        r = v3;
                        g = v1;
                        b = other.Brightness;
                        break;
                    default:
                        r = other.Brightness;
                        g = v1;
                        b = v2;
                        break;
                }
            }

            Red = (byte) Math.Round(r * 255D);
            Green = (byte) Math.Round(g * 255D);
            Blue = (byte) Math.Round(b * 255D);
        }

        /// <summary>
        ///     Convert a hex string into a color
        /// </summary>
        /// <param name="hex">The hex string</param>
        /// <returns>A color</returns>
        private static byte HexToByte(string hex)
        {
            return Convert.ToByte("0x" + hex, 16);
        }

        /// <summary>
        ///     The string representation of the color
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "#" + Hex;
        }

        /// <summary>
        ///     Create a new color from a hex representation
        /// </summary>
        /// <param name="hex">The hex string</param>
        /// <returns></returns>
        public static Rgb FromHex(string hex)
        {
            var red = HexToByte(hex.Substring(0, 2));
            var gre = HexToByte(hex.Substring(2, 2));
            var blu = HexToByte(hex.Substring(4, 2));

            return new Rgb(red, gre, blu);
        }

        /// <summary>
        ///     The hex representation of this color
        /// </summary>
        public string Hex => Red.ToString("X2") + Green.ToString("X2") + Blue.ToString("X2");

        /// <summary>
        ///     True if two colors are the same
        /// </summary>
        /// <param name="obj">The object to compare to</param>
        /// <returns></returns>
        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;
            return this == (Rgb) obj;
        }

        /// <summary>
        ///     Get the hash code of this color
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return Red.GetHashCode() ^ Green.GetHashCode() ^ Blue.GetHashCode();
        }
    }
}