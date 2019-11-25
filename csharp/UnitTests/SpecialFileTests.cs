using System.IO;
using System.Threading.Tasks;
using keyboards;
using NUnit.Framework;

namespace UnitTests
{
    public class SpecialFileTests
    {
        private const string NoExistsPath = "noexists";
        private static string ExistsPath => Path.GetTempFileName();

        private static string NoPermsPath
        {
            get
            {
                var path = Path.GetTempFileName();
                File.SetAttributes(path, FileAttributes.ReadOnly);
                return path;
            }
        }

        [Test]
        public void CanCheckExists()
        {
            var exists = new SpecialFile(ExistsPath);
            var noExists = new SpecialFile(NoExistsPath);

            Assert.IsTrue(exists.Exists);
            Assert.IsFalse(noExists.Exists);
        }

        [Test]
        public void HasWritePermissionsWithWritableFile()
        {
            var canWrite = new SpecialFile(ExistsPath);

            Assert.IsTrue(canWrite.HasPermission);
        }

        [Test]
        public void NoPermissionsWithReadOnlyFile()
        {
            var noWrite = new SpecialFile(NoPermsPath);
            Assert.IsFalse(noWrite.HasPermission);
        }

        [Test]
        public void PermissionsToNonExistingFile()
        {
            var noExists = new SpecialFile(NoExistsPath);
            Assert.IsTrue(noExists.HasPermission);
        }

        [Test]
        public async Task CanWriteToRegularFile()
        {
            var exists = new SpecialFile(Path.GetTempFileName());
            await exists.Commit("test");
            Assert.AreEqual("test", await exists.Read());
            Assert.AreEqual("test", exists.Contents);
            Assert.AreEqual("test", (await exists.Lines())[0]);
        }

        [Test]
        public async Task CannotWriteToUnwritableFile()
        {
            var no = new SpecialFile(NoPermsPath);
            await no.Commit("test");
            Assert.AreEqual("", no.Contents);
        }

        [Test]
        public void CanDeleteNonExistentFile()
        {
            Assert.IsFalse(File.Exists(NoExistsPath));
            var no = new SpecialFile(NoExistsPath);
            no.Delete();
            Assert.DoesNotThrow(() => no.Delete());
        }

        [Test]
        public void CanDeleteRegularFile()
        {
            var path = ExistsPath;
            Assert.IsTrue(File.Exists(path));
            var file = new SpecialFile(path);
            Assert.DoesNotThrow(() => file.Delete());
            Assert.IsFalse(File.Exists(path));
        }

        [Test]
        public void CanDeleteReadOnlyFile()
        {
            var path = NoPermsPath;
            Assert.IsTrue(File.Exists(path));
            var file = new SpecialFile(path);
            Assert.DoesNotThrow(() => file.Delete());
            Assert.IsTrue(File.Exists(path));
        }
    }
}