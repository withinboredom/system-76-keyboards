using System.Collections.Generic;
using keyboards.Monitors;

namespace keyboards
{
    public class ControlContainer : IControlContainer
    {
        public IFile File(string filename)
        {
            return new FastFile(filename);
        }

        public HashSet<IMonitor> Monitors { get; set; } = new HashSet<IMonitor>();
    }
}