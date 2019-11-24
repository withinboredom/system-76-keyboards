using System.Threading.Tasks;
using keyboards.ColorSpace;
using keyboards.Filters;
using NUnit.Framework;

namespace UnitTests.Filters
{
    public class BlackWhiteTests
    {
        [Test]
        public async Task ReducesSaturation()
        {
            var color = new Rgb(0, 255, 0);
            var filter = new BlackWhite();
            var newColor = await filter.ApplyFilter(color);
            Assert.AreEqual(Rgb.FromHex("BFFFBF"), newColor);
        }
    }
}