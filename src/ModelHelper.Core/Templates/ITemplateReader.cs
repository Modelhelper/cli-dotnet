using ModelHelper.Templates;
using System.Runtime.InteropServices;

namespace ModelHelper.Core.Templates
{
    public interface ITemplateReader<T>
    {
        T Read(string path, string name);
        T ReadFromContent(string content, string name);
    }
}