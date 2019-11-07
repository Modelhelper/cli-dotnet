using ModelHelper.Core.Project;
using Newtonsoft.Json;

namespace ModelHelper.Project.V2
{
    public class DefaultProjectWriter : IProjectWriter<Project2>
    {
        public void Write(string path, Project2 project)
        {
            var settings = new JsonSerializerSettings();

            var json = JsonConvert.SerializeObject(project);
            System.IO.File.WriteAllText(path, json);
        }
    }
}