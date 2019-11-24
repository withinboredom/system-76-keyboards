using keyboards.ColorSpace;
using NUnit.Framework;

namespace UnitTests.ColorSpace
{
    public class RgbTests
    {
        [Test]
        public void EmptyColorIsBlack()
        {
            var color = Rgb.Empty;
            Assert.AreEqual(new Rgb(0, 0, 0), color);
        }

        [Test]
        public void CanCompareColors()
        {
            var c1 = new Rgb(100, 100, 100);
            var c2 = new Rgb(100, 100, 100);
            Assert.IsTrue(c1 == c2);
            Assert.IsFalse(c1 != c2);
            Assert.AreEqual(c1.GetHashCode(), c2.GetHashCode());
            Assert.IsTrue(c1.Equals(c2));
        }

        [Test]
        public void CanCopyColor()
        {
            var color = new Rgb(200, 200, 200);
            Assert.AreEqual(color, new Rgb(color));
        }

        [Test]
        public void CanConvertHsb()
        {
            var color = new Hsb(0, 0.13157894D, 0.14901960D);
            var expected = new Rgb(38, 33, 33);
            Assert.AreEqual(expected, new Rgb(color));
        }
    }
}