using System.Collections.Generic;
using System.Threading.Tasks;
using keyboards.ColorSpace;
using keyboards.Filters;

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
        ///     The active led this side is attached to
        /// </summary>
        IFile? Led { get; set; }

        Task Commit(IEnumerable<IFilter> filters);

        /// <summary>
        ///     Load the active led value
        /// </summary>
        /// <returns></returns>
        Task Load();
    }
}