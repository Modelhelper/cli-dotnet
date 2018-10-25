using System.Linq;
using ModelHelper.Core.Database;
using ModelHelper.Extensibility;

namespace ModelHelper.Core.Rules
{
    public class TableWithIdentityCannotHaveMultiPrimary : ITableRule
    {
        public EvaluationResult Evaluate(IEntity input)
        {
            var result = new EvaluationResult { Result = EvaluationResultOption.Passes };
            if (input.Columns.Any(c => c.IsIdentity))
            {
                var identityPrimaryCount = input.Columns.Count(c => c.IsIdentity && c.IsPrimaryKey);
                var primaryCount = input.Columns.Count(c => c.IsPrimaryKey);

                if (primaryCount > 1)
                {
                    result.Message =
                        $"The table {input.Name} has one identity column but implements {primaryCount} primary keys";
                    result.Result = EvaluationResultOption.Failed;
                    return result;
                }
            }

            return result;
        }
    }
}