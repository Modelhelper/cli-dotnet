namespace ModelHelper.Core.Rules
{
    public interface IRule<in T>
    {
        EvaluationResult Evaluate(T input);
    }

    //public class DatabaseEvaluator : IRuleEvaluator<IEnumerable<ITable>>, TableEvaluateResult>
    //{
    //    public List<IRule<ITable>> Rules { get; set; }
    //    public TableEvaluateResult Evaluate(IEnumerable<ITable> input)
    //    {
    //        foreach (var rule in Rules)
    //        {
    //            var result = rule.Evaluate(input);
    //        }
    //        throw new System.NotImplementedException();
    //    }
    //}
}