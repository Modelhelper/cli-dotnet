using ModelHelper.Core.Project;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using System.Text;

namespace ModelHelper.Extensions
{
    public static class ProjectExtensions
    {
                public static Project3 OpenProject(this DirectoryInfo projectDirectory)
        {
            if (projectDirectory.Exists)
            {
                var files = projectDirectory.GetFiles("Project.json", SearchOption.TopDirectoryOnly);

                var projectFile = files.FirstOrDefault();

                if (projectFile != null)
                {
                    var content = File.ReadAllText(projectFile.FullName)
;                }
            }

            return null;
        }

        public static Project3 LoadContent(string content)
        {
            try
            {
                var project = JsonConvert.DeserializeObject<Project3>(content);
                return project;
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        public static void Save(this Project3 project, DirectoryInfo projectDirectory)
        {
            if (!projectDirectory.Exists)
            {
                Directory.CreateDirectory(projectDirectory.FullName);
            }

            var json = JsonConvert.SerializeObject(project, Formatting.Indented);
            var fileName = "Project.json";
            var filePath = Path.Combine(projectDirectory.FullName, fileName);

            File.WriteAllText(filePath, json, Encoding.UTF8);
        }
    }
}