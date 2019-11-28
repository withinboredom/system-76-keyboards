using System.Threading.Tasks;
using keyboards.ColorSpace;
using keyboards.Filters;
using NUnit.Framework;

namespace UnitTests.Filters
{
    public class WashedOutTests
    {
        [Test]
        public async Task IncreasesBrightness()
        {
            var color = Hsb.Empty;
            var filter = new WashedOut();
            var returned = await filter.ApplyFilter(new Rgb(color));
            Assert.AreEqual(color.SetBrightness(1), new Hsb(returned));
        }
    }
}