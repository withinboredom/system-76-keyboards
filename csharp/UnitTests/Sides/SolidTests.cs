using System.IO;
using keyboards.ColorSpace;
using keyboards.Filters;
using keyboards.Sides;
using NUnit.Framework;

namespace UnitTests.Sides
{
    public class SolidTests
    {
        private const string FixtureFile = "test-color";

        [SetUp]
        public void CreateFixture()
        {
            File.WriteAllText(FixtureFile, "FF00FF");
        }

        [TearDown]
        public void ResetFixture()
        {
            File.Delete(FixtureFile);
        }
        
        [Test]
        public void TestSolidColor()
        {
            var side = new Solid(Rgb.FromHex("FFFFFF"), FixtureFile);
            side.Render(0, 0);
            side.Commit(new IFilter[] { });
            Assert.AreEqual("FFFFFF", System.IO.File.ReadAllText(FixtureFile));
        }
    }
}