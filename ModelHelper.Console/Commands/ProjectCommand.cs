using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using ModelHelper.Core.CommandLine;
using ModelHelper.Core.Extensions;
using ModelHelper.Core.Help;
using ModelHelper.Core.Project;
using ModelHelper.Core.Rules;
using ModelHelper.Extensions;
using ModelHelper.Core.Project.VersionCheckers;

namespace ModelHelper.Commands
{
    [Export(typeof(ICommand))]
    [ExportMetadata("Key", "project")]
    [ExportMetadata("Alias", "p")]
    public class ProjectCommand : BaseCommand
    {
        [ImportMany]
        IEnumerable<Lazy<ICommand>> _commands;

        public ProjectCommand()
        {
            Key = "project";
            Alias = "p";
            Help = HelpFactory.Project(); // GetHelp();
        }


        [Option(Key = "--location-list", Aliases = new[] { "-ll"})]
        public bool ShowLocationList { get; set; }

        [Option(Key = "--option-list", Aliases = new[] { "-ol" })]
        public bool ShowOptionList { get; set; }

        [Option(Key = "--location-list", Aliases = new[] { "-la" })]
        public bool AddLocationItem { get; set; }

        [Option(Key = "--upgrade")]
        public bool Upgrade { get; set; }


        public bool AddOption { get; set; }


        public override bool EvaluateArguments(IRuleEvaluator<Dictionary<string, string>> evaluator)
        {
            return true;
        }

        public override void Execute(List<string> args)
        {
            var argumentMap = this.Parse(args);

            var writer = new ProjectJsonWriter();
            var projectReader = new ProjectJsonReader();
            var projectPath = Path.Combine(Directory.GetCurrentDirectory(), ".model-helper");

            if (File.Exists(projectPath))
            {
                var version = projectReader.CheckVersion(projectPath);
                var runUpgrade = Upgrade && version != null && (version.Version != "1.0.0" || version.IsBeta);

                if (runUpgrade) 
                {
                    ProjectFactory.Upgrade(projectPath, version);
                    return;
                }

                //if (version != null && version.IsBeta)
                //{
                //    Console.WriteLine("The current project file is too old and needs to be upgraded.");
                //    Console.WriteLine("Please run the following command to upgrade (use ctrl + v to paste the command");
                //    var command = "mh project --upgrade";
                //    command.WriteConsoleCommand();
                //    ModelHelperExtensions.ToClipboard(command);
                //    return;   
                //}

                var project = projectReader.Read(projectPath);

                //if (args.Contains("--new"))
                //{
                //    var command = _commands?.FirstOrDefault(c => c.Value.Key == "new");
                //    command?.Value.Execute(args);

                //}

                if (ShowLocationList)
                {
                    project.Code.Locations.ToConsoleTable().WriteToConsole(); ;                                        
                }
                else if (AddLocationItem)
                {
                    AddLocation(project, writer, projectPath);
                }
                else if (ShowOptionList)
                {

                    if (project.Options.Any())
                    {
                        project.Options.Select( o => new { Key = o.Key, Value = o.Value}).ToList().ToConsoleTable().WriteToConsole();
                    }                    
                }
                else
                {

                    Console.WriteLine("");
                    project.WriteToConsole();
                    Console.WriteLine("");


                }
            }
            

            
        }

        private static void AddLocation(IProject project, ProjectJsonWriter writer, string projectPath)
        {
            var location = new ProjectCodeStructure();
            Console.Write("Enter a unique key: ");
            location.Key = Console.ReadLine();

            Console.Write("Enter a namespace: ");
            location.Namespace = Console.ReadLine();

            Console.Write("Enter a path: ");
            location.Path = Console.ReadLine();

            project.Code.Locations.Add(location);
            writer.Write(projectPath, project);
        }

        private HelpItem GetHelp()
        {
            var item = new HelpItem();

            item.Key = "project";
            item.Title = "Help for the 'project' command";
            item.ShortDescription = "Work with project related properties and lists (the .model-helper file)";
            item.LongDescription = item.ShortDescription + "";
            item.Options = new List<Core.Help.HelpOption>
            {
                new Core.Help.HelpOption{Key = "--new <project-name>"},
                //new Core.Help.HelpOption{Key = "--from <project-name>", ShortDescription = "Brukes sammen med --new for å kopiere innhold fra den valgte prosjektfilen"},
                new Core.Help.HelpOption{Key = "--show", ShortDescription = "Shows the content for the project file", Aliases = new List<string>
                {
                    "-s"
                }},
                //new Core.Help.HelpOption{Key = "--recent", ShortDescription = "Viser en liste over de siste prosjektene du har jobbet med"},
                //new HelpOption{Key = "--skip-tests", HelpMessage = "Skips testing the connection to the database, based on the connection string"},
                new Core.Help.HelpOption{Key = "--overwrite", ShortDescription = "skriver over eksisterende .model-helper fil uten å gi advarsel", Aliases = new List<string>
                {
                    "-o"
                }},
                //new Option{Key = "--open", HelpMessage = "Opens the .model-helper file in VSCode"},

            };

            item.Alias = "p";
            item.Samples.Add(new HelpSample
            {
                CommandText = "  mh new <prosjektnavn>\n  mh new",
                Title = "Create a new project at the current location",
                Introduction = @"The followinf commands will create a new .model-helper file at the current location.",
                Description = "You will be asked a few questions about customer name, project name and if you would like to connect to a database",
                Important = "Important message..."
                
            });
            return item;
        }
    }
}