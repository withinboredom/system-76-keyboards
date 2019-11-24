using System.Threading.Tasks;
using keyboards.ColorSpace;

namespace keyboards.Filters
{
    /// <summary>
    ///     Attempt to create a "washed out" look
    /// </summary>
    public class WashedOut : IFilter
    {
        /// <summary>
        ///     Called after render but before filtering, exactly once
        /// </summary>
        /// <param name="time">The time of the render</param>
        /// <returns></returns>
        public Task PreApply(long time)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        ///     Applies the filter to a color, called per side
        /// </summary>
        /// <param name="color">The color to filter</param>
        /// <returns></returns>
        public Task<Rgb> ApplyFilter(Rgb color)
        {
            var hsb = new Hsb(color);
            return Task.FromResult(new Rgb(hsb.SetBrightness(1)));
        }
    }
}