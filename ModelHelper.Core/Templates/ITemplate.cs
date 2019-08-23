using System.Collections.Generic;

namespace ModelHelper.Core.Templates
{
    public interface ITemplate
    {
        string Key { get; set; }
        string Name { get; set; }
        string Description { get; set; }
        string Language { get; set; }
        bool CanExport { get; set; }
        string ExportFileName { get; set; }
        string ExportType { get; set; }
       

        bool IsInsertableSnippet { get; set; }
        string InsertIdentifier { get; set; }

        IEnumerable<string> Groups { get; set; }
        IEnumerable<string> Tags { get; set; }

        Dictionary<string, string> Dictionary { get; set; }

        string Body { get; set; }
    }
}