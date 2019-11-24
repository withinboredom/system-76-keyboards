using System.Collections.Generic;
using System.Linq;

namespace keyboards.Sides
{
    public class MovingAverage
    {
        private readonly Queue<double> _measures = new Queue<double>(10);
        private readonly byte _numberSamples;

        public MovingAverage(byte numberSamples = 10)
        {
            _numberSamples = numberSamples;
        }

        public double GetAverage(double newValue)
        {
            if (_measures.Count > _numberSamples)
                _measures.Dequeue();

            _measures.Enqueue(newValue);
            return _measures.Average();
        }
    }
}