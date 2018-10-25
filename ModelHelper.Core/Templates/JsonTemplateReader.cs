using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ModelHelper.Core.Models;
using Newtonsoft.Json;

namespace ModelHelper.Core.Templates
{
    public class JsonTemplateReader : ITemplateReader
    {
        public ITemplate Read(string path)
        {
            if (File.Exists(path))
            {
                try
                {
                    var content = System.IO.File.ReadAllText(path);

                    if (string.IsNullOrEmpty(content))
                    {
                        return null;
                    }

                    var jsonTemplate = JsonConvert.DeserializeObject<JsonTemplate>(content);


                    if (jsonTemplate == null)
                    {
                        return null;
                    }

                    var bodyBuilder = new StringBuilder();

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
                        Tags = jsonTemplate.Tags != null ? jsonTemplate.Tags : new List<string>(),
                        ExportType = !string.IsNullOrEmpty(jsonTemplate.ExportType) ? jsonTemplate.ExportType : "",
                        ExportFileName = jsonTemplate.ExportFileName,
                        Groups = jsonTemplate.Groups != null ? jsonTemplate.Groups : new List<string>(),
                        CanExport = jsonTemplate.CanExport != null && jsonTemplate.CanExport.Value ? true : false,
                        Description = jsonTemplate.Description,
                        Language = jsonTemplate.Language,
                        Body = bodyBuilder.ToString()
                    };

                    return output;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return null;
                }
                
            }

            return null;

        }
    }
}