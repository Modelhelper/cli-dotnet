using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelHelper.Core.Extensions;
using ModelHelper.Core.Project.Converters;
using ModelHelper.Core.Project.V1;

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
            IProject project = new Project();
            if (File.Exists(path))
            {
                if(version != null && version.IsBeta)
                {
                    var betaConverter = new ConvertProjectFromBetaToVersion1();

                    var betaProject = betaConverter.LoadBetaProject(path);

                    project = betaProject.ToVersion1().ToVersion2();
                    
                }

                if(version != null && version.Major.HasValue && version.Major.Value == 1 )
                {
                    var converter = new ConvertProjectFromVersion1();
                    var reader = new V1.ProjectV1Reader();
                    var projectV1 = reader.Read(path);

                    project = projectV1.ToVersion2();

                }

                if(project != null)
                {
                    var projectWriter = new DefaultProjectWriter();
                    projectWriter.Write(path, project);
                    result.Converted = true;
                    result.Version = project.Version;
                }
            }

            return result;
        }
    }
}
