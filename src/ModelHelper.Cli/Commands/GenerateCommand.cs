using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.Rendering;

using Microsoft.Extensions.Logging;

namespace ModelHelper.Cli.Commands
{
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
            // ISuggestionRegistration suggestionProvider = GetSuggestionRegistration();
            


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
                        System.Console.WriteLine(e);

                    }

                    foreach (var t in template)
                    {
                        System.Console.WriteLine(t);

                    }

                    System.Console.WriteLine(environment.ToString());
                    System.Console.WriteLine(exportByKey.ToString());


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
}