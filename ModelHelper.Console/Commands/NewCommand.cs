using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data.SqlClient;
using System.IO;
using ModelHelper.Core.Extensions;
using ModelHelper.Core.Project;
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

        public override void Execute(List<string> arguments)
        {
            var s = arguments;
            var skipTests = s.Contains("--skip-tests");
            var projectName = s.Count > 0 && !s[0].StartsWith("-") ? s[0] : "";
            var openForEdit = s.Contains("--open");

            var projectExists = ModelHelperExtensions.ProjectFileExists();
            var createProjectFile = true;

            if (projectExists)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Det eksisterer en prosjektfil allerde, vil du overskrive denne (y/N)? ");
                Console.ResetColor();
                var result = Console.ReadLine();

                createProjectFile = !string.IsNullOrEmpty(result) && result.ToLowerInvariant() == "y";

            }

            if (createProjectFile)
            {


                var project = new ModelHelper.Core.Project.Project();
                var projectWriter = new ModelHelper.Core.Project.ProjectJsonWriter();


                Console.WriteLine("\nDu blir nå presentert noen få spørsmål\n");

                if (string.IsNullOrEmpty(projectName))
                {
                    Console.Write("Prosektnavn: ");
                    projectName = Console.ReadLine();
                }

                project.Name = projectName;

                Console.Write("Kundenavn: ");
                project.Customer = Console.ReadLine();

                Console.Write("Legg inn 'ConnectionString' nå? (Y/n): ");
                var createConnection = Console.ReadLine();
                if (string.IsNullOrEmpty(createConnection) ||
                    (!string.IsNullOrEmpty(createConnection) &&
                     createConnection.Equals("y", StringComparison.InvariantCultureIgnoreCase)))
                {
                    var connectionstring = new SqlConnectionStringBuilder();

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

                        project.DataSource.Connection = connectionstring.ConnectionString;


                    }
                }
                project.Code.Connection = new ConnectionSection {
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

                var projectPath = Path.Combine(Directory.GetCurrentDirectory(), ".model-helper");
                projectWriter.Write(projectPath, project);
            }
        }
    }
}