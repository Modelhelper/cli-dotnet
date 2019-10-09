using System.Collections.Generic;
using ModelHelper.Core.Database;
using ModelHelper.Core.Project;
using ModelHelper.Core.Project.V1;
using ModelHelper.Extensibility;

namespace ModelHelper.Core.Templates
{
    public interface ITemplateModel
    {
        IEntity Table { get; set; }
        IProject Project { get; set; }

        bool IncludeChildren { get; set; }
        bool IncludeParents { get; set; }

        bool IncludeRelations { get; set; }
        Dictionary<string, object> Options { get; set; }
        Dictionary<string, string> Dictionary { get; set; }

        List<ISqlGenerator> SqlScriptGenerators { get; set; }
        List<IDatatypeConverter> DatatypeConverters { get; set; }
    }
}