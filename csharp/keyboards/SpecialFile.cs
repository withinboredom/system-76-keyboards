using System.IO;
using System.Threading.Tasks;

namespace keyboards
{
    public class SpecialFile : IFile
    {
        public SpecialFile(string filename)
        {
            Filename = filename;
        }

        private string Filename { get; }

        public bool Exists => File.Exists(Filename);

        public bool HasPermission
        {
            get
            {
                if (Exists) return (File.GetAttributes(Filename) & FileAttributes.ReadOnly) == 0;

                var dir = Path.GetDirectoryName(Path.GetFullPath(Filename));
                return (new DirectoryInfo(dir).Attributes & FileAttributes.ReadOnly) == 0;
            }
        }

        public string Contents => File.ReadAllText(Filename);

        public Task Commit(string contents)
        {
            return HasPermission ? File.WriteAllTextAsync(Filename, contents) : Task.CompletedTask;
        }

        public Task<string> Read()
        {
            return File.ReadAllTextAsync(Filename);
        }

        public Task<string[]> Lines()
        {
            return File.ReadAllLinesAsync(Filename);
        }

        public void Delete()
        {
            if (HasPermission)
                File.Delete(Filename);
        }
    }
}