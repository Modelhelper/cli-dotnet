using System.Collections.Generic;

namespace ModelHelper.Templates
{
    public class Template3
    {
        public string Key { get; set; }
        public string Version { get; set; }
        public string Language { get; set; }
        public string Description { get; set; }
        public string ShortDescription { get; set; }
        public IEnumerable<string> Tags { get; set; }
        public IEnumerable<string> Groups { get; set; }
        public IEnumerable<string> Models { get; set; }
        public string ExportType { get; set; }
        public string ExportFileName { get; set; }       
        public string SnippetIdentifier { get; set; }
        public string Body { get; set; }
        
    }
}