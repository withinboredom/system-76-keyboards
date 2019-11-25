using System.Collections.Generic;
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
        private readonly bool _disabled;

        /// <summary>
        ///     The file to read/save from
        /// </summary>
        private readonly IFile _file;

        /// <summary>
        ///     Creates a new side
        /// </summary>
        /// <param name="file"></param>
        protected Side(IFile file)
        {
            _file = file;
            _disabled = !file.Exists;

            Load().Wait();
        }

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

        /// <summary>
        ///     Commit this side to the hardware
        /// </summary>
        /// <returns></returns>
        public async Task Commit(IEnumerable<IFilter> filters)
        {
            if (_disabled) return;

            var commitColor = CurrentColor;
            foreach (var filter in filters) commitColor = await filter.ApplyFilter(commitColor);

            var hex = commitColor.Hex;
            await _file.Commit(hex);
        }

        /// <summary>
        ///     Load side from the hardware
        /// </summary>
        /// <returns></returns>
        protected async Task Load()
        {
            if (_disabled) return;
            var hex = await _file.Read();
            CurrentColor = Rgb.FromHex(hex);
        }
    }
}