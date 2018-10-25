using Newtonsoft.Json;

namespace ModelHelper.Core.Project
{
    public class ProjectJsonWriter : IProjectWriter
    {
        public void Write(string path, IProject project)
        {
            var settings = new JsonSerializerSettings();
            
            var json = JsonConvert.SerializeObject(project);
            System.IO.File.WriteAllText(path, json);
        }
    }
}