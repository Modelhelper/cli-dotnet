using System.Collections.Generic;

namespace ModelHelper.Core.Templates
{
    public class Template3
    {
        public string Key { get; set; }
        public string Version { get; set; }
        public string Language { get; set; }
        public string Description { get; set; }
        public string ShortDescription { get; set; }

        public List<string> Tags { get; set; } = new List<string>();
        public List<string> Groups { get; set; } = new List<string>();
        public List<string> Models { get; set; } = new List<string>();
        public string ExportType { get; set; }
        public string ExportFileName { get; set; }
        public string SnippetIdentifier { get; set; }
        public string Body { get; set; }
    }
}