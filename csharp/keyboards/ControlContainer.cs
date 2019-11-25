namespace keyboards
{
    public class ControlContainer : IControlContainer
    {
        public IFile File(string filename)
        {
            return new SpecialFile(filename);
        }
    }
}