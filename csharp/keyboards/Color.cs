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

        private double sRGBToLin(double colorChannel)
        {
            if (colorChannel <= 0.04045D)
            {
                return colorChannel / 12.92D;
            }

            return Math.Pow((colorChannel + 0.055D) / 1.055D, 2.4D);
        }

        public double Luminance
        {
            get
            {
                var vR = Red / 255D;
                var vG = Green / 255D;
                var vB = Blue / 255D;

                var y = 0.2126D * sRGBToLin(vR) + 0.7152D * sRGBToLin(vG) + 0.0722D * sRGBToLin(vB);
                return y;
            }
        }
    }
}