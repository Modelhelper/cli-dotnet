using System.Collections.Generic;
using ModelHelper.Core.Rules;

namespace ModelHelper.Commands
{
    public class CommandArgumentsValid : ICommandArgumentRule
    {
        public CommandArgumentsValid(List<CommandArgument> validArguments)
        {
            ValidArguments = validArguments;
        }

        public List<CommandArgument> ValidArguments { get; set; }

        public EvaluationResult Evaluate(Dictionary<string, string> input)
        {
            throw new System.NotImplementedException();
        }
    }
}