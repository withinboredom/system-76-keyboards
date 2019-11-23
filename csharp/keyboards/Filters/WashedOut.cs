using System.Threading.Tasks;
using keyboards.ColorSpace;

namespace keyboards.Filters
{
    public class WashedOut : IFilter
    {
        public Task PreApply(long time)
        {
            return Task.CompletedTask;
        }

        public Task<Rgb> ApplyFilter(Rgb color)
        {
            var hsb = new Hsb(color);
            return Task.FromResult(new Rgb(hsb.SetBrightness(1)));
        }
    }
}