using keyboards.ColorSpace;
using keyboards.Sides;

namespace keyboards.Keyboards
{
    public class SolidColor : Keyboard
    {
        public SolidColor(IControlContainer container, Rgb color) : base(container)
        {
            Sides = new[]
            {
                new Solid(color),
                new Solid(color),
                new Solid(color)
            };
        }
    }
}