using System.Collections.Generic;
using System.Text;
using ModelHelper.Core.Rules;

namespace ModelHelper.Commands
{
    public class ProjectArgumentEvaluator : IRuleEvaluator<Dictionary<string, string>>
    {
        public ProjectArgumentEvaluator(List<CommandArgument> validArguments)
        {
            Rules = new List<IRule<Dictionary<string, string>>>
            {
                new CommandArgumentsValid(validArguments),
                new IncludesMandatoryCommandArguments(validArguments)
            };
        }

        public List<IRule<Dictionary<string, string>>> Rules { get; set; }

        public EvaluationResult Evaluate(Dictionary<string, string> input)
        {
            var evaluation = new EvaluationResult{Result = EvaluationResultOption.Passes};
            var message = new StringBuilder();
            
            foreach (var rule in Rules)
            {
                var ruleEvaluation = rule.Evaluate(input);

                if (ruleEvaluation.Result == EvaluationResultOption.Failed)
                {
                    evaluation.Result = ruleEvaluation.Result;
                    message.AppendLine(ruleEvaluation.Message);
                }
            }

            return evaluation;
        }
    }
}