namespace keyboards
{
    public interface IControlContainer
    {
        /// <summary>
        /// Get an IFile for a File
        /// </summary>
        /// <param name="filename">The file to use</param>
        /// <returns></returns>
        IFile File(string filename);
    }
}