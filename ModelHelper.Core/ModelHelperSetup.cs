using ModelHelper.Core.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelHelper.Core
{
    
    public static class ModelHelperSetup
    {
        public static DirectoryInfo CreateRootDirectory()
        {
            
            if (!ApplicationDefaults.RootDirectory.Exists)
            {
                ApplicationDefaults.RootDirectory.Create();
                
            }

            return ApplicationDefaults.RootDirectory;
        }

        public static GlobalConfig CreateConfigFile(this DirectoryInfo root, ConfigLocation sharedLocations)
        {
            var config = new GlobalConfig { Shared = sharedLocations };


            return config;
        }

        public static GlobalConfig SaveConfigFile(this GlobalConfig config)
        {
            var writer = new Configuration.GlobalConfigWriter();
            writer.Write(config);            

            return config;
        }

        //public static DirectoryInfo CreateGlobalTemplateDirectory()
        //{

        //}

        //public static int Add


        //public static DirectoryInfo CreateGlobalTemplateDirectory()
        //{

        //}

        //public string RootDirectory { get; private set; }
        //public GlobalConfig Configuration { get; private set; }
    }

    public interface IGlobalTemplateDirectory
    {
        string Path { get; }
    }
}
