using keyboards.Monitors;
using keyboards.Sides;
using Microsoft.Extensions.Configuration;

namespace keyboards.Keyboards
{
    /// <summary>
    ///     Represents a rainbow keyboard
    /// </summary>
    public class Rainbow : Keyboard
    {
        private class RainbowConfiguration
        {
            public double RedPhase { get; set; } = 0.0003;
            public double GreenPhase { get; set; } = 0.001;
            public double BluePhase { get; set; } = 0.0007;
            public long Step { get; set; } = 100;
        }

        private readonly RainbowConfiguration _configuration = new RainbowConfiguration();
        
        private void Init(IControlContainer container)
        {
            Sides = new[]
            {
                new RainbowSide(Time.Instance(container), _configuration.RedPhase, _configuration.GreenPhase, _configuration.BluePhase, 0),
                new RainbowSide(Time.Instance(container), _configuration.RedPhase, _configuration.GreenPhase, _configuration.BluePhase, _configuration.Step),
                new RainbowSide(Time.Instance(container), _configuration.RedPhase, _configuration.GreenPhase, _configuration.BluePhase, _configuration.Step * 2)
            };
        }
        
        public Rainbow(IControlContainer container, IConfiguration options) : base(container)
        {
            _configuration = options.GetSection("Rainbow").Get<RainbowConfiguration>();
            Init(container);
        }
        
        /// <summary>
        ///     Construct the rainbow!
        /// </summary>
        public Rainbow(IControlContainer container) : base(container)
        {
            Init(container);
        }
    }
}