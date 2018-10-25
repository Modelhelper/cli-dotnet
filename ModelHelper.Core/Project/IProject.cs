using System;
using System.Collections.Generic;

namespace ModelHelper.Core.Project
{
    public interface IProject
    {
        string Version { get; }
        string Name { get; set; }
               
        string Description { get; set; }
        string Customer { get; set; }

        string RootPath { get; set; }

        ProjectSourceSection DataSource { get; set; }

        ProjectCodeSection Code { get; set; }

        List<KeyValuePair<string, string>> Options { get; set; }
        
    }

    public class ProjectCodeSection
    {
        public bool UseQueryOptions { get; set; }

        public QueryOption QueryOptions { get; set; }

        public List<ProjectCodeStructure> Locations { get; set; }

        public ConnectionSection Connection { get; set; }
    }

    public class ConnectionSection
    {
        public string Interface { get; set; }
        public string Variable { get; set; }
        public string Method { get; set; }
    }

    public class ProjectSourceSection
    {
        
        public string Connection { get; set; }
        public string Type { get; set; } = "mssql";
        public string DefaultSchema { get; set; } = "dbo";

        public List<ColumnExtra> ColumnMapping { get; set; } = new List<ColumnExtra>();
    }
    internal interface IBetaProject
    {
        string Name { get; set; }
        
        DataSection Database { get; set; }

        string Description { get; set; }
        string Customer { get; set; }

        List<ProjectCodeStructure> CodeLocations { get; set; }

        List<KeyValuePair<string, string>> Options { get; set; }
        //Dictionary<string, string> Options { get; set; }
        //[Obsolete]
        //ProjectCodeStructure Interfaces { get; set; }
        //[Obsolete]
        //ProjectCodeStructure Models { get; set; }

        //[Obsolete]
        //ProjectCodeStructure Repositories { get; set; }

        //[Obsolete]
        //ProjectCodeStructure Controllers { get; set; }

        ProjectApiModel Api { get; set; }

        [Obsolete]
        string CustomTemplateDirectory { get; set; }
    }
}