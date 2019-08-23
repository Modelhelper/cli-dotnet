using System.Collections.Generic;

namespace ModelHelper.Core.Project.V1
{
    public class ProjectSourceSectionV1
    {
        
        public string Connection { get; set; }
        public string Type { get; set; } = "mssql";
        public string DefaultSchema { get; set; } = "dbo";

        public List<ColumnExtraV1> ColumnMapping { get; set; } = new List<ColumnExtraV1>();
    }
}