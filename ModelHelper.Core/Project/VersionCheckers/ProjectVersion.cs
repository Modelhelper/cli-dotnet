using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelHelper.Core.Project.VersionCheckers
{
   

    public static class ReadProjectVersion
    {
        public static ProjectVersion ReadVersion(string path)
        {
            var version = new ProjectVersion();

            if (File.Exists(path))
            {
                var content = System.IO.File.ReadAllText(path);

                if (string.IsNullOrEmpty(content))
                {
                    return null;
                }

                version = JsonConvert.DeserializeObject<ProjectVersion>(content);

                return version;
            }

            return null;

        }


    }
}
