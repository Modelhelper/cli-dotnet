using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ModelHelper.Core.Project.V0;
using ModelHelper.Core.Project.V1;
using Newtonsoft.Json;

namespace ModelHelper.Core.Project.Converters
{
    internal class ConvertProjectFromBetaToVersion1 : IProjectConverter<IBetaProject, IProjectV1>
    {
        public IProjectV1 Convert(IBetaProject betaProject)
        {
            //var result = new ConversionResult {
            //    Converted = false               
            //};

            //var betaProject = this.LoadBetaProject(path);
            //var projectWriter = new ProjectJsonWriter();
           

            if (betaProject != null)
            {
                var project = new ProjectV1();
                project.Customer = betaProject.Customer;
                project.Name = betaProject.Name;
                project.Options = betaProject.Options;
                project.Description = betaProject.Description;

                project.Code.Locations = GetCodeLocations(betaProject);

                project.Code.QueryOptions = GetQueryOptions(betaProject.Database);
                project.Code.UseQueryOptions = betaProject.Database.UseQueryOptions;

                project.Code.Connection = GetCodeConnection(betaProject);
                project.DataSource = GetSource(betaProject.Database);


                return project;
                // projectWriter.Write(path, project);

                //result.Converted = true;
                //result.Version = project.Version;
            }



            return null;
        }

        private CodeConnectionSection GetCodeConnection(IBetaProject betaProject)
        {
            var connection = new CodeConnectionSection();

            connection.Interface = betaProject.Database.ConnectionInterface;
            connection.Method = betaProject.Database.ConnectionMethod;
            connection.Variable = betaProject.Database.ConnectionVariable;

            return connection;
        }

        private QueryOption GetQueryOptions(BetaDataSection database)
        {
            var qo = new QueryOption();


            if (database.QueryOption != null)
            {
                qo.ClaimsPrincipalExtensionMethod = database.QueryOption.ClaimsPrincipalExtensionMethod;
                qo.ClaimsPrincipalExtensionNamespace = database.QueryOption.ClaimsPrincipalExtensionNamespace;
                qo.ClassName = database.QueryOption.ClassName;
                qo.Namespace = database.QueryOption.Namespace;
                qo.UseClaimsPrincipalExtension = database.QueryOption.UseClaimsPrincipalExtension;
                qo.UseQueryOptions = database.UseQueryOptions;
                qo.UserIdProperty = database.QueryOption.UserIdProperty;
                qo.UserIdType = database.QueryOption.UserIdType;
            }
            else
            {
                qo.ClassName = !string.IsNullOrEmpty(database.QueryOptionsClassName) ? database.QueryOptionsClassName : "";
                
            }
            
            return qo;
        }

        private ProjectSourceSectionV1 GetSource(BetaDataSection section)
        {
            var source = new ProjectSourceSectionV1();
            source.Connection = section.Connection;

            if (section.ColumnExtras != null && section.ColumnExtras.Any())
            {
                source.ColumnMapping = section.ColumnExtras.Select(c => new ColumnExtraV1 {
                    Name = c.Name,
                    IsCreatedByUser = c.IsCreatedByUser,
                    IsCreatedDate = c.IsCreatedDate,
                    IncludeInViewModel = c.IncludeInViewModel,
                    IsDeletedMarker = c.IsDeletedMarker,
                    IsIgnored = c.IsIgnored,
                    IsModifiedByUser = c.IsModifiedByUser,
                    IsModifiedDate = c.IsModifiedDate,
                    PropertyName = c.PropertyName,
                    Translation = c.Translation
                }).ToList();
            }

            if (section.IgnoredColumns != null && section.IgnoredColumns.Any())
            {
                foreach(var ic in section.IgnoredColumns)
                {
                    var c = source.ColumnMapping.FirstOrDefault(cm => cm.Name.Equals(ic, StringComparison.CurrentCultureIgnoreCase));
                    if (c != null)
                    {
                        c.IsIgnored = true;
                    }
                    else
                    {
                        source.ColumnMapping.Add(new ColumnExtraV1 { Name = ic, IsIgnored = true });
                    }
                }
            }


            return source;
        }
        private List<ProjectCodeStructure> GetCodeLocations(IBetaProject project)
        {
            var list = new List<ProjectCodeStructure>();

            if (project.CodeLocations != null && project.CodeLocations.Any())
            {
                list = project.CodeLocations;
                return list;
            }

            if (project.Interfaces != null)
            {
                list.Add(new ProjectCodeStructure { Key = "api.interfaces", Namespace = project.Interfaces.Namespace, Path = project.Interfaces.Path });
            }

            if (project.Models != null)
            {
                list.Add(new ProjectCodeStructure { Key = "api.models", Namespace = project.Models.Namespace, Path = project.Models.Path });
            }

            if (project.Repositories != null)
            {
                list.Add(new ProjectCodeStructure { Key = "api.repositories", Namespace = project.Repositories.Namespace, Path = project.Repositories.Path });
            }

            if (project.Controllers != null)
            {
                list.Add(new ProjectCodeStructure { Key = "api.controllers", Namespace = project.Controllers.Namespace, Path = project.Controllers.Path });
            }

            return list;
        }
        public BetaProject LoadBetaProject(string path)
        {
            if (File.Exists(path))
            {
                var content = System.IO.File.ReadAllText(path);

                if (string.IsNullOrEmpty(content))
                {
                    return null;
                }

                var betaProject = JsonConvert.DeserializeObject<BetaProject>(content);

                return betaProject;
            }

            return null;
        }
    }
}