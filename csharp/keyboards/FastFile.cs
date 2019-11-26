using System.IO;

namespace keyboards
{
    /// <summary>
    /// A file implementation that assumes the world won't change
    /// </summary>
    public class FastFile : SpecialFile
    {
        private bool? _existsCache;
        private bool? _hasPermissionsCache;
        
        public new bool Exists
        {
            get
            {
                if (_existsCache == null)
                {
                    _existsCache = base.Exists;
                }

                return _existsCache.Value;
            }
        }

        public new bool HasPermission
        {
            get
            {
                if (_hasPermissionsCache == null)
                {
                    _hasPermissionsCache = base.HasPermission;
                }

                return _hasPermissionsCache.Value;
            }
        }

        public FastFile(string filename) : base(filename)
        {
        }
    }
}