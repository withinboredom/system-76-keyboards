using System.Threading.Tasks;
using keyboards;
using keyboards.Monitors;

namespace UnitTests
{
    public class FakeMonitor : Monitor
    {
        public FakeMonitor(IControlContainer container) : base(container)
        {
        }
        
        public double Reading { get; set; }

        protected override Task<double> GetReading()
        {
            return Task.FromResult(Reading);
        }
    }
}