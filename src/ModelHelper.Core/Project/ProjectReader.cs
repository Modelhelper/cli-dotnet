using System.IO;
using Newtonsoft.Json;

namespace ModelHelper.Core.Project
{
    public class ProjectReader : IProjectReader<Project3>
    {
        public ProjectReader()
        {
        }

        public Project3 Read(string path)
        {
            var project = new Project3{ Exists = false};
            
            if (string.IsNullOrEmpty(path))
            {
                return project;
            }

            if (!File.Exists(path))
            {
                return project;
            }

            var content = System.IO.File.ReadAllText(path);

                if (string.IsNullOrEmpty(content))
                {
                    return project;
                }

                project = JsonConvert.DeserializeObject<Project3>(content);
                project.Exists = true;
                
                return project;
            
        }
    }
}