using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelHelper.Core.Configuration
{
    public class GlobalConfig
    {
        const string UNSET = "UNSET";
        public GlobalConfig()
        {
            Shared = new ConfigLocation
            {
                Template = UNSET,
                CodeDefinitions = UNSET,
                ProjectDefinitions = UNSET
            };

            Global = new ConfigLocation
            {
                Template = Path.Combine(ApplicationDefaults.RootDirectory.FullName,  "templates"),
                CodeDefinitions = Path.Combine(ApplicationDefaults.RootDirectory.FullName, "def"),                
                ProjectDefinitions = Path.Combine(ApplicationDefaults.RootDirectory.FullName, "projects"),                
            };

            InstalledCliVersion = 3;
            RemoteTemplateDownloadUrl = @"https://github.com/Modelhelper/templates/releases/latest/download/" + Constants.RemoteTemplateZipFile;
            RemoteProjectDefinitionDownloadUrl = @"https://github.com/Modelhelper/cli-dotnet/releases/latest/download/" + Constants.RemoteProjectDefinitionZipFile;
            RemoteCodeDefinitionDownloadUrl = @"https://github.com/Modelhelper/cli-dotnet/releases/latest/download/" + Constants.RemoteCodeDefinitionZipFile;

        }

        public bool AskToUpgradeProject { get; set; } = true;
        public int InstalledCliVersion { get; set; }
        public string RemoteTemplateDownloadUrl { get; set; }
        public string RemoteProjectDefinitionDownloadUrl { get; set; }
        public string RemoteCodeDefinitionDownloadUrl { get; set; }

        
        public string LogLocation { get; set; } = Constants.UNSET;

        public ConfigLocation Shared { get; set; }
        public ConfigLocation Global { get; set; }


        public static GlobalConfig Load()
        {
            var reader = new GlobalConfigReader();
            return reader.Read();
        }

        public static string YamlString()
        {
            var reader = new GlobalConfigReader();
            return reader.ReadYaml();
        }
    }

    public class ConfigLocation
    {
        public string Template { get; set; } = Constants.UNSET;
        public string CodeDefinitions { get; set; } = Constants.UNSET;
        public string ProjectDefinitions { get; set; } = Constants.UNSET;
    }

    public class GlobalConfigReader
    {
        private string path;
        public GlobalConfigReader()
        {
            path = System.IO.Path.Combine(ApplicationDefaults.RootDirectory.FullName, Constants.ConfigFileName);
        }
        public GlobalConfig Read()
        {
            
            var des = new YamlDotNet.Serialization.Deserializer();
            var yaml = System.IO.File.ReadAllText(path);
            return des.Deserialize<GlobalConfig>(yaml);

        }

        public string ReadYaml()
        {
            
            return System.IO.File.ReadAllText(path);
        }
    }

    public class GlobalConfigWriter
    {
        private string path;
        public GlobalConfigWriter()
        {
            path = System.IO.Path.Combine(ApplicationDefaults.RootDirectory.FullName, Constants.ConfigFileName);
        }
        
        public void Write(GlobalConfig config)
        {
            var ser = new YamlDotNet.Serialization.Serializer();
            var yaml = ser.Serialize(config);

            System.IO.File.WriteAllText(path, yaml);
        }
    }

}
