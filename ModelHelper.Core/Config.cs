using System.Security.Policy;

namespace ModelHelper.Core
{
    public class Config
    {
        public  string RemoteTemplateLocation { get; set; }
        public string RemoteBinaryLocation { get; set; }

        public string ScriptLocation { get; set; }

        public string TemplateLocation { get; set; }

        public string DefaultConnectionString { get; set; }
    }
}