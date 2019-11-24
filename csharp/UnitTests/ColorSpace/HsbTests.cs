using keyboards.ColorSpace;
using NUnit.Framework;

namespace UnitTests.ColorSpace
{
    public class HsbTests
    {
        [Test]
        public void EmptyColorIsBlack()
        {
            var color = Hsb.Empty;
            Assert.AreEqual(new Hsb(0, 0, 0), color);
        }

        [Test]
        public void CanCompareColors()
        {
            var c1 = new Hsb(0, 0, 1);
            var c2 = new Hsb(0, 0, 1);
            Assert.IsTrue(c1 == c2);
            Assert.IsFalse(c1 != c2);
            Assert.AreEqual(c1.GetHashCode(), c2.GetHashCode());
            Assert.IsTrue(c1.Equals(c2));
        }

        [Test]
        public void CanCopyColor()
        {
            var c1 = new Hsb(234, 0.2D, 0.5D);
            Assert.AreEqual(c1, new Hsb(c1));
        }

        [Test]
        public void CanConvertRgb()
        {
            var c1 = new Hsb(new Rgb(38, 33, 33));
            Assert.AreEqual(c1, new Hsb(0, 0.13157894D, 0.14901960D));
        }
    }
}