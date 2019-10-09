using System.Collections.Generic;
using System.IO;
using System.Linq;
using ModelHelper.Core.Project.V1;
using Newtonsoft.Json;

namespace ModelHelper.Core.Project.Converters
{
    internal class ConvertProjectFromVersion1 : IProjectConverter<IProjectV1, IProject>
    {
        public IProject Convert(IProjectV1 from)
        {
            var project = new Project();
            project.Data = new ProjectData
            {
                ColumnMapping = ConvertColumnMapping(from)
            };
            var connection = new ProjectDataConnection
            {
                ConnectionString = from.DataSource.Connection,
                DbType = "mssql",
                Name = "mssql"
            };

            
            project.Data.Connections.Add(connection);

            project.Code = from.Code;
            project.RootNamespace = from.Customer;
            project.Customer = from.Customer;
            project.Options = from.Options;
            project.Description = from.Description;
            project.Name = from.Name;
            
            return project;        
        }

        public string GetUsedAs(ColumnExtraV1 column)
        {
            if (column.IsCreatedByUser) return "CreatedBy";
            if (column.IsCreatedDate) return "CreatedOn";
            if (column.IsModifiedByUser) return "ModifiedBy";
            if (column.IsModifiedDate) return "ModifiedOn";
            if (column.IsDeletedMarker) return "DeletedMarker";

            return "";
        }
        public List<ProjectDataColumnMapping> ConvertColumnMapping(IProjectV1 from)
        {
            

            if (from == null || from.DataSource == null || from.DataSource.ColumnMapping == null)
            {
                return new List<ProjectDataColumnMapping>(); ;
            }

            var list = from.DataSource.ColumnMapping.Select(c => new ProjectDataColumnMapping
            {
                Name = c.Name,
                PropertyName = c.PropertyName,
                IncludeInViewModel = c.IncludeInViewModel,
                IsIgnored = c.IsIgnored,
                UsedAs = GetUsedAs(c)
            });

            return list.ToList();
        }
        //private IProject LoadProjectToConvert(string path)
        //{
        //    if (File.Exists(path))
        //    {
        //        var content = System.IO.File.ReadAllText(path);

        //        if (string.IsNullOrEmpty(content))
        //        {
        //            return null;
        //        }

        //        var projectV1 = JsonConvert.DeserializeObject<ProjectV1>(content);

        //        return projectV1;
        //    }

        //    return null;
        //}

    }
}