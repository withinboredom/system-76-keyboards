using System.IO;
using System.Threading.Tasks;
using keyboards.Sides;

namespace keyboards
{
    /// <summary>
    /// A side of the keyboard
    /// </summary>
    public abstract class Side : ISide
    {
        /// <summary>
        /// The file to read/save from
        /// </summary>
        private readonly string? _file;

        /// <summary>
        /// Commit this side to the hardware
        /// </summary>
        /// <returns></returns>
        public Task Commit()
        {
            if(_file == null) return Task.CompletedTask;
            var hex = CurrentColor?.Hex;
            return System.IO.File.WriteAllTextAsync(_file, hex);
        }

        /// <summary>
        /// Load side from the hardware
        /// </summary>
        /// <returns></returns>
        protected async Task Load()
        {
            if (_file == null) return;
            var hex = await System.IO.File.ReadAllTextAsync(_file);
            var red = Color.FromHex(hex.Substring(0,2));
            var gre = Color.FromHex(hex.Substring(2, 2));
            var blu = Color.FromHex(hex.Substring(4, 2));
            CurrentColor = new Color {Red = red, Green = gre, Blue = blu};
        }

        /// <summary>
        /// The current color
        /// </summary>
        public Color? CurrentColor { get; set; }

        /// <summary>
        /// Renders the side
        /// </summary>
        /// <param name="time"></param>
        public abstract void Render(long time);

        /// <summary>
        /// Creates a new side
        /// </summary>
        /// <param name="filename"></param>
        protected Side(string filename)
        {
            if (System.IO.File.Exists(filename))
            {
                _file = filename;
            }

            Load().Wait();
        }
    }
}