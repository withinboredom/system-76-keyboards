using System.Threading.Tasks;
using keyboards;
using keyboards.ColorSpace;
using keyboards.Filters;
using keyboards.Sides;
using NUnit.Framework;

namespace UnitTests.Sides
{
    public class SolidTests
    {
        private readonly IFile _fixtureFile = new SpecialFile("test-color");

        [SetUp]
        public async Task CreateFixture()
        {
            await _fixtureFile.Commit("FF00FF");
        }

        [TearDown]
        public void ResetFixture()
        {
            _fixtureFile.Delete();
        }

        [Test]
        public void TestSolidColor()
        {
            var side = new Solid(Rgb.FromHex("FFFFFF")) {Led = _fixtureFile};
            side.Render(0, 0);
            side.Commit(new IFilter[] { });
            Assert.AreEqual("FFFFFF", side.Led.Contents);
        }
    }
}