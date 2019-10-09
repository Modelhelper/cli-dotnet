using System.Collections.Generic;

namespace ModelHelper.Core.Project
{
    public class ProjectDataConnection
    {
        public string Name { get; set; }
        public string ConnectionString { get; set; }
        public string DefaultSchema { get; set; }
        public string DbType { get; set; }
        public string Interface { get; set; }
        public string Variable { get; set; }
        public string Method { get; set; }

        public QueryOption QueryOptions { get; set; }

        public List<ProjectDataColumnMapping> ColumnMapping { get;set;} = new List<ProjectDataColumnMapping>();
    }
}