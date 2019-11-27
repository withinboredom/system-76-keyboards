using keyboards.ColorSpace;
using keyboards.Sides;
using Microsoft.Extensions.Configuration;

namespace keyboards.Keyboards
{
    public class SolidColor : Keyboard
    {
        public static SolidColor ParseConfiguration(IControlContainer container, IConfigurationSection section)
        {
            var colors = section.GetSection("Colors");
            var left = colors.GetSection("Left");
            var center = colors.GetSection("Center");
            var right = colors.GetSection("Right");

            return new SolidColor(container)
            {
                Sides = new[]
                {
                    new Solid(Rgb.FromHex(left.Value)),
                    new Solid(Rgb.FromHex(center.Value)),
                    new Solid(Rgb.FromHex(right.Value)),
                }
            };
        }
        
        public SolidColor(IControlContainer container) : base(container)
        {
            return;/*
            Sides = new[]
            {
                new Solid(color),
                new Solid(color),
                new Solid(color)
            };*/
        }
    }
}