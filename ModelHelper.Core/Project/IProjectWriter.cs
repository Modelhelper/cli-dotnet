using ModelHelper.Core.Project.V1;

namespace ModelHelper.Core.Project
{
    public interface IProjectWriter<T>
    {
        void Write(string path, T project);
    }
}