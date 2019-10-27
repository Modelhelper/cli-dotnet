using ModelHelper.Templates;

namespace ModelHelper.Core.Templates
{
    public interface ITemplateWriter
    {
        void Write(string path, ITemplate template);
    }
}