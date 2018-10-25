using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ModelHelper.Core.Models;
using Newtonsoft.Json;

namespace ModelHelper.Core.Templates
{
    public class JsonTemplateWriter : ITemplateWriter
    {
        public ITemplate Read(string path)
        {
            if (File.Exists(path))
            {
                var stream = new StreamReader(new FileStream(path, FileMode.Open));
                var content = stream.ReadToEnd();

                var jsonTemplate = JsonConvert.DeserializeObject<JsonTemplate>(content);
                var bodyBuilder = new StringBuilder();

                if (jsonTemplate == null)
                {
                    return null;
                }

                if (jsonTemplate?.Body != null && jsonTemplate.Body.Any())
                {
                    foreach (var builder in jsonTemplate.Body)
                    {
                        bodyBuilder.AppendLine(builder);
                    }
                }
                

                var output = new Template
                {
                    Key = jsonTemplate.Key,
                    Name = jsonTemplate.Name,
                    Tags = jsonTemplate.Tags,
                    ExportFileName = jsonTemplate.ExportFileName,
                    CanExport = jsonTemplate.CanExport != null && jsonTemplate.CanExport.Value  ? true : false,
                    Description = jsonTemplate.Description,
                    Language = jsonTemplate.Language,
                    Body = bodyBuilder.ToString()
                };

                return output;
            }

            return null;
            
        }

        public void Write(string path, ITemplate template)
        {
            var splitter = new List<string> { Environment.NewLine, "\n" };

            var jsonTemplate = new JsonTemplate
            {
                Key = template.Key,
                Name = template.Name,
                Tags = template.Tags,
                Groups = template.Groups,
                ExportType = template.ExportType,
                ExportFileName = template.ExportFileName,
                CanExport = template.CanExport ,
                Description = template.Description,
                Language = template.Language,
                Body = template.Body != null ? template.Body.Split(splitter.ToArray(), StringSplitOptions.None) : new List<string>().ToArray()
            };
            
            var jsonData = JsonConvert.SerializeObject(jsonTemplate);

            System.IO.File.WriteAllText(path, jsonData);
            
            
        }
    }
}