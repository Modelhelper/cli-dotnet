using System.Collections.Generic;

namespace ModelHelper.Core.Project.V1
{
    public interface IProjectV1
    {
        string Version { get; }
        string Name { get; set; }
               
        string Description { get; set; }
        string Customer { get; set; }

        string RootPath { get; set; }

        ProjectSourceSectionV1 DataSource { get; set; }

        ProjectCodeSection Code { get; set; }

        List<KeyValuePair<string, string>> Options { get; set; }
        
    }
}