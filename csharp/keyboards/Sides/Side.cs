using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using keyboards.ColorSpace;
using keyboards.Filters;

namespace keyboards.Sides
{
    /// <summary>
    ///     A side of the keyboard
    /// </summary>
    public abstract class Side : ISide
    {
        /// <summary>
        ///     The current color
        /// </summary>
        public Rgb CurrentColor { get; set; }

        /// <summary>
        ///     Renders the side
        /// </summary>
        /// <param name="time"></param>
        /// <param name="deltaTime"></param>
        public abstract Task Render(long time, long deltaTime);

        public IFile Led { get; set; }

        /// <summary>
        ///     Commit this side to the hardware
        /// </summary>
        /// <returns></returns>
        public async Task Commit(IEnumerable<IFilter> filters)
        {
            var commitColor = CurrentColor;
            foreach (var filter in filters) commitColor = await filter.ApplyFilter(commitColor);

            var hex = commitColor.Hex;
            await Led.Commit(hex);
        }

        /// <summary>
        ///     Load side from the hardware
        /// </summary>
        /// <returns></returns>
        public async Task Load()
        {
            var hex = await Led.Read();
            CurrentColor = Rgb.FromHex(hex);
        }
    }
}