using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using keyboards.Monitors;

namespace keyboards.Sides
{
    public class BreatheSide<T> : Side where T : IMonitor, new()
    {
        private T _monitor;
        private readonly List<double> _circle;
        private MovingAverage _average;
        private State internalState;

        enum State
        {
            Breathing,
            Sleeping,
        }

        public BreatheSide(string filename) : base(filename)
        {
            _monitor = new T();
            _average = new MovingAverage();
            _circle = new List<double>();
            var theta = 0D;
            while (theta < Math.PI)
            {
                _circle.Add(Math.Sin(theta));
                theta += 0.01;
            }
            _circle.AddRange(_circle.AsEnumerable().Reverse());
        }

        public override async Task Render(long time, long deltaTime)
        {
            switch (internalState)
            {
                case State.Breathing:
                    break;                    
            }
        }
    }
}