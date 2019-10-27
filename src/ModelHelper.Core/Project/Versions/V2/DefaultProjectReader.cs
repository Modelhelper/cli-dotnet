using Newtonsoft.Json;
using System.IO;

namespace ModelHelper.Project.V2
{
    public class DefaultProjectReader : IProjectReader<Project2>
    {
        public string CurrentVersion { get; } = "2.0.0";
        public Project2 Read(string path)
        {
            if (File.Exists(path))
            {

                var content = System.IO.File.ReadAllText(path);

                if (string.IsNullOrEmpty(content))
                {
                    return null;
                }

                var project = JsonConvert.DeserializeObject<Project2>(content);


                return project;
            }

            return null;
        }

        public ProjectVersion CheckVersion(string path)
        {
            var version = new ProjectVersion();

            
            if (File.Exists(path))
            {
                var content = System.IO.File.ReadAllText(path);

                if (string.IsNullOrEmpty(content))
                {
                    return null;
                }

                version = JsonConvert.DeserializeObject<ProjectVersion>(content);

                version.MustUpdate = version.Major < 2 || version.IsBeta;
                return version;
            }

            return null;
        }
    }
}