using ModelHelper.Core.Database;
using ModelHelper.Extensibility;

namespace ModelHelper.Core.Templates
{
    public interface ITemplateExporter
    {
        void Export(string path, string content, IEntity table);
    }
}