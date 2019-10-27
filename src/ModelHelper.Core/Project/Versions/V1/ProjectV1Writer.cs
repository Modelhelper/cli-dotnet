using ModelHelper.Project;
using Newtonsoft.Json;

namespace ModelHelper.Core.Project.V1
{
    public class ProjectV1Writer : IProjectWriter<IProjectV1>
    {
        public void Write(string path, IProjectV1 project)
        {
            var settings = new JsonSerializerSettings();
            
            var json = JsonConvert.SerializeObject(project);
            System.IO.File.WriteAllText(path, json);
        }
    }
}