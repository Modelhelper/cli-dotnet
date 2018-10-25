using System.Collections.Generic;

namespace ModelHelper.Core.Project
{
    public class Project : IProject
    {
        public Project()
        {
            
        }
            
        
        public string Name { get; set; }
        //public DataSection Database { get; set; }

        public string Description { get; set; }
        public string Customer { get; set; }
        // public List<ProjectCodeStructure> CodeLocations { get; set; }

        public List<KeyValuePair<string, string>> Options { get; set; } = new List<KeyValuePair<string, string>>();
        public string RootPath { get; set; }
        public ProjectSourceSection DataSource { get; set; } = new ProjectSourceSection();
        public ProjectCodeSection Code { get; set; } = new ProjectCodeSection();
        public string Version { get; } = "1.0.0";
        // public Dictionary<string, string> Options { get; set; }


        //public ProjectApiModel Api { get; set; }
        //public string CustomTemplateDirectory { get; set; }
    }



    internal class BetaProject
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

    internal class BetaDataSection
    {
public BetaDataSection()
        {
            QueryOption = new BetaQueryOption();
            ColumnExtras = new List<BetaColumnExtra>();
            IgnoredColumns = new List<string>();
            Translations = new List<KeyValuePair<string, string>>();
            NameMap = new List<KeyValuePair<string, string>>();
        }

        public string DataSourceType { get; set; }

        public string Connection { get; set; }
        public string ConnectionInterface { get; set; }
        public string ConnectionVariable { get; set; }
        public string ConnectionMethod { get; set; }

        
        public bool UseQueryOptions { get; set; }
        
        public string QueryOptionsClassName { get; set; }

        public BetaQueryOption QueryOption { get; set; }

        public List<string> IgnoredColumns { get; set; }

        public List<KeyValuePair<string, string>> Translations { get; set; }
        public List<KeyValuePair<string, string>> NameMap { get; set; }

        public List<BetaColumnExtra> ColumnExtras { get; set; }

    }

    internal class BetaApi
    {
        public bool UseLogger { get; set; }
        public bool UseTelemetry { get; set; }

        
    }

    public class BetaQueryOption
    {
        public bool UseQueryOptions { get; set; }
        public string ClassName { get; set; }
        public string Namespace { get; set; }
        public string UserIdProperty { get; set; }
        public string UserIdType { get; set; }
        public bool UseClaimsPrincipalExtension { get; set; }
        public string ClaimsPrincipalExtensionMethod { get; set; }
        public string ClaimsPrincipalExtensionNamespace { get; set; }



    }
    public class BetaColumnExtra
    {
        public string Name { get; set; }
        public string PropertyName { get; set; }
        public string Translation { get; set; }

        public bool IsIgnored { get; set; }
        public bool IsCreatedByUser { get; set; }
        public bool IsCreatedDate { get; set; }
        public bool IsModifiedByUser { get; set; }
        public bool IsModifiedDate { get; set; }
        public bool IsDeletedMarker { get; set; }

        public bool IncludeInViewModel { get; set; }
    }

}

