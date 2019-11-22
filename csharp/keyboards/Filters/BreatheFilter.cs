using System;
using keyboards.ColorSpace;

namespace keyboards.Filters
{
    public class BreatheFilter : IFilter
    {
        public Rgb ApplyFilter(Rgb color)
        {
            var c = new Hsb(color);
            
            var next = new Rgb(c.SetBrightness(c.Brightness / 8D));

            return next;
        }
    }
}