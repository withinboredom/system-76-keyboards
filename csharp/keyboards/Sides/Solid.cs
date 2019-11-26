using System.Threading.Tasks;
using keyboards.ColorSpace;

namespace keyboards.Sides
{
    public class Solid : Side
    {
        public Solid(Rgb color)
        {
            CurrentColor = color;
        }
    }
}