using System.Collections.Generic;

namespace ModelHelper.Core.Templates
{
    public interface IJsonTemplate
    {
        string Key { get; set; }
        string Name { get; set; }
        string Description { get; set; }
        string Language { get; set; }
        bool? CanExport { get; set; }
        string ExportFileName { get; set; }

        string ExportType { get; set; }

        TemplateType TemplateType { get; set; }

        IEnumerable<string> Groups { get; set; }
        IEnumerable<string> Tags { get; set; }

        IEnumerable<string> Body { get; set; }
    }
}