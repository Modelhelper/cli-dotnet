using ModelHelper.Project.V2;
using System.Collections.Generic;

namespace ModelHelper.Project
{
    public interface IProject
    {
        string Version { get; }
        string Name { get; set; }

        string RootNamespace { get;set;}
        string Description { get; set; }
        string Customer { get; set; }

        ProjectData Data { get;set;}
        string RootPath { get; set; }

        ProjectCodeSection Code { get; set; }

        List<KeyValuePair<string, string>> Options { get; set; }
    }

    
}