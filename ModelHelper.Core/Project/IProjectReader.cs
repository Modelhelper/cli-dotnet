namespace ModelHelper.Core.Project
{
    public interface IProjectReader
    {
        string CurrentVersion { get;  }
        IProject Read(string path);
        ProjectVersion CheckVersion(string path);


    }

    public interface IProjectConverter
    {
        IProject Convert(string path);
    }
    public interface IProjectVersionChecker
    {
        VersionCheckerResult Check(string path);
    }

    public class VersionCheckerResult
    {
        public bool MustConvert { get; set; }

    }

    public class ProjectVersion
    {
        public string Version { get; set; }
        public bool IsBeta
        {
            get { return string.IsNullOrEmpty(Version); }
        }


    }
}