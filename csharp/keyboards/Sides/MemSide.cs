using System.Threading.Tasks;
using keyboards.Monitors;

namespace keyboards.Sides
{
    public class MemSide : MonitorSide
    {
        private readonly MovingAverage _average;
        private readonly Memory _memory;

        public MemSide(double red = 90, double yellow = 70, double green = 50) : base(red, yellow, green)
        {
            _average = new MovingAverage();
            _memory = new Memory();
        }

        protected override async Task<double> GetValue()
        {
            var total = await _memory.Percentage();
            return _average.GetAverage(total);
        }
    }
}