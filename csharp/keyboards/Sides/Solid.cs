using System.Threading.Tasks;
using keyboards.ColorSpace;

namespace keyboards.Sides
{
    public class Solid : Side
    {
        public Solid(Rgb color, IFile file) : base(file)
        {
            CurrentColor = color;
        }

        public override Task Render(long time, long deltaTime)
        {
            return Task.CompletedTask;
        }
    }
}