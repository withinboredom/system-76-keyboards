using System.Threading.Tasks;
using keyboards;
using keyboards.Monitors;

namespace UnitTests
{
    public class FakeMonitor : Monitor
    {
        private double _reading;

        public FakeMonitor(IControlContainer container) : base(container)
        {
        }

        public double Reading
        {
            get => _reading;
            set
            {
                _reading = value;
                CheckForChanges();
            }
        }

        protected override Task<double> GetReading()
        {
            return Task.FromResult(Reading);
        }
    }
}