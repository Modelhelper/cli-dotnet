using System.Collections.Generic;
using ModelHelper.Core.Templates;

namespace ModelHelper.Core.Models
{
    public class Template : ITemplate
    {
        public Template()
        {
            Dictionary = new Dictionary<string, string>();
        }
        public string Key { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Language { get; set; }
        public bool CanExport { get; set; }
        public string ExportFileName { get; set; }
        public string ExportType { get; set; }
        public bool IsInsertableSnippet { get; set; }
        public string InsertIdentifier { get; set; }
        public IEnumerable<string> Groups { get; set; }
        public IEnumerable<string> Tags { get; set; }
        public Dictionary<string, string> Dictionary { get; set; } 

        public string Body { get; set; }
    }
}