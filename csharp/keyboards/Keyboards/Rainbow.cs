using keyboards.Monitors;
using keyboards.Sides;

namespace keyboards.Keyboards
{
    /// <summary>
    ///     Represents a rainbow keyboard
    /// </summary>
    public class Rainbow : Keyboard
    {
        /// <summary>
        ///     Construct the rainbow!
        /// </summary>
        public Rainbow(IControlContainer container) : base(container)
        {
            const double rp = 0.0003;
            const double gp = 0.001;
            const double bp = 0.0007;

            Sides = new[]
            {
                new RainbowSide(Time.Instance(container),  rp, gp, bp, 0),
                new RainbowSide(Time.Instance(container),rp, gp, bp, 100),
                new RainbowSide(Time.Instance(container),rp, gp, bp, 200)
            };
        }
    }
}