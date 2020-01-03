using System.Collections.Generic;

namespace ModelHelper.Core.Project
{
    public interface IProject3
    {
        string Version { get; }
        string RootNamespace { get; set; }
        string Customer { get; set; }
        string Name { get; set; }
        string Description { get; set; }
        string RootPath { get; set; }
        ProjectSource Source { get; set; }
        Dictionary<string, string> Options { get; set; }
        ProjectCodeSection Code { get; set; }
    }
    
    

}