using System.Threading.Tasks;
using keyboards.ColorSpace;
using keyboards.Monitors;

namespace keyboards.Filters
{
    public class PowerFilter : IFilter
    {
        private double _brightness;

        public PowerFilter(IControlContainer container)
        {
            Display.Instance(container).Changed += DisplayOnChanged;
        }

        public Task PreApply(long time)
        {
            return Task.CompletedTask;
        }

        public Task<Rgb> ApplyFilter(Rgb color)
        {
            if (_brightness > 0.95D) return Task.FromResult(color);

            var newColor = new Hsb(color);
            var newBrightness = _brightness * newColor.Brightness;
            if (newBrightness < 0.01D) newBrightness = 0;

            return Task.FromResult(new Rgb(newColor.SetBrightness(newBrightness)));
        }

        private void DisplayOnChanged(object? sender, double e)
        {
            _brightness = e / 100D;
        }
    }
}