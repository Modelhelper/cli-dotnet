using System.Collections.Generic;

namespace ModelHelper.Core.Rules
{
    public interface IRuleEvaluator<T>
    {
        List<IRule<T>> Rules { get; set; }
        EvaluationResult Evaluate(T input);
    }
}