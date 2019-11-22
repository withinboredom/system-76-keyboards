using System;

namespace keyboards
{
    /// <summary>
    /// A Color
    /// </summary>
    public class Color
    {
        /// <summary>
        /// The red component of the color
        /// </summary>
        public byte Red { get; set; }
        
        /// <summary>
        /// The green component of the color
        /// </summary>
        public byte Green { get; set; }
        
        /// <summary>
        /// The blue component of the color
        /// </summary>
        public byte Blue { get; set; }

        /// <summary>
        /// Convert a hex string into a color
        /// </summary>
        /// <param name="hex">The hex string</param>
        /// <returns>A color</returns>
        public static byte FromHex(string hex)
        {
            return Convert.ToByte("0x" + hex, 16);
        }

        /// <summary>
        /// The hex string of this color
        /// </summary>
        public string Hex => Red.ToString("X2") + Green.ToString("X2") + Blue.ToString("X2");
    }
}