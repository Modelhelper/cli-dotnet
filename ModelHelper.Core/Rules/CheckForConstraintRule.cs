using System.Linq;
using ModelHelper.Core.Database;
using ModelHelper.Extensibility;

namespace ModelHelper.Core.Rules
{
    public class CheckForConstraintRule : ITableRule
    {
        public EvaluationResult Evaluate(IEntity input)
        {
            var result = new EvaluationResult();

            var columnsEndWithId = input.Columns.Count(c => c.Name.ToLowerInvariant().EndsWith("id") && c.Name.Length > 2);

            if (columnsEndWithId > 0 && !input.ParentRelations.Any())
            {
                result.Message = $"There are {columnsEndWithId} column(s) that ends with Id, but no FK constraints";
                result.Result = EvaluationResultOption.Warning;
            }

            return result;
        }
    }
}