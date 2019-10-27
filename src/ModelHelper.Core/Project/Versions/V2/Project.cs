using Newtonsoft.Json;

namespace ModelHelper.Project.V2
{
    public class Project2 : ProjectBase //, IProject
    {
        public Project2()
        {
            Version = "2.0.0";
        }

        public string RootNamespace { get; set; }
        public string Customer { get; set; }
        public ProjectData Data { get; set; }
    }
}