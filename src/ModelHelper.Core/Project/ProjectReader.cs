using System.IO;
using Newtonsoft.Json;
using ModelHelper.IO;
using System;

namespace ModelHelper.Core.Project
{

    public class ProjectReader  //IProjectReader<Project3>
    {
        public ProjectReader()
        {
        }

        public Project3 Read(string path)
        {



            var reader = new JsonReader<Project3>();
            var p = reader.Read(path);

            if (p != null)
            {
                p.Exists = true;
                return p;
            }
            else
            {
                var project = new Project3 { Exists = false };
                project.Exists = true;

                return project;

            }

            // var content = System.IO.File.ReadAllText(path);

            //     if (string.IsNullOrEmpty(content))
            //     {
            //         return project;
            //     }

            //     project = JsonConvert.DeserializeObject<Project3>(content);

        }
    }
}