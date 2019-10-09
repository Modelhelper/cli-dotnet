using System.Collections.Generic;

namespace ModelHelper.Core.Templates
{
    public class JsonTemplate : IJsonTemplate
    {
        public string Key { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Language { get; set; }
        public bool? CanExport { get; set; }
        public string ExportFileName { get; set; }
        public string ExportType { get; set; }
        public TemplateType TemplateType { get; set; }
        public IEnumerable<string> Groups { get; set; }
        public IEnumerable<string> Tags { get; set; }
        public IEnumerable<string> Body { get; set; }

        public IEnumerable<KeyValuePair<string, string>> Dictionary {get;set;}

        
    }
}