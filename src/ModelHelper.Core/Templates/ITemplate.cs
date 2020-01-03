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
}