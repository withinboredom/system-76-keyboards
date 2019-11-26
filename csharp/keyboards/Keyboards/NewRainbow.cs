using keyboards.Monitors;
using keyboards.Sides;

namespace keyboards.Keyboards
{
    public class NewRainbow : Keyboard
    {
        public NewRainbow(IControlContainer container) : base(container)
        {
            Sides = new[]
            {
                new HueSide(Time.Instance(container), 0D),
                new HueSide(Time.Instance(container), 500D),
                new HueSide(Time.Instance(container), 1000D)
            };
        }
    }
}