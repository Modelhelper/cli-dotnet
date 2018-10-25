using System.Linq;
using ModelHelper.Core.Database;
using ModelHelper.Extensibility;

namespace ModelHelper.Core.Rules
{
    public class TableUseSameCollation : IRule<IEntity>
    {
        public EvaluationResult Evaluate(IEntity input)
        {
            var result = new EvaluationResult();

            var collatedColumns = input.Columns.Where(c => !string.IsNullOrEmpty(c.Collation)).GroupBy(c => c.Collation);

            if (collatedColumns.Count() > 1)
            {
                result.Message = $"There are column(s) that uses different collation";
                result.Result = EvaluationResultOption.Failed;
            }

            return result;
        }
    }
}