using System.Linq;
using System.Threading.Tasks;
using keyboards.ColorSpace;
using keyboards.Filters;
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
        public async Task FadeIn()
        {
            var monitor = GetMonitor();
            monitor.Reading = 100;

            var filter = new PowerFilter(new FakeContainer(), monitor);
            await filter.PreApply(0);
            var color = await filter.ApplyFilter(Rgb.FromHex("FFFFFF"));
            Assert.AreEqual(Rgb.FromHex("000000"), color);
        }

        [Test]
        public async Task ProperlyTransitions()
        {
            var monitor = GetMonitor();
            monitor.Reading = 100;

            var filter = new PowerFilter(new FakeContainer(), monitor);
            var color = Rgb.Empty;

            foreach (var i in Enumerable.Range(1, 2))
            {
                await Task.Delay(1000);
                await monitor.CheckForChanges();
                await filter.PreApply(0);
                color = await filter.ApplyFilter(Rgb.FromHex("FFFFFF"));
            }

            Assert.AreEqual(Rgb.FromHex("FFFFFF"), color);

            monitor.Reading = 0;

            foreach (var i in Enumerable.Range(1, 2))
            {
                await Task.Delay(1000);
                await monitor.CheckForChanges();
                await filter.PreApply(0);
                color = await filter.ApplyFilter(Rgb.FromHex("FFFFFF"));
            }

            Assert.AreEqual(Rgb.FromHex("000000"), color);
        }
    }
}