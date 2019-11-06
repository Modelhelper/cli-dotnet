using System.Collections.Generic;

namespace ModelHelper.Templates
{
    public interface ITemplate
    {
        string Key { get; set; }
        string Version { get; set; }
        string Language { get; set; }
        string Description { get; set; }
        string ShortDescription { get; set; }
        IEnumerable<string> Tags { get; set; }
        IEnumerable<string> Groups { get; set; }
        IEnumerable<string> Models { get; set; }
        string ExportType { get; set; }
        string ExportFileName { get; set; }       
        string SnippetIdentifier { get; set; }
        string Body { get; set; }

    }

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