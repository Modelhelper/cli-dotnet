using ModelHelper.Core.Extensions;
using ModelHelper.Core.Project;
using ModelHelper.Data;
using ModelHelper.Extensibility;

namespace ModelHelper.Extensions
{
    public static class ProjectExtensions
    {
        public static IDatabase CreateRepository(this IProject project, string connectionName = "")
        {
            var connection = string.IsNullOrEmpty(connectionName)
                ? project.GetConnection()
                : project.GetConnection(connectionName);

            if (connection == null)
            {
                return null;
            }
            var config = new ModelHelper.Core.Database.DatabaseConfig();
            config.ConnectionString = connection.ConnectionString;
            config.ColumnMapping = project.ColumnMappings(connection);//  connection.ColumnMapping;

            switch (connection.DbType.ToLowerInvariant())
            {
                case "mssql":
                    return new SqlServerDatabase(config);
            }

            
            return null;
        }
    }
}