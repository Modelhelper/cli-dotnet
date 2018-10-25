using System.Linq;
using ModelHelper.Core.Database;
using ModelHelper.Extensibility;

namespace ModelHelper.Core.Rules
{
    public class TableImplementsPrimaryKey : ITableRule
    {
        public EvaluationResult Evaluate(IEntity input)
        {
            var result = new EvaluationResult{Result = EvaluationResultOption.Passes};

            
            var hasPrimaryKey= input.Columns.Any(c => c.IsPrimaryKey);
            if (!hasPrimaryKey)
            {
                result.Message = "A table should have at least one primary key";
                result.Result = EvaluationResultOption.Failed;
            }
            return result;
        }
    }
}