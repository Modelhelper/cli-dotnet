using System.Collections.Generic;
using System.Linq;
using System.Text;
using ModelHelper.Core.Database;
using ModelHelper.Extensibility;

namespace ModelHelper.Core.Rules
{
    public class TableEvaluator : IRuleEvaluator<IEntity>
    {
        public TableEvaluator()
        {
            Rules = new List<IRule<IEntity>>
            {
                new CheckForConstraintRule(),
                new TableUseSameCollation(),
                new TableWithIdentityCannotHaveMultiPrimary(),
                new TableImplementsPrimaryKey(),
                // new TableWithIdentityCannotHaveMultiPrimary()
            };
        }
        public List<IRule<IEntity>> Rules { get; set; }
        public EvaluationResult Evaluate(IEntity input)
        {
            var evaluations = new List<EvaluationResult>();
            var result = new EvaluationResult();
            foreach (var rule in Rules)
            {
                var ruleResult = rule.Evaluate(input);
                evaluations.Add(ruleResult);
            }

            result.Evaluations = evaluations;
            
            if (evaluations.Any(r => r.Result == EvaluationResultOption.Failed))
            {
                var messages = new StringBuilder();
                foreach (var item in evaluations.Where(r => r.Result == EvaluationResultOption.Failed))
                {
                    messages.AppendLine(item.Message);
                }
                result.Message = messages.ToString();
                result.Result = EvaluationResultOption.Failed;
            }
            else
            {
                result.Result = EvaluationResultOption.Passes;
            }
            return result;
            
        }
    }
}