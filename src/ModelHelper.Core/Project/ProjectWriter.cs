using Newtonsoft.Json;

namespace ModelHelper.Core.Project
{
    public class ProjectWriter : IProjectWriter<IProject3>
    {
        public void Write(string path, IProject3 project)
        {
           var settings = new JsonSerializerSettings();
            
            var json = JsonConvert.SerializeObject(project);
            System.IO.File.WriteAllText(path, json);
        }
    }
}