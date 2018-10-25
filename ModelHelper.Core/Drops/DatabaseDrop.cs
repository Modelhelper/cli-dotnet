using ModelHelper.Core.Project;
using DotLiquid;

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
        public DatabaseDrop(IProject project)
        {
            ConnectionInterface = project.Code.Connection.Interface;
            ConnectionMethod = project.Code.Connection.Method;
            ConnectionVariable = project.Code.Connection.Variable;
            QueryOptionsClassName =project.Code.QueryOptions.ClassName;
            UseQueryOptions = project.Code.UseQueryOptions;

            DataSource = string.IsNullOrEmpty(project.DataSource.Type)
                ? "mssql"
                : project.DataSource.Type;

            QueryOption = new QueryOptionDrop(project.Code.QueryOptions);
        }

        public string DataSource { get;  }
    }
}