using System.Collections.Generic;
using ModelHelper.Core.Rules;

namespace ModelHelper.Commands
{
    public class ProjectArgumentsEvaluator : IRuleEvaluator<Dictionary<string, string>>
    {
        public ProjectArgumentsEvaluator()
        {
            Rules = new List<IRule<Dictionary<string, string>>>();
        }

        public List<IRule<Dictionary<string, string>>> Rules { get; set; } // => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        public EvaluationResult Evaluate(Dictionary<string, string> input)
        {
            return new EvaluationResult {Result = EvaluationResultOption.Passes};
        }
    }
}