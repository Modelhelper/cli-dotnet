using System.IO;
using ModelHelper.Project;
using Newtonsoft.Json;

namespace ModelHelper.Core.Project.V1
{
    public class ProjectV1Reader : IProjectReader<ProjectV1>
    {
        public string CurrentVersion { get => "1.0.0"; }

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

                return version;
            }

            return null;
        }

        public ProjectV1 Read(string path)
        {

            if (File.Exists(path))
            {
                
                var content = System.IO.File.ReadAllText(path);

                if (string.IsNullOrEmpty(content))
                {
                    return null;
                }

                var project = JsonConvert.DeserializeObject<ProjectV1>(content);

                
                return project;
            }

            return null;
            
        }
    }
}