

using System;

namespace ModelHelper.Core.Project
{
    [Obsolete]
    public interface IProjectWriter<T>
    {
        void Write(string path, T project);
    }
}