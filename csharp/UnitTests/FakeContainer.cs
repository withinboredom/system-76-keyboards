using keyboards;

namespace UnitTests
{
    public class FakeContainer : IControlContainer
    {
        public IFile File(string filename)
        {
            return new FakeFile(filename);
        }
    }
}