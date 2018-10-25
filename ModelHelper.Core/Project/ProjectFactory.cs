using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelHelper.Core.Project
{
    public static class ProjectFactory
    {
        public static IProject CreateDefault()
        {
            return new Project();
        }

        public static ConversionResult Upgrade(string path, ProjectVersion version = null)
        {
            var result = new ConversionResult { Converted = false };
            if (File.Exists(path))
            {
                if(version != null && version.IsBeta)
                {
                    var converter = new ConvertProjectFromBetaToVersion1();
                    var project = converter.Convert(path);

                    if (project != null)
                    {
                        var projectWriter = new ProjectJsonWriter();
                        projectWriter.Write(path, project);
                        result.Converted = true;
                        result.Version = project.Version;
                    }
                }
            }

            return result;
        }
    }
}
