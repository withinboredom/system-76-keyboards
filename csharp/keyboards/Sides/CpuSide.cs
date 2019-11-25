using System.Threading.Tasks;
using keyboards.Monitors;

namespace keyboards.Sides
{
    public class CpuSide : MonitorSide
    {
        private readonly MovingAverage _average;
        private readonly Cpu _monitor;

        public CpuSide(double red = 90, double yellow = 70, double green = 50) : base(red, yellow, green)
        {
            _average = new MovingAverage();
            _monitor = new Cpu();
        }

        protected override async Task<double> GetValue()
        {
            var usage = await _monitor.Percentage();

            usage = _average.GetAverage(usage);

            return usage;
        }
    }
}