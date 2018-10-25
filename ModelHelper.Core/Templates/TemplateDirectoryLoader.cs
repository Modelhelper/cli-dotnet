using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ModelHelper.Core.Templates
{
    public static class TemplateDirectoryLoader
    {
        public static IEnumerable<FileInfo> LoadFrom(string path)
        {
            var files = new List<FileInfo>();

            if (Directory.Exists(path))
            {
                var templates = Directory.EnumerateFiles(path);

                files = new List<FileInfo>(templates.Select(f => new FileInfo(f)));
            }


            return files;
        }
    }
}