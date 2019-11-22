using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace keyboards
{
    /// <summary>
    /// Represents an entire keyboard
    /// </summary>
    public class Keyboard
    {
        /// <summary>
        /// The update frequency
        /// </summary>
        public double Frequency { get; set; }
        
        /// <summary>
        /// The left side of the keyboard
        /// </summary>
        protected Side Left { get; set; }
        
        /// <summary>
        /// The center side
        /// </summary>
        protected Side Center { get; set; }
        
        /// <summary>
        /// The right side of the keyboard
        /// </summary>
        protected Side Right { get; set; }

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
        private void Render(long time)
        {
            Left?.Render(time);
            Center?.Render(time);
            Right?.Render(time);
        }

        /// <summary>
        /// Run the keyboard
        /// </summary>
        /// <param name="token">A cancellation token to stop</param>
        /// <returns></returns>
        public async Task Run(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                var time = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
                var timeToNext = (DateTime.Now + TimeSpan.FromSeconds(Frequency)) - DateTime.Now;
                Render(time);
                await Task.WhenAll(Left?.Commit(), Center?.Commit(), Right?.Commit());
                await Task.Delay(timeToNext, token);
            }
        }
    }
}