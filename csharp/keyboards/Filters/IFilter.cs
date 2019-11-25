using System.Threading.Tasks;
using keyboards.ColorSpace;

namespace keyboards.Filters
{
    /// <summary>
    ///     Apply a filter to the colors
    /// </summary>
    public interface IFilter
    {
        /// <summary>
        ///     Called after render but before filtering, exactly once
        /// </summary>
        /// <param name="time">The time of the render</param>
        /// <returns></returns>
        Task PreApply(long time);

        /// <summary>
        ///     Applies the filter to a color, called per side
        /// </summary>
        /// <param name="color">The color to filter</param>
        /// <returns></returns>
        Task<Rgb> ApplyFilter(Rgb color);
    }
}