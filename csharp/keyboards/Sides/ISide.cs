using System.Threading.Tasks;
using keyboards.ColorSpace;

namespace keyboards.Sides
{
    /// <summary>
    ///     Represents the side of a keyboard that can be a different color
    /// </summary>
    public interface ISide
    {
        /// <summary>
        ///     The current color of the side
        /// </summary>
        Rgb CurrentColor { get; set; }

        /// <summary>
        ///     Render the current color of the side to memory
        /// </summary>
        /// <param name="time">The current time</param>
        /// <param name="deltaTime">The number of milliseconds since the last render</param>
        Task Render(long time, long deltaTime);
    }
}