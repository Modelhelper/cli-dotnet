using System.Collections.Generic;
using ModelHelper.Core.Project.V1;
using ModelHelper.Project.V2;

namespace ModelHelper.Core.Project.V0
{
    public interface IBetaProject
    {
        string Name { get; set; }
        BetaDataSection Database { get; set; }
        string Description { get; set; }
        string Customer { get; set; }
        List<ProjectCodeStructure> CodeLocations { get; set; }
        List<KeyValuePair<string, string>> Options { get; set; }
        BetaApi Api { get; set; }
        string CustomTemplateDirectory { get; set; }
        ProjectCodeStructure Interfaces { get; set; }
        ProjectCodeStructure Models { get; set; }
        ProjectCodeStructure Repositories { get; set; }
        ProjectCodeStructure Controllers { get; set; }
    }

    public class BetaProject : IBetaProject
    {
        public BetaProject()
        {
            Database = new BetaDataSection()
            {
                //IgnoredColumns = new List<string>()
            };

            CodeLocations = new List<ProjectCodeStructure>();
            Options = new List<KeyValuePair<string, string>>();
            //Options = new Dictionary<string, string>();
        }


        public string Name { get; set; }
        public BetaDataSection Database { get; set; }

        public string Description { get; set; }
        public string Customer { get; set; }
        public List<ProjectCodeStructure> CodeLocations { get; set; }

        public List<KeyValuePair<string, string>> Options { get; set; }
        // public Dictionary<string, string> Options { get; set; }


        public BetaApi Api { get; set; }
        public string CustomTemplateDirectory { get; set; }

        public ProjectCodeStructure Interfaces { get; set; }
        public ProjectCodeStructure Models { get; set; }
        public ProjectCodeStructure Repositories { get; set; }
        public ProjectCodeStructure Controllers { get; set; }
        
    }
}