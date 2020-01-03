using ModelHelper.Core.Templates;
using System.IO;

namespace ModelHelper.Extensions
{
    public static class TemplateExtensions
    {
        public static Template3 OpenTemplate(this string templateFile)
        {
            if (!File.Exists(templateFile))
            {
                throw new FileNotFoundException($"The file '{templateFile}' could not be found");
            }

            var yaml = System.IO.File.ReadAllText(templateFile);
            return LoadTemplateFromContent(yaml);
            
        }

        public static Template3 LoadTemplateFromContent(string content)
        {
            
            var yamlDeserializer = new YamlDotNet.Serialization.Deserializer();          
            var template = yamlDeserializer.Deserialize<Template3>(content);

            return template;
        }

    }

    
}