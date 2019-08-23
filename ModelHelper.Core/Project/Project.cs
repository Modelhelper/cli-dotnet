using System.IO;
using Newtonsoft.Json;

namespace ModelHelper.Core.Project
{
    public class Project : ProjectBase, IProject
    {
        public Project()
        {
            Version = "2.0.0";
        }

        public string RootNamespace { get; set; }
        public string Customer { get; set; }
        public ProjectData Data { get; set; }
    }

    public class DefaultProjectReader : IProjectReader<IProject>
    {
        public string CurrentVersion { get; } = "2.0.0";
        public IProject Read(string path)
        {
            if (File.Exists(path))
            {

                var content = System.IO.File.ReadAllText(path);

                if (string.IsNullOrEmpty(content))
                {
                    return null;
                }

                var project = JsonConvert.DeserializeObject<Project>(content);


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

    public class DefaultProjectWriter : IProjectWriter<IProject>
    {
        public void Write(string path, IProject project)
        {
            var settings = new JsonSerializerSettings();

            var json = JsonConvert.SerializeObject(project);
            System.IO.File.WriteAllText(path, json);
        }
    }
}