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
        public Rainbow()
        {
            const double rp = 0.0003;
            const double gp = 0.001;
            const double bp = 0.0007;

            Left = new RainbowSide(rp, gp, bp, 0, LeftFile);
            Center = new RainbowSide(rp, gp, bp, 100, CenterFile);
            Right = new RainbowSide(rp, gp, bp, 200, RightFile);
            Single = new RainbowSide(rp, gp, bp, 0, SingleFile);
        }
    }
}