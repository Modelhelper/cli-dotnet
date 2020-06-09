using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.Rendering;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ModelHelper.Core;
using ModelHelper.Core.Project;

namespace ModelHelper.Cli.Commands
{
    public class ProjectCommand
    {
        private readonly string _description = @"Generates code based on the selected entity and template";
        private readonly ILogger<ProjectCommand> logger;
        private readonly ITerminal _terminal;
        private readonly IProject3 currentProject;
        private readonly IModelHelperDefaults defaults;

        public ProjectCommand(ITerminal terminal, IProject3 currentProject, IModelHelperDefaults defaults)
        {
            this._terminal = terminal;
            this.currentProject = currentProject;
            this.defaults = defaults;
        }


        public Command Create()
        {
            var common = new CommonOptions();
            var command = new Command("project", _description);
            command.AddAlias("p");
            
            command.Handler = CommandHandler.Create(async () => await HandleCommand());

            var newCommand = new NewProjectCommand(_terminal, currentProject, defaults);
            // sub commands
            command.AddCommand(newCommand.CreateNewProjectCommand());
            command.AddCommand(this.CreateAddCommand());
            command.AddCommand(this.CreateUpgradeProjectCommand());

            return command;
        }

        internal async Task HandleCommand()
        {
            System.Console.WriteLine("In main project");
        }




        internal Command CreateUpgradeProjectCommand()
        {
            //var common = new CommonOptions();
            var command = new Command("upgrade", "Upgrades the current project to the newest project schema");
            command.AddAlias("u");

            command.Handler = CommandHandler.Create(async () => await HandleUpgradeCommand());

            return command;
        }

        internal async Task HandleUpgradeCommand()
        {
            System.Console.WriteLine($"Upgrade project");
        }


        internal Command CreateAddCommand()
        {
            //var common = new CommonOptions();
            var command = new Command("add", "Add segment items to project nodes");
            command.AddAlias("a");

            command.AddCommand(CreateAddConnectionCommand());
            command.AddCommand(CreateAddLocationCommand());

            command.Handler = CommandHandler.Create(async () => await HandleAddCommand());

            return command;
        }

        internal async Task HandleAddCommand()
        {
            System.Console.WriteLine($"Add subcommand");
        }

        internal Command CreateAddConnectionCommand()
        {
            //var common = new CommonOptions();
            var command = new Command("connection", "Upgrades the current project to the newest project schema");
            command.AddAlias("c");

            var argument = new Argument<string>("connectionName");

            command.AddArgument(argument);

            command.Handler = CommandHandler.Create(async () => await HandleAddConnectionCommand());

            return command;
        }

        internal async Task HandleAddConnectionCommand()
        {
            System.Console.WriteLine($"Add connection");
        }

        internal Command CreateAddLocationCommand()
        {
            //var common = new CommonOptions();
            var command = new Command("location", "Upgrades the current project to the newest project schema");
            command.AddAlias("l");


            var argument = new Argument<string>("locationKey");
            command.AddArgument(argument);

            command.Handler = CommandHandler.Create(async () => await HandleAddLocationCommand());

            return command;
        }

        internal async Task HandleAddLocationCommand()
        {
            System.Console.WriteLine($"Add location");
        }
    }

    public class NewProjectCommand
    {
        private readonly ITerminal _terminal;
        private IProject3 _currentProject;
        private readonly IModelHelperDefaults _defaults;

        public NewProjectCommand(ITerminal terminal, IProject3 currentProject, IModelHelperDefaults defaults)
        {
            this._terminal = terminal;
            this._currentProject = currentProject;
            this._defaults = defaults;
        }


        public Command CreateNewProjectCommand()
        {
            //var common = new CommonOptions();
            var command = new Command("new", "Creates a new project on the current location");
            command.AddAlias("init");
            command.AddAlias("n");
            command.AddAlias("i");

            command.AddOption(this.ProjectType);
            command.AddOption(this.ScanProject);
            command.AddOption(this.Verbose);

            var projectName = new Argument<string>("name");

            command.AddArgument(projectName);

            command.Handler = CommandHandler.Create<string, ProjectCreateType, bool, bool>(async (name, type, verbose, scan) => await HandleNewCommand(name, type, verbose, scan));

            return command;
        }

        internal async Task HandleNewCommand(string name, ProjectCreateType type, bool verbose, bool scan)
        {
            if (_defaults.CurrentProjectDirectory.Exists || _defaults.CurrentProjectFile.Exists)
            {
                System.Console.WriteLine("\nA project file already exists, do you want to overwrite this (y/N)? ");
            }
            else
            {
                _defaults.CurrentProjectDirectory.Create();
                var writer = new ProjectWriter();
                _currentProject.Exists = true;

                _currentProject = ProjectFactory.Create(type);
                
                writer.Write(_defaults.CurrentProjectFile.FullName, _currentProject);
                System.Console.WriteLine($"New project created at '{_defaults.CurrentProjectDirectory.FullName}'");
            }
        }

        public Option Verbose
        {
            get
            {
                var option = new Option("--verbose", "Output all log statement during initialization");
                option.AddAlias("-v");
                option.Argument = new Argument<bool>(getDefaultValue: () => false);

                return option;

            }
        }
        public Option ProjectType
        {
            get
            {
                var option = new Option("--type", "Sets the content of the new project based on type");
                option.AddAlias("-t");
                option.Argument = new Argument<ProjectCreateType>(getDefaultValue: () => ProjectCreateType.Default);

                return option;

            }
        }

        public Option ScanProject
        {
            get
            {
                var option = new Option("--scan", "Scan sub folders for solutions and projects");
                option.AddAlias("-s");
                option.Argument = new Argument<bool>(getDefaultValue: () => false);

                return option;

            }
        }




    }
}