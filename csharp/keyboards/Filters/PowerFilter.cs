using System.Threading.Tasks;
using keyboards.ColorSpace;
using keyboards.Monitors;

namespace keyboards.Filters
{
    public class PowerFilter : IFilter
    {
        private IMonitor _display;
        private double _brightness;
        
        public PowerFilter(IControlContainer container)
        {
            _display = Display.Instance(container);
            _display.Changed += DisplayOnChanged;
        }

        private void DisplayOnChanged(object? sender, double e)
        {
            _brightness = e / 100D;
        }

        public Task PreApply(long time)
        {
            return Task.CompletedTask;
        }

        public Task<Rgb> ApplyFilter(Rgb color)
        {
            var newColor = new Hsb(color);
            var newBrightness = _brightness * newColor.Brightness;
            return Task.FromResult(new Rgb(newColor.SetBrightness(newBrightness)));
        }
    }
}