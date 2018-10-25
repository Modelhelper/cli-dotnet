using System;
using System.IO;
using Newtonsoft.Json;
using ModelHelper.Core.Project.VersionCheckers;
using System.Collections.Generic;
using System.Linq;

namespace ModelHelper.Core.Project
{
    public class ProjectJsonReader : IProjectReader
    {
        public string CurrentVersion { get => "1.0.0"; }

        public ProjectVersion CheckVersion(string path)
        {
            var version = new ProjectVersion();

            if (File.Exists(path))
            {
                var content = System.IO.File.ReadAllText(path);

                if (string.IsNullOrEmpty(content))
                {
                    return null;
                }

                version = JsonConvert.DeserializeObject<ProjectVersion>(content);

                return version;
            }

            return null;
        }

        public IProject Read(string path)
        {

            if (File.Exists(path))
            {
                
                var content = System.IO.File.ReadAllText(path);

                if (string.IsNullOrEmpty(content))
                {
                    return null;
                }

                var project = JsonConvert.DeserializeObject<Project>(content);

                //if (project?.Database?.QueryOption != null)
                //{

                //    if (string.IsNullOrEmpty(project.Database.QueryOption.ClassName) &&
                //        !string.IsNullOrEmpty(project.Database.QueryOptionsClassName) )
                //    {
                //        var len = project.Database.QueryOptionsClassName.LastIndexOf(".", StringComparison.Ordinal);

                //        project.Database.QueryOption.ClassName = len > 0 && len > len + 1
                //            ? project.Database.QueryOptionsClassName.Substring(len+1)
                //            : project.Database.QueryOptionsClassName;

                //        project.Database.QueryOption.Namespace = len > 0
                //            ? project.Database.QueryOptionsClassName.Substring(0, len)
                //            : "";

                //        project.Database.QueryOption.UseQueryOptions = project.Database.UseQueryOptions;
                //    }

                //    if (string.IsNullOrEmpty(project.Database.QueryOption.UserIdProperty))
                //    {
                //        project.Database.QueryOption.UserIdProperty = "UserId";
                //    }

                //    if (string.IsNullOrEmpty(project.Database.QueryOption.UserIdType))
                //    {
                //        project.Database.QueryOption.UserIdType = "int";
                //    }

                //    if (string.IsNullOrEmpty(project.Database.QueryOption.ClassName))
                //    {
                //        project.Database.QueryOption.UseQueryOptions = false;
                //    }

                //    if (string.IsNullOrEmpty(project.Database.QueryOption.ClaimsPrincipalExtensionMethod))
                //    {
                //        project.Database.QueryOption.UseClaimsPrincipalExtension = false;
                //    }

                //    project.Database.UseQueryOptions = project.Database.QueryOption.UseQueryOptions;
                //    project.Database.QueryOptionsClassName = $"{project.Database.QueryOption.Namespace}.{project.Database.QueryOption.ClassName}" ;

                //}
                return project;
            }

            return null;
            
        }
    }

    internal class ConvertProjectFromBetaToVersion1 : IProjectConverter
    {
        public IProject Convert(string path)
        {
            //var result = new ConversionResult {
            //    Converted = false               
            //};

            var betaProject = this.LoadBetaProject(path);
            //var projectWriter = new ProjectJsonWriter();
           

            if (betaProject != null)
            {
                var project = new Project();
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

        private ConnectionSection GetCodeConnection(BetaProject betaProject)
        {
            var connection = new ConnectionSection();

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

        private ProjectSourceSection GetSource(BetaDataSection section)
        {
            var source = new ProjectSourceSection();
            source.Connection = section.Connection;

            if (section.ColumnExtras != null && section.ColumnExtras.Any())
            {
                source.ColumnMapping = section.ColumnExtras.Select(c => new ColumnExtra {
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
                        source.ColumnMapping.Add(new ColumnExtra { Name = ic, IsIgnored = true });
                    }
                }
            }


            return source;
        }
        private List<ProjectCodeStructure> GetCodeLocations(BetaProject project)
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
        private BetaProject LoadBetaProject(string path)
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