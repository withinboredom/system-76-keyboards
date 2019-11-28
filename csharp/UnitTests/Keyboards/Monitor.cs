using keyboards.Keyboards;
using NUnit.Framework;

namespace UnitTests.Keyboards
{
    public class MonitorTests
    {
        [Test]
        public void CanBeCreated()
        {
            var monitor = new Monitor(new FakeContainer(), null);
            Assert.IsTrue(monitor.Filters.Length == 0);
        }
    }
}