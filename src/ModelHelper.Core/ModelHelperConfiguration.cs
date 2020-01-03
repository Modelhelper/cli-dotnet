namespace ModelHelper.Core
{
    public class ModelHelperConfiguration : IModelHelperConfiguration
    {
        public string ConfigVersion { get; set; } = "3";
        public string AppVersion { get; set; } = "3.0";
        public string RemoteTemplateDownloadLocation { get; set; }
        public string LogLocation { get; set; }

        public ConfigLocation Shared { get; set; } = new ConfigLocation();
        public ConfigLocation Global { get; set; } = new ConfigLocation();

        public static ModelHelperConfiguration CreateDefault()
        {
            var config = new ModelHelperConfiguration
            {
                RemoteTemplateDownloadLocation = "https://github.com/Modelhelper/templates/releases/latest/download/mh-templates.zip"
            };
                config.Global.CodeDefinitions = ".\\def";
                config.Global.ProjectDefinitions = ".\\def";
                config.Global.TemplateLocation = ".\\templates";

            return config;
        }
    }
    public class ConfigLocation
    {
        public string TemplateLocation { get; set; } = "NOTSET";
        public string ProjectDefinitions { get; set; } = "NOTSET";
        public string CodeDefinitions { get; set; } = "NOTSET";

    }
}