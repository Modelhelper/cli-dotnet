using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using ModelHelper.Core.Extensions;
using Newtonsoft.Json;

namespace ModelHelper.Core
{
    public static class ModelHelperConfig
    {
        private static Config _config;
        public static Config ReadConfig()
        {
            var path = Path.Combine(ModelHelperExtensions.RootFolder, "config.json");

            return ReadConfig(path);
            
            //var config = new Config();
            //if (File.Exists(path))
            //{
            //    var content = System.IO.File.ReadAllText(path);

            //    if (!string.IsNullOrEmpty(content))
            //    {
                    

            //    _config = JsonConvert.DeserializeObject<Config>(content);
            //        config = _config;
            //    }
            //}

            //return config;
        }

        public static Config ReadConfig(string path)
        {           
            var config = new Config();
            if (File.Exists(path))
            {
                var content = System.IO.File.ReadAllText(path);

                if (!string.IsNullOrEmpty(content))
                {


                    _config = JsonConvert.DeserializeObject<Config>(content);
                    config = _config;
                }
            }

            return config;
        }


        public static void SaveConfig(Config config)
        {
            var path = Path.Combine(ModelHelperExtensions.RootFolder, "config.json");
            if (File.Exists(path))
            {
                var json = JsonConvert.SerializeObject(config);
                System.IO.File.WriteAllText(path, json);
            }
        }

        public static string RemoteTemplateLocation => _config != null ? _config.RemoteTemplateLocation :"";
        public static string RemoteBinaryLocation => _config != null ? _config.RemoteBinaryLocation : "";

        //public static string ScriptLocation => _config != null ? _config.ScriptLocation : "";

    }

    //public class ConfigReader
    //{
    //    public Config Read()
    //}
}