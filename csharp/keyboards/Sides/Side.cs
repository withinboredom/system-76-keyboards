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
        protected bool IsDirty { get; set; } = true;
        
        /// <summary>
        ///     The current color
        /// </summary>
        public Rgb CurrentColor { get; set; }
        
        public IFile? Led { get; set; }

        /// <summary>
        ///     Load side from the hardware
        /// </summary>
        /// <returns></returns>
        public async Task Load()
        {
            if (Led != null)
            {
                var hex = await Led.Read();
                CurrentColor = Rgb.FromHex(hex);
            }
        }

        /// <summary>
        ///     Commit this side to the hardware
        /// </summary>
        /// <returns></returns>
        public async Task Commit(IEnumerable<IFilter> filters)
        {
            if (!IsDirty) return;
            
            var commitColor = CurrentColor;
            foreach (var filter in filters) commitColor = await filter.ApplyFilter(commitColor);

            var hex = commitColor.Hex;
            if (Led != null) await Led.Commit(hex);
            IsDirty = false;
        }
    }
}