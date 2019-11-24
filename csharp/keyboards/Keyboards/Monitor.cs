using keyboards.Sides;

namespace keyboards.Keyboards
{
    public class Monitor : Keyboard
    {
        public Monitor()
        {
            Left = new CpuSide(LeftFile, 95, 75);
            Center = new MemSide(CenterFile, 80, 45, 15);
            Right = new DiskSide(RightFile, 90, 66, 30);
            Single = new CpuSide(SingleFile, 95, 75);
        }
    }
}