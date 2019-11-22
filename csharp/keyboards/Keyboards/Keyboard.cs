using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using keyboards.Filters;

namespace keyboards.Keyboards
{
    /// <summary>
    /// Represents an entire keyboard
    /// </summary>
    public class Keyboard
    {
        private IFilter[] _filters = new IFilter[] {};

        /// <summary>
        /// The update frequency
        /// </summary>
        public double Frequency { get; set; }

        public IFilter[] Filters
        {
            get => _filters;
            set => _filters = value;
        }

        /// <summary>
        /// The left side of the keyboard
        /// </summary>
        protected Side? Left { get; set; }
        
        /// <summary>
        /// The center side
        /// </summary>
        protected Side? Center { get; set; }
        
        /// <summary>
        /// The right side of the keyboard
        /// </summary>
        protected Side? Right { get; set; }

        /// <summary>
        /// Default location of the left side of the keyboard
        /// </summary>
        protected const string LeftFile = "/sys/class/leds/system76::kbd_backlight/color_left";
        
        /// <summary>
        /// Default location of the center side of the keyboard
        /// </summary>
        protected const string CenterFile = "/sys/class/leds/system76::kbd_backlight/color_center";
        
        /// <summary>
        /// Default location of the right side of the keyboard
        /// </summary>
        protected const string RightFile = "/sys/class/leds/system76::kbd_backlight/color_right";

        /// <summary>
        /// Renders a keyboard
        /// </summary>
        /// <param name="time">The current time in milliseconds</param>
        private async Task Render(long time, long deltaTime)
        {
            await Task.WhenAll(Left == null ? Task.CompletedTask : Left.Render(time, deltaTime),
                Center == null ? Task.CompletedTask : Center.Render(time, deltaTime),
                Right == null ? Task.CompletedTask : Right.Render(time, deltaTime));
        }

        /// <summary>
        /// Run the keyboard
        /// </summary>
        /// <param name="token">A cancellation token to stop</param>
        /// <returns></returns>
        public async Task Run(CancellationToken token)
        {
            var lastTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            while (!token.IsCancellationRequested)
            {
                var time = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
                var timeToNext = (DateTime.Now + TimeSpan.FromSeconds(Frequency)) - DateTime.Now;
                await Render(time, time - lastTime);
                await Task.WhenAll(Left == null ? Task.CompletedTask : Left.Commit(Filters),
                    Center == null ? Task.CompletedTask : Center.Commit(new IFilter[]{}),
                    Right == null ? Task.CompletedTask : Right.Commit(Filters));
                await Task.Delay(timeToNext, token);
            }
        }
    }
}