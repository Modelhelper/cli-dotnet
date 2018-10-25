using System.Collections.Generic;
using ModelHelper.Core.Rules;

namespace ModelHelper.Commands
{
    public class IncludesMandatoryCommandArguments : ICommandArgumentRule
    {
        public IncludesMandatoryCommandArguments(List<CommandArgument> validArguments)
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