using keyboards.ColorSpace;
using keyboards.Sides;

namespace keyboards.Keyboards
{
    public class SolidColor : Keyboard
    {
        public SolidColor(Rgb color)
        {
            Left = new Solid(color, LeftFile);
            Center = new Solid(color, CenterFile);
            Right = new Solid(color, RightFile);
        }
    }
}