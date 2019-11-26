using keyboards.Monitors;
using keyboards.Sides;

namespace keyboards.Keyboards
{
    public class Monitor : Keyboard
    {
        public Monitor(IControlContainer container) : base(container)
        {
            Sides = new Side[]
            {
                new MonitorSide(Cpu.Instance(container), 95, 75),
                new MonitorSide(Memory.Instance(container), 80, 45, 15),
                new MonitorSide(Disk.Instance(container), 90, 66, 30)
            };
        }
    }
}