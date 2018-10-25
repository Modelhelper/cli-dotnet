using System.Linq;
using ModelHelper.Core.Database;
using ModelHelper.Extensibility;

namespace ModelHelper.Core.Rules
{
    public class IdentityIsAlsoPrimary : ITableRule
    {
        public EvaluationResult Evaluate(IEntity input)
        {
            var result = new EvaluationResult{Result = EvaluationResultOption.Passes};
            if (input.Columns.Any(c => c.IsIdentity))
            {
                var count = input.Columns.Count(c => c.IsIdentity && c.IsPrimaryKey);
                if (count == 0)
                {
                    result.Message =
                        $"The table {input.Name} has an Identity column but with a different primary key. ";
                    result.Result = EvaluationResultOption.Failed;

                    return result;
                }
                
            }

            return result;
            
        }
    }
}