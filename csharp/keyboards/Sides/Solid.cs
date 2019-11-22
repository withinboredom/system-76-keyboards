using System.Threading.Tasks;
using keyboards.ColorSpace;

namespace keyboards.Sides
{
    public class Solid : Side
    {
        public Solid(Rgb color, string filename) : base(filename)
        {
            CurrentColor = color;
        }

        public override async Task Render(long time, long deltaTime)
        {
        }
    }
}