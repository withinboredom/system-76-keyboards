using System.Threading.Tasks;

namespace keyboards
{
    public interface IFile
    {
        bool Exists { get; }

        bool HasPermission { get; }

        string Contents { get; }

        Task Commit(string contents);

        Task<string> Read();

        Task<string[]> Lines();

        void Delete();
    }
}