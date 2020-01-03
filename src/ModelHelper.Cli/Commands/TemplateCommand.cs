using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.Rendering;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ModelHelper.Core;

namespace ModelHelper.Cli.Commands
{
    public class TemplateCommand
    {
        private readonly string _description = @"Generates code based on the selected entity and template";
        private readonly ILogger<ProjectCommand> logger;
        private readonly ITerminal _terminal;
        private readonly IModelHelperDefaults defaults;

        public TemplateCommand(ITerminal terminal, IModelHelperDefaults defaults)
        {
            this._terminal = terminal;
            this.defaults = defaults;
        }


        public Command Create()
        {
            // var common = new CommonOptions();
            var command = new Command("template", _description);
            command.AddAlias("t");

            command.Handler = CommandHandler.Create(async () => await HandleCommand());                

            return command;
        }

        internal async Task HandleCommand()
        {
            _terminal.Out.WriteLine("In template");
        }

    }
}