using System.Threading.Tasks;
using keyboards.Monitors;

namespace keyboards.Sides
{
    public class DiskSide : MonitorSide
    {
        private readonly MovingAverage _average;
        private readonly Disk _disk;

        public DiskSide(double red = 90, double yellow = 70, double green = 50) : base(red, yellow, green)
        {
            _average = new MovingAverage();
            _disk = new Disk();
        }

        protected override async Task<double> GetValue()
        {
            var usage = await _disk.Percentage();
            usage = _average.GetAverage(usage);

            return usage;
        }
    }
}