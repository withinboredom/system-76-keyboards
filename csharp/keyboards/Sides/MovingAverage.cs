using System.Collections.Generic;
using System.Linq;

namespace keyboards.Sides
{
    public class MovingAverage
    {
        private readonly Queue<double> _measures = new Queue<double>(10);

        public double GetAverage(double newValue)
        {
            if (_measures.Count > 10)
                _measures.Dequeue();
            
            _measures.Enqueue(newValue);
            return _measures.Average();
        }
    }
}