using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace ModelHelper.Core.Project
{
    public class Project3
    {
        public Project3()
        {
            Version = "3.0.0";
        }
        public string Version { get; private set; }
        public string RootNamespace { get; set; }
        public string Customer { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string RootPath { get; set; }
        public ProjectSource Source { get; set; }
        public Dictionary<string, string> Options { get; set; } = new Dictionary<string, string>();
        public ProjectCodeSection Code { get; set; } = new ProjectCodeSection();

        public class ProjectCodeSection
        {
            public bool UseLogging { get; set; } = false;
            public bool RemoveColumnPrefixes { get; set; } = true;
            public bool UseConnectionFactory { get; set; } = true;
            public bool UseQueryOptions { get; set; } = false;
            public bool InjectUserContext { get; set; } = false;            
            // public string Key { get; set; }

            public List<CodeLocation> Locations { get; set; }

            public CodeConnectionFactory ConnectionFactory { get; set; }
            public CodeUserContext UserContext { get; set; }
            public CodeQueryOption QueryOptions { get; set; }

            public class CodeLocation
            {
                public string Key { get; set; }
                public string Namespace { get; set; }
                public string Path { get; set; }

                public string NamePrefix { get; set; }
                public string NamePostfix { get; set; }
            }

            public class CodeConnectionFactory
            {
                public string Interface { get; set; }
                public string Variable { get; set; }
                public string Method { get; set; }                
                public string CodeLocationKey { get; set; }
            }

            public class CodeUserContext
            {
                public string InterfaceName { get; set; }
                public string VariableName { get; set; }
                public string UserProperty { get; set; }
                public string UserType { get; set; }
                public string CodeLocationKey { get; set; }
            }

            public class CodeQueryOption
            {
                public string ClassName { get; set; }
                public string UserProperty { get; set; }
                public string UserType { get; set; }
                public string CreateMethod { get; set; }
                public string CodeLocationKey { get; set; }
            }
        }

        public class ProjectSource
        {
            public string DefaultSource { get; set; } = "main";
            public List<ProjectSourceConnection> Connections { get; set; } = new List<ProjectSourceConnection>();

            public List<ProjectEntityGroup> Groups { get; set; } = new List<ProjectEntityGroup>();
            public List<ProjectSourceColumnMapping> Mapping { get; set; } = new List<ProjectSourceColumnMapping>();

            public class ProjectEntityGroup
            {
                public string Name { get; set; }
                public string Schema { get; set; }
                public Dictionary<string, string> Options { get; set; } = new Dictionary<string, string>();

                public List<string> Entities { get; set; }

            }

            public class ProjectSourceConnection
            {
                public string Name { get; set; } = "main";
                public string ConnectionString { get; set; }
                public string DefaultSchema { get; set; } = "dbo";
                public string Type { get; set; } = "mssql";

                public string CreateConnectionMethod { get; set; }
                public List<ProjectSourceColumnMapping> Mapping { get; set; } = new List<ProjectSourceColumnMapping>();
                public List<ProjectEntityGroup> Groups { get; set; } = new List<ProjectEntityGroup>();

                public Dictionary<string, string> Options { get; set; } = new Dictionary<string, string>();


                
            }

            public class ProjectSourceColumnMapping
            {
                public string Name { get; set; }
                public bool IsIgnored { get; set; }
                public string UsedAs { get; set; }
            }
        }
    }


    
}