using ModelHelper.Core.Project;
using DotLiquid;
using ModelHelper.Core.Project.V1;

namespace ModelHelper.Core.Drops
{
    public class DatabaseDrop : Drop
    {
        public string ConnectionInterface { get; }
        public string ConnectionMethod { get; }
        public string ConnectionVariable { get; }
        public string QueryOptionsClassName { get; }
        public bool UseQueryOptions { get; }

        public QueryOptionDrop QueryOption { get; }
        public DatabaseDrop(IProject project, string dbType = "mssql")
        {
            ConnectionInterface = project?.Code?.Connection != null ? project.Code.Connection.Interface : string.Empty;
            ConnectionMethod = project?.Code?.Connection != null ? project.Code.Connection.Method : string.Empty;
            ConnectionVariable = project?.Code?.Connection != null ? project.Code.Connection.Variable : string.Empty;
            QueryOptionsClassName =project?.Code?.QueryOptions != null ? project.Code.QueryOptions.ClassName : string.Empty;
            UseQueryOptions = project?.Code?.UseQueryOptions ?? false;

            DataSource = string.IsNullOrEmpty(dbType)
                ? "mssql"
                : dbType;

            QueryOption = new QueryOptionDrop(project.Code.QueryOptions);
        }

        public string DataSource { get;  }
    }
}