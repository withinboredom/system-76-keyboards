using System.Threading.Tasks;
using keyboards;

namespace UnitTests
{
    public class FakeFile : IFile
    {
        public FakeFile(string filename)
        {
        }

        public bool Exists { get; internal set; }

        public bool HasPermission { get; internal set; }

        public string Contents { get; internal set; }

        public Task Commit(string contents)
        {
            Contents = contents;
            return Task.CompletedTask;
        }

        public Task<string> Read()
        {
            return Task.FromResult(Contents);
        }

        public Task<string[]> Lines()
        {
            return Task.FromResult(Contents.Split('\n'));
        }

        public void Delete()
        {
            Exists = false;
            Contents = string.Empty;
        }
    }
}