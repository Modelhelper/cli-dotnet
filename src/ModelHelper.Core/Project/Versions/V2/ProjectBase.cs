using System.Collections.Generic;

namespace ModelHelper.Project.V2
{
    public abstract class ProjectBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
        

        public string RootPath { get; set; }
        public List<KeyValuePair<string, string>> Options { get; set; } = new List<KeyValuePair<string, string>>();
        public ProjectCodeSection Code { get; set; } = new ProjectCodeSection();
        public string Version { get; protected set; }
    }
}