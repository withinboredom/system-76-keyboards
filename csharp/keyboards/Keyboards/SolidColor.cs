using keyboards.ColorSpace;
using keyboards.Sides;
using Microsoft.Extensions.Configuration;

namespace keyboards.Keyboards
{
    /// <inheritdoc />
    public class SolidColor : Keyboard
    {
        private class SolidColorConfiguration
        {
            public string LeftOrSingle { get; set; } = "#ABABAB";
            public string Center { get; set; } = "#ABABAB";
            public string Right { get; set; } = "#ABABAB";
        }

        private readonly SolidColorConfiguration _colorConfiguration = new SolidColorConfiguration();
        
        private void Init()
        {
            Sides = new[]
            {
                new Solid(Rgb.FromHex(_colorConfiguration.LeftOrSingle)),
                new Solid(Rgb.FromHex(_colorConfiguration.Center)),
                new Solid(Rgb.FromHex(_colorConfiguration.Right)),
            };
        }

        public SolidColor(IControlContainer container, IConfiguration configuration) : base(container)
        {
            _colorConfiguration = configuration.GetSection("SolidColor").Get<SolidColorConfiguration>();
            Init();
        }

        public SolidColor(IControlContainer container) : base(container)
        {
            Init();
        }
    }
}