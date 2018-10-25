using System.IO;

namespace ModelHelper.Core.Extensions
{
    public class TemplateFile
    {
        public string Name { get; set; }
        public FileInfo FileInfo { get; set; }
        public string Location { get; set; }
        public string Scope { get; set; }
        public string SubFolder { get; set; }
    }
}