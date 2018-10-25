using System.Collections.Generic;
using System.Linq;

namespace ModelHelper.Core.Rules
{
    public class EvaluationResult
    {
        public EvaluationResult()
        {
            Result = EvaluationResultOption.Passes;
            Evaluations = new List<EvaluationResult>();
        }
        public string Message { get; set; }

        public bool Failed
        {
            get
            {
                return Evaluations != null && Evaluations.Any(e => e.Result == EvaluationResultOption.Failed);
            }
        }
        public bool HasWarnings
        {
            get
            {
                return Evaluations != null && Evaluations.Any(e => e.Result == EvaluationResultOption.Warning);
            }
        }
        public List<EvaluationResult> Evaluations { get; set; }
        
        
        public EvaluationResultOption Result { get; set; }

    }
}