using System;
using System.Threading;
using System.Threading.Tasks;
using keyboards.Filters;
using keyboards.Sides;

namespace keyboards.Keyboards
{
    /// <summary>
    ///     Represents an entire keyboard
    /// </summary>
    public class Keyboard
    {
        /// <summary>
        ///     Default location of the center side of the keyboard
        /// </summary>
        protected readonly IFile CenterFile = new SpecialFile("/sys/class/leds/system76::kbd_backlight/color_center");

        /// <summary>
        ///     Default location of the left side of the keyboard
        /// </summary>
        protected readonly IFile LeftFile = new SpecialFile("/sys/class/leds/system76::kbd_backlight/color_left");

        /// <summary>
        ///     Default location of the right side of the keyboard
        /// </summary>
        protected readonly IFile RightFile = new SpecialFile("/sys/class/leds/system76::kbd_backlight/color_right");

        /// <summary>
        ///     Default location for a single color zone
        /// </summary>
        protected readonly IFile SingleFile = new SpecialFile("/sys/class/leds/system76_acpi::kbd_backlight/color");

        /// <summary>
        ///     The update frequency
        /// </summary>
        public double Frequency { get; set; }

        public IFilter[] Filters { get; set; } = { };

        /// <summary>
        ///     The left side of the keyboard
        /// </summary>
        protected Side? Left { get; set; }

        /// <summary>
        ///     The center side
        /// </summary>
        protected Side? Center { get; set; }

        /// <summary>
        ///     The right side of the keyboard
        /// </summary>
        protected Side? Right { get; set; }

        /// <summary>
        ///     The single color zone
        /// </summary>
        protected Side? Single { get; set; }

        /// <summary>
        ///     Renders a keyboard
        /// </summary>
        /// <param name="time">The current time in milliseconds</param>
        /// <param name="deltaTime"></param>
        private async Task Render(long time, long deltaTime)
        {
            await Task.WhenAll(Left == null ? Task.CompletedTask : Left.Render(time, deltaTime),
                Center == null ? Task.CompletedTask : Center.Render(time, deltaTime),
                Right == null ? Task.CompletedTask : Right.Render(time, deltaTime),
                Single == null ? Task.CompletedTask : Single.Render(time, deltaTime));
        }

        /// <summary>
        ///     Run the keyboard
        /// </summary>
        /// <param name="token">A cancellation token to stop</param>
        /// <returns></returns>
        public async Task<int> Run(CancellationToken token)
        {
            var lastTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            while (!token.IsCancellationRequested)
            {
                var startRender = DateTime.Now;
                var time = startRender.Ticks / TimeSpan.TicksPerMillisecond;
                await Render(time, time - lastTime);
                foreach (var filter in Filters) await filter.PreApply(time);
                await Task.WhenAll(Left == null ? Task.CompletedTask : Left.Commit(Filters),
                    Center == null ? Task.CompletedTask : Center.Commit(Filters),
                    Right == null ? Task.CompletedTask : Right.Commit(Filters),
                    Single == null ? Task.CompletedTask : Single.Commit(Filters));
                var timeToNext = startRender + TimeSpan.FromSeconds(Frequency) - DateTime.Now;
                if (timeToNext.Ticks > 0)
                    await Task.Delay(timeToNext, token);
            }

            return 0;
        }
    }
}