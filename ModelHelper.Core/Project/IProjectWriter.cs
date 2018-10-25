namespace ModelHelper.Core.Project
{
    public interface IProjectWriter
    {
        void Write(string path, IProject project);
    }
}