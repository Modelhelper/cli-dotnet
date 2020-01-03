using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.Rendering;
using Microsoft.Extensions.Logging;

namespace ModelHelper.Console.Commands
{
    public class AboutCommand
    {
        public AboutCommand()
        {
        }




    }

    public class CommandFactory
    {
        private readonly ITerminal _terminal;
        private readonly IServiceProvider _serviceProvider;

        public CommandFactory(ITerminal terminal, IServiceProvider serviceProvider)
        {
            _terminal = terminal;
            this._serviceProvider = serviceProvider;
        }

        public RootCommand CreateRoot()
        {
            var rootCommand = (RootCommand)_serviceProvider.GetService(typeof(ModelHelperRootCommand));
            return rootCommand;
        }
    }
    public class ModelHelperRootCommand
    {
        private readonly ITerminal _terminal;
        private readonly SpanFormatter _spanFormatter = new SpanFormatter();
        public ModelHelperRootCommand(ITerminal terminal)
        {
            this._terminal = terminal;
        }

        public RootCommand Create()
        {
            var rootCommand = new RootCommand();
            rootCommand.Description = "ModelHelper";

            // rootCommand.AddCommand(generateCommand.Create());
            
            rootCommand.Handler = CommandHandler.Create(() =>
            {
                _terminal.Clear();
                // about
                var span = _spanFormatter.ParseToSpan(
                $"{BackgroundColorSpan.Red()}red {BackgroundColorSpan.Blue()}blue {BackgroundColorSpan.Green()}green {BackgroundColorSpan.Reset()}or a {BackgroundColorSpan.Rgb(12, 34, 56)}little of each.");

            var renderer = new ConsoleRenderer(_terminal, OutputMode.Ansi);

            renderer.RenderToRegion(span, new Region(0, 0, 200, 1, false));

                this._terminal.Out.WriteLine("About modelhelper");
            });

            return rootCommand;
        }
    }
    public class GenerateCommand
    {
        private readonly string _description = @"Generates code based on the selected entity and template";
        private readonly ILogger<GenerateCommand> logger;
        private readonly ITerminal terminal;

        public GenerateCommand(ILogger<GenerateCommand> logger, ITerminal terminal)
        {
            this.logger = logger;
            this.terminal = terminal;
        }

        public Command Create()
        {
            var common = new CommonOptions();
            var command = new Command("generate", _description);
            command.AddAlias("g");

            command.AddOption(common.Entities);
            command.AddOption(common.EntityGroups);
            command.AddOption(common.Templates);
            command.AddOption(common.TemplateGroup);
            command.AddOption(common.Env);
            command.AddOption(common.WithConnection);
            command.AddOption(common.ExportByKey);

            command.Handler = CommandHandler.Create<List<string>, List<string>, List<string>, List<string>, Environment, string, bool>(async (entity, entityGroups, template, templateGroup, environment, withConnection, exportByKey) =>
            {
                try
                {
                    this.logger.LogInformation("Start code gen");

                    foreach (var e in entity)
                    {
                        this.terminal.Out.WriteLine(e);
                        // System.Console.WriteLine(e);

                    }

                    foreach (var t in template)
                    {
                        this.terminal.Out.WriteLine(t);

                    }

                    this.terminal.Out.WriteLine(environment.ToString());
                    this.terminal.Out.WriteLine(exportByKey.ToString());


                }
                catch (OperationCanceledException)
                {
                    //console.Error.WriteLine("The operation was aborted");
                    //return 1;
                }
            });

            return command;
        }


    }

    public class GenerateCommandHandler
    {

    }

    public enum Environment
    {
        Test,
        Prod,
        Local
    }
    public class CommonOptions
    {
        public Option Env
        {
            get
            {
                var option = new Option("--env", "Sets the environment that the url should point to. Default value: Prod");
                option.AddAlias("--environment");
                option.Argument = new Argument<Environment>(defaultValue: () => Environment.Prod);

                return option;
            }
        }
        public Option TemplateGroup
        {
            get
            {
                var option = new Option("--template-group", "A list of template groups to use for code generation");
                option.AddAlias("-tg");
                option.AddAlias("-grp");
                option.Argument = new Argument<List<string>>();

                return option;

            }
        }


        public Option Entities
        {
            get
            {
                var option = new Option("--entity", "One or more tables to be used in the given template");
                option.AddAlias("-e");
                option.Argument = new Argument<List<string>>();

                return option;

            }
        }

        public Option ExceptEntities
        {
            get
            {
                var option = new Option("--except", "Excludes the named table(s) for code generation");
                option.AddAlias("--except-table");
                option.Argument = new Argument<List<string>>();

                return option;

            }
        }

        public Option EntityGroups
        {
            get
            {
                var option = new Option("--entity-group", "One or more template groups to use in code generation");
                option.AddAlias("-eg");
                option.Argument = new Argument<List<string>>();

                return option;

            }
        }

        public Option Templates
        {
            get
            {
                var option = new Option("--template", "The group that the list belongs to");
                option.AddAlias("-t");
                option.Argument = new Argument<List<string>>();

                return option;

            }
        }

        public Option ExportByKey
        {
            get
            {
                var option = new Option("--export-bykey", "The group that the list belongs to");
                option.AddAlias("-ek");
                option.Argument = new Argument<bool>(defaultValue: () => false);

                return option;

            }
        }

        public Option OverwriteFiles
        {
            get
            {
                var option = new Option("--overwrite", "The group that the list belongs to");
                option.AddAlias("-o");
                option.Argument = new Argument<bool>(defaultValue: () => false);

                return option;

            }
        }

        public Option ExportToPath
        {
            get
            {
                var option = new Option("--export", "The group that the list belongs to");
                option.Argument = new Argument<string>();

                return option;

            }
        }

        public Option WithConnection
        {
            get
            {
                var option = new Option("--connection", "The group that the list belongs to");
                option.AddAlias("-c");
                option.Argument = new Argument<string>();

                return option;

            }
        }

        public Option IncludeRelations
        {
            get
            {
                var option = new Option("--include-relations", "The group that the list belongs to");
                option.AddAlias("-ir");
                option.AddAlias("-r");
                option.Argument = new Argument<bool>(defaultValue: () => false);

                return option;

            }
        }
        public Option ShowResult
        {
            get
            {
                var option = new Option("--show", "The group that the list belongs to");
                option.AddAlias("-s");
                option.Argument = new Argument<bool>(defaultValue: () => false);

                return option;

            }
        }

        public Option SkipClipboard
        {
            get
            {
                var option = new Option("--skip-clipboard", "The group that the list belongs to");
                option.AddAlias("-sc");
                option.Argument = new Argument<bool>(defaultValue: () => false);

                return option;

            }
        }

        public Option Verbose
        {
            get
            {
                var option = new Option("--verbose", "The group that the list belongs to");
                option.AddAlias("-v");
                option.Argument = new Argument<bool>(defaultValue: () => false);

                return option;

            }
        }



        public Option ViewsOnly
        {
            get
            {
                var option = new Option("--view-only", "The group that the list belongs to");
                option.AddAlias("-vo");
                option.Argument = new Argument<bool>(defaultValue: () => false);

                return option;

            }
        }
        public Option TablesOnly
        {
            get
            {
                var option = new Option("--table-only", "The group that the list belongs to");
                option.AddAlias("-to");
                option.Argument = new Argument<bool>(defaultValue: () => false);

                return option;

            }
        }
    }
}