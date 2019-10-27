using ModelHelper.Core.Configuration;
using ModelHelper.Core.Templates;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelHelper
{
    public static class Application
    {

        public static string RootFolder
        {
            get
            {
                return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                    "ModelHelper");
            }
        }

        public static bool ProjectFileExists()
        {
            var file = Path.Combine(Directory.GetCurrentDirectory(), ".model-helper", "project.json");
            return File.Exists(file);
        }
        public static bool ProjectDirectoryExists()
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), ".model-helper");
            return Directory.Exists(path);
        }

        public static bool RootDirectoryExists()
        {
            return Directory.Exists(RootFolder);
        }


        public static List<ITemplate> Templates { get; set; } = new List<ITemplate>();

        public static GlobalConfig Config { get; set; }


        //public static List<ITemplate> LoadTemplates()

    }
}
