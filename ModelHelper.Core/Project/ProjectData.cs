using System.Collections.Generic;

namespace ModelHelper.Core.Project
{
    public class ProjectData
    {
        public string DefaultConnection { get; set;}
        public List<ProjectDataConnection> Connections { get;set;} = new List<ProjectDataConnection>();

        public List<ProjectDataColumnMapping> ColumnMapping { get; set; } = new List<ProjectDataColumnMapping>();
    }
}