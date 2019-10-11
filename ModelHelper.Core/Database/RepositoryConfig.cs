using System.Collections.Generic;
using ModelHelper.Core.Project;

namespace ModelHelper.Core.Database
{
    public class DatabaseConfig
    {
        public string ConnectionString { get; set; }
        public List<ProjectDataColumnMapping> ColumnMapping { get; set; }
    }
}