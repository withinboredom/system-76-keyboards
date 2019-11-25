using System.Collections.Generic;
using keyboards;
using keyboards.Monitors;

namespace UnitTests
{
    public class FakeContainer : IControlContainer
    {
        public HashSet<IMonitor> Monitors { get; set; }

        public IFile File(string filename)
        {
            return new FakeFile(filename);
        }
    }
}