using System.Collections.Generic;

namespace ModelHelper.Core.Rules
{
    public abstract class RuleEvaluatorBase<T> : IRuleEvaluator<T>
    {
        public RuleEvaluatorBase()
        {
            Rules = new List<IRule<T>>();

        }

        public List<IRule<T>> Rules { get;set; }

        public virtual EvaluationResult Evaluate(T input)
        {
            var eval = new EvaluationResult { Result = EvaluationResultOption.Passes };
            
            foreach (var rule in Rules)
            {
                var ruleEval = rule.Evaluate(input);
                eval.Evaluations.Add(ruleEval);

                if (ruleEval.Result == EvaluationResultOption.Failed)
                {
                    eval.Result = ruleEval.Result;
                }
            }
            
            
            return eval;
        }
    }
}