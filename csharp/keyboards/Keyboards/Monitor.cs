using System.Collections.Generic;
using keyboards.Sides;

namespace keyboards.Keyboards
{
    public class Monitor : Keyboard
    {
        public Monitor(IControlContainer container) : base(container)
        {
            Sides = new Side[]
            {
                new CpuSide(95, 75),
                new MemSide(80, 45, 15),
                new DiskSide(90, 66, 30),
            };
        }
    }
}