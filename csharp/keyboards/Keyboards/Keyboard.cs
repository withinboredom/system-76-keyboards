using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using keyboards.Filters;
using keyboards.Sides;

namespace keyboards.Keyboards
{
    /// <summary>
    ///     Represents an entire keyboard
    /// </summary>
    public abstract class Keyboard
    {
        private readonly IControlContainer _container;

        public Keyboard(IControlContainer container)
        {
            Sides = new List<Side>();
            _container = container;
        }

        /// <summary>
        ///     The update frequency
        /// </summary>
        public double Frequency { get; set; }

        protected IEnumerable<Side> Sides { get; set; }

        public IFilter[] Filters { get; set; } = { };

        private Task PrepareSides()
        {
            var sides = Sides.ToArray();
            var leds = LedProvider.SupportedConfiguration(_container).ToArray();

            Sides = leds.Zip(sides, (file, side) =>
            {
                side.Led = file;
                return side;
            }).ToArray();

            return Task.WhenAll(Sides.Select(s => s.Load()));
        }

        private Task PreApplyFilters(long time)
        {
            var applies = Filters.Select(f => f.PreApply(time));
            return Task.WhenAll(applies);
        }

        private Task Commit()
        {
            var commits = Sides.Select(s => s.Commit(Filters));
            return Task.WhenAll(commits);
        }

        /// <summary>
        ///     Run the keyboard
        /// </summary>
        /// <param name="token">A cancellation token to stop</param>
        /// <returns></returns>
        public async Task<int> Run(CancellationToken token)
        {
            await PrepareSides();
            
            var update = _container.Monitors.Select(m => m.CheckForChanges());
            var lastTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            while (!token.IsCancellationRequested)
            {
                var startRender = DateTime.Now;
                var time = startRender.Ticks / TimeSpan.TicksPerMillisecond;
                await Task.WhenAll(update);
                await PreApplyFilters(time);
                await Commit();
                var timeToNext = startRender + TimeSpan.FromSeconds(Frequency) - DateTime.Now;
                if (timeToNext.Ticks > 0)
                    await Task.Delay((int)timeToNext.TotalMilliseconds, token);
            }

            return 0;
        }
    }
}