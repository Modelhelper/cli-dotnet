

namespace ModelHelper.Project
{
    public interface IProjectWriter<T>
    {
        void Write(string path, T project);
    }
}