using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelHelper.Core
{
    public class Constants
    {
        public static string HELP_INIT_TITLE = "title";
        public static string HELP_INIT_WELCOME = "welcomeMessage";

        public static readonly string UNSET = "UNSET";
        public static readonly string TEMPLATE = "template";

        public static readonly string ConfigFileName = "config.yaml";

        public static readonly string ProjectDirectoryName = ".model-helper";
        public static readonly string ProjectFileName = "project.json";

        internal static readonly string RemoteProjectDefinitionZipFile = "project-definitions.zip";
        internal static readonly string RemoteCodeDefinitionZipFile = "code-definitions.zip";
        internal static readonly string RemoteTemplateZipFile = "mh-templates.zip";

    }

    public static class ApplicationDefaults
    {
        public static DirectoryInfo RootDirectory
        {
            get
            {
                var p = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "ModelHelper");
                return new DirectoryInfo(p);
            }
        }

        public static DirectoryInfo CurrentProjectDirectory
        {
            get
            {
                var p = Path.Combine(Directory.GetCurrentDirectory(), Constants.ProjectDirectoryName);
                return new DirectoryInfo(p);
            }
        }

        public static FileInfo CurrentProjectFile
        {
            get
            {
                var p = Path.Combine(Directory.GetCurrentDirectory(), Constants.ProjectDirectoryName, Constants.ProjectFileName);
                return new FileInfo(p);
            }
        }
    }
}
