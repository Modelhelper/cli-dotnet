using ModelHelper.Templates;

namespace ModelHelper.Core.Templates
{
    public interface ITemplateWriter<T>
    {
        void Write(string path, T template);
    }
}