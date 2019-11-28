using keyboards.ColorSpace;
using keyboards.Sides;
using Microsoft.Extensions.Configuration;

namespace keyboards.Keyboards
{
    /// <inheritdoc />
    public class SolidColor : Keyboard
    {
        private readonly SolidColorConfiguration _colorConfiguration;

        public SolidColor(IControlContainer container, IConfiguration configuration) : base(container)
        {
            _colorConfiguration = configuration.GetSection("SolidColor").Get<SolidColorConfiguration>();
            Init();
        }

        private void Init()
        {
            Sides = new[]
            {
                new Solid(Rgb.FromHex(_colorConfiguration.LeftOrSingle)),
                new Solid(Rgb.FromHex(_colorConfiguration.Center)),
                new Solid(Rgb.FromHex(_colorConfiguration.Right))
            };
        }

        // ReSharper disable once ClassNeverInstantiated.Local
        private class SolidColorConfiguration
        {
            public string LeftOrSingle { get; } = "#ABABAB";
            public string Center { get; } = "#ABABAB";
            public string Right { get; } = "#ABABAB";
        }
    }
}