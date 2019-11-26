using System.Threading.Tasks;
using keyboards.ColorSpace;
using keyboards.Filters;
using keyboards.Monitors;
using NUnit.Framework;

namespace UnitTests.Filters
{
    public class PowerFilterTests
    {
        private FakeMonitor GetMonitor()
        {
            var container = new FakeContainer();
            return new FakeMonitor(container);
        }
        
        [Test]
        public async Task HundredNoChangeColor()
        {
            var monitor = GetMonitor();
            monitor.Reading = 100;
            
            var filter = new PowerFilter(new FakeContainer(), monitor);
            await filter.PreApply(0);
            var color = await filter.ApplyFilter(Rgb.FromHex("FFFFFF"));
            Assert.AreEqual(Rgb.FromHex("FFFFFF"), color);
        }
    }
}