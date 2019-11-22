using System;

namespace keyboards.ColorSpace
{
    public struct Rgb
    {
        public static readonly Rgb Empty = new Rgb();

        public static bool operator ==(Rgb i1, Rgb i2)
        {
            return i1.Red == i2.Red &&
                   i1.Green == i2.Green &&
                   i1.Blue == i2.Blue;
        }

        public static bool operator !=(Rgb i1, Rgb i2)
        {
            return i1.Red != i2.Red ||
                   i1.Green != i2.Green ||
                   i1.Blue != i2.Blue;
        }

        public byte Red { get; }
        public byte Green { get; }
        public byte Blue { get; }

        public Rgb(byte r, byte g, byte b)
        {
            Red = r;
            Green = g;
            Blue = b;
        }

        public Rgb(Hsb other)
        {
            double r = 0, b = 0, g = 0;

            if (other.Brightness < 0)
                r = b = g = 0;
            else if (other.Saturation < 0)
                r = g = b = other.Brightness;
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
        /// Convert a hex string into a color
        /// </summary>
        /// <param name="hex">The hex string</param>
        /// <returns>A color</returns>
        private static byte HexToByte(string hex)
        {
            return Convert.ToByte("0x" + hex, 16);
        }

        public override string ToString()
        {
            return "#" + Hex;
        }

        public static Rgb FromHex(string hex)
        {
            var red = HexToByte(hex.Substring(0,2));
            var gre = HexToByte(hex.Substring(2, 2));
            var blu = HexToByte(hex.Substring(4, 2));
            
            return new Rgb(red, gre, blu);
        }

        public string Hex => Red.ToString("X2") + Green.ToString("X2") + Blue.ToString("X2");

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;
            return this == (Rgb) obj;
        }

        public override int GetHashCode()
        {
            return Red.GetHashCode() ^ Green.GetHashCode() ^ Blue.GetHashCode();
        }
    }
}