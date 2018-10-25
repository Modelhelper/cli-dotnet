using System.Collections.Generic;
using ModelHelper.Core.Help;
using ModelHelper.Core.Rules;

namespace ModelHelper.Commands
{
    public abstract class BaseCommand : ICommand
    {
        public BaseCommand()
        {
            ValidArguments = new List<CommandArgument>();
            IsPublic = true;
            Help = new HelpItem();
        }

        public string Key { get; set; }
        public string Alias { get; set; }
        public bool IsPublic { get; set; }

        public HelpItem Help { get; set; }

        public List<CommandArgument> ValidArguments { get; set; }

        public abstract bool EvaluateArguments(IRuleEvaluator<Dictionary<string, string>> evaluator);
        public abstract void Execute(List<string> arguments);
    }
}