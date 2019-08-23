using System;
using System.Collections.Generic;
using System.Linq;
using ModelHelper.Core.Database;
using ModelHelper.Core.Project;
using ModelHelper.Core.Project.Converters;
using ModelHelper.Core.Project.V0;
using ModelHelper.Core.Project.V1;
using ModelHelper.Extensibility;

namespace ModelHelper.Core.Extensions
{
    public static class ProjectExtensions
    {
        public static ProjectDataConnection GetConnection(this IProject project, string name)
        {
            if (project != null && project.Data != null)
            {
                var connection = project.Data.Connections.FirstOrDefault(p =>
                    p.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));

                return connection;
            }

            return null;
        }

        public static ProjectDataConnection GetConnection(this IProject project)
        {
            ProjectDataConnection connection = null;

            if (project != null && project.Data != null)
            {
                var name = project.Data.DefaultConnection;

                if (!string.IsNullOrEmpty(name))
                {
                    connection = project.Data.Connections.FirstOrDefault(p =>
                        p.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));

                }
                else
                {
                    connection = project.Data.Connections.FirstOrDefault();
                }

                return connection;
            }

            return null;
        }

        public static List<ProjectDataColumnMapping> ColumnMappings(this IProject project)
        {
            // should return a joined list of columnMappings for the default connection
            if (project == null)
            {
                return new List<ProjectDataColumnMapping>();
            }

            var connection = project.GetConnection();

            return project.ColumnMappings(connection);
        }

        public static List<ProjectDataColumnMapping> ColumnMappings(this IProject project, string connectionName)
        {
            // should return a joined list of columnMappings for the default connection
            if (project == null)
            {
                return new List<ProjectDataColumnMapping>();
            }

            var connection = project.GetConnection(connectionName);

            return project.ColumnMappings(connectionName);
        }

        public static List<ProjectDataColumnMapping> ColumnMappings(this IProject project, ProjectDataConnection connection)
        {
            // should return a joined list of columnMappings for the default connection
            if (project?.Data == null || connection == null)
            {
                return new List<ProjectDataColumnMapping>();
            }

            var list = new List<ProjectDataColumnMapping>(project.Data.ColumnMapping);
            var missing = connection.ColumnMapping.Where(c => list.Select(l => l.Name).Contains(c.Name));
            list.InsertRange(0, missing);

            return list;

        }

        public static IProject ToVersion2(this IProjectV1 project)
        {
            var converter = new ConvertProjectFromVersion1();
            var projectV2 = converter.Convert(project);

            return projectV2;
        }

        public static IProjectV1 ToVersion1(this IBetaProject project)
        {
            var converter = new ConvertProjectFromBetaToVersion1();
            var projectV1 = converter.Convert(project);

            return projectV1;
        }

        
    }
}