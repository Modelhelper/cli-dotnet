using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using ModelHelper.Core.Extensions;
using ModelHelper.Core.Templates;
using Newtonsoft.Json;

namespace ModelHelper.Core.Configuration
{

    [Obsolete]
    public static class ModelHelperConfig
    {
        private static Config _config;

        public static List<ITemplate> Templates { get; set; } = new List<ITemplate>();

        //public static void LoadTemplates(params string[] templateLocations)
        //{
        //    foreach (var location in templateLocations)
        //    {

        //    }
        //    var modelHelperData = ConsoleExtensions.UserTemplateDirectory();

        //    var templateReader = new JsonTemplateReader();
        //    var customTemplatePath = ModelHelperConfig.TemplateLocation; // Path.Combine(Directory.GetCurrentDirectory(), "templates");

        //    var templateFiles = new List<TemplateFile>();

        //    templateFiles.AddRange(customTemplatePath.GetTemplateFiles("project"));
        //    templateFiles.AddRange(modelHelperData.GetTemplateFiles("mh"));

        //}
        public static Config ReadConfig()
        {
            var path = Path.Combine(ModelHelperExtensions.RootFolder, "config.json");

            return ReadConfig(path);
        }

        public static Config ReadConfig(string path)
        {
            var config = new Config();
            if (File.Exists(path))
            {
                var content = File.ReadAllText(path);

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
                File.WriteAllText(path, json);
            }
        }

        internal static string GetTemplateLocation()
        {
            var location = _config != null && !string.IsNullOrEmpty(_config.TemplateLocation)
                ? _config.TemplateLocation
                : Path.Combine(Directory.GetCurrentDirectory(), "templates");

            return location;
        }
        public static string RemoteTemplateLocation => _config != null ? _config.RemoteTemplateLocation : "";
        public static string RemoteBinaryLocation => _config != null ? _config.RemoteBinaryLocation : "";

        public static string TemplateLocation => GetTemplateLocation();
        //_config != null && !string.IsNullOrEmpty(_config.TemplateLocation) ? 
        //_config.TemplateLocation : 
        //Path.Combine(Directory.GetCurrentDirectory(), "templates");

        //public static string ScriptLocation => _config != null ? _config.ScriptLocation : "";

    }

    //public class ConfigReader
    //{
    //    public Config Read()
    //}
}