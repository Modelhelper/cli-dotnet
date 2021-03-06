using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data.SqlClient;
using System.IO;
using ModelHelper.Core.Extensions;
using ModelHelper.Core.Project;
using ModelHelper.Core.Project.V1;
using ModelHelper.Core.Rules;

namespace ModelHelper.Commands
{

    [Export(typeof(ICommand))]
    [ExportMetadata("Key", "new")]    
    public class NewCommand : BaseCommand
    {
        public NewCommand()
        {
            Key = "new";
        }

        public override bool EvaluateArguments(IRuleEvaluator<Dictionary<string, string>> evaluator)
        {
            return true;
        }

        public override void Execute(Core.ApplicationContext context)
        {
            var s = context.Options;
            var skipTests = s.Contains("--skip-tests");
            var projectName = s.Count > 0 && !s[0].StartsWith("-") ? s[0] : "";
            var openForEdit = s.Contains("--open");

            var projectExists = Application.ProjectFileExists();
            var createProjectFile = true;

            if (projectExists)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                //Console.Write("Det eksisterer en prosjektfil allerde, vil du overskrive denne (y/N)? ");
                Console.Write("A project file already exists, do you want to overwrite this (y/N)? ");
                Console.ResetColor();
                var result = Console.ReadLine();

                createProjectFile = !string.IsNullOrEmpty(result) && result.ToLowerInvariant() == "y";

            }

            if (createProjectFile)
            {


                var project = new Project();
                var projectWriter = new DefaultProjectWriter();


                Console.WriteLine("\nProject setup: \n");

                if (string.IsNullOrEmpty(projectName))
                {
                    Console.Write("Project Name: ");
                    projectName = Console.ReadLine();
                }

                project.Name = projectName;

                Console.Write("Customer Name: ");
                project.Customer = Console.ReadLine();

                Console.Write("Root Namespace: ");
                project.RootNamespace = Console.ReadLine();

                Console.Write("Add default connection now? (Y/n): ");
                var createConnection = Console.ReadLine();
                if (string.IsNullOrEmpty(createConnection) ||
                    (!string.IsNullOrEmpty(createConnection) &&
                     createConnection.Equals("y", StringComparison.InvariantCultureIgnoreCase)))
                {
                    var connectionstring = new SqlConnectionStringBuilder();

                    Console.Write("Connection Name: ");
                    var connectionName = Console.ReadLine();

                    Console.Write("MS Sql Server name: ");
                    var sqlServer = Console.ReadLine();
                    if (!string.IsNullOrEmpty(sqlServer))
                    {
                        connectionstring.DataSource = sqlServer;

                        Console.Write("Database Name: ");
                        connectionstring.InitialCatalog = Console.ReadLine();

                        Console.Write("Use integrated security (Y/n): ");
                        var integratedAnswer = Console.ReadLine();
                        var useIntegrated = string.IsNullOrEmpty(integratedAnswer) ||
                                            (!string.IsNullOrEmpty(integratedAnswer) && integratedAnswer.Equals("y", StringComparison.InvariantCultureIgnoreCase));

                        if (useIntegrated)
                        {
                            connectionstring.IntegratedSecurity = true;
                        }
                        else
                        {
                            Console.Write("Username: ");
                            connectionstring.UserID = Console.ReadLine();

                            Console.Write("Password: ");
                            var pass = "";
                            ConsoleKeyInfo key;
                            do
                            {
                                key = Console.ReadKey(true);
                                if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                                {
                                    pass += key.KeyChar;
                                    Console.Write("*");
                                }
                                else
                                {
                                    if (key.Key == ConsoleKey.Backspace && connectionstring.Password.Length > 0)
                                    {
                                        pass = pass.Substring(0, (pass.Length - 1));
                                        Console.Write("\b \b");
                                    }
                                }
                            } while (key.Key != ConsoleKey.Enter);
                            connectionstring.Password = pass;

                        }

                        connectionstring.ConnectTimeout = 10;
                        connectionstring.ApplicationName = "ModelHelper";

                        project.Data.Connections.Add(new ProjectDataConnection
                        {
                            Name = connectionName,
                            ConnectionString = connectionstring.ConnectionString,
                            DefaultSchema = "dbo",
                            DbType = "mssql"
                        }); 


                    }
                }
                project.Code.Connection = new CodeConnectionSection {
                    Method = "",
                    Interface = "",
                    Variable = ""
                };

                project.Code.QueryOptions = new QueryOption
                {
                    UseQueryOptions = false
                };
                //project.Api = new ProjectApiModel
                //{
                //    UseLogger = false,
                //    UseTelemetry = false,
                //    UseUserRepository = false
                //};

                project.Code.Locations = new List<ProjectCodeStructure>
                    {
                        new ProjectCodeStructure{Key = "api.interfaces", Namespace = $"{project.Customer }.Infrastructure.Repositories" , Path = ".\\interfaces"},
                        new ProjectCodeStructure{Key = "api.models", Namespace = $"{project.Customer }.Infrastructure.Model" , Path = ".\\models"},
                        new ProjectCodeStructure{Key = "api.controllers", Namespace = $"{project.Customer }.Api.Controllers" , Path = ".\\controllers"},
                        new ProjectCodeStructure{Key = "api.repositories", Namespace = $"{project.Customer }.Data.Repositories" , Path = ".\\repositories"},
                    };


               // project.CustomTemplateDirectory = ".\\templates";

                var projectPath = context.ProjectFile.FullName; // Path.Combine(Directory.GetCurrentDirectory(), ".model-helper", "project.json");
                projectWriter.Write(projectPath, project);
            }
        }
    }
}