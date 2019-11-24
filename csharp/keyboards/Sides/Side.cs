using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using keyboards.ColorSpace;
using keyboards.Filters;
using keyboards.Sides;

namespace keyboards
{
    /// <summary>
    ///     A side of the keyboard
    /// </summary>
    public abstract class Side : ISide
    {
        /// <summary>
        ///     The file to read/save from
        /// </summary>
        private readonly string? _file;

        /// <summary>
        ///     Creates a new side
        /// </summary>
        /// <param name="filename"></param>
        protected Side(string filename)
        {
            if (File.Exists(filename)) _file = filename;

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
            if (_file == null) return;

            var commitColor = CurrentColor;
            foreach (var filter in filters) commitColor = await filter.ApplyFilter(commitColor);

            var hex = commitColor.Hex;
            await File.WriteAllTextAsync(_file, hex);
        }

        /// <summary>
        ///     Load side from the hardware
        /// </summary>
        /// <returns></returns>
        protected async Task Load()
        {
            if (_file == null) return;
            var hex = await File.ReadAllTextAsync(_file);
            CurrentColor = Rgb.FromHex(hex);
        }
    }
}