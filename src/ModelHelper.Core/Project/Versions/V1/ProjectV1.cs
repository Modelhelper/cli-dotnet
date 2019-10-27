using System.Collections.Generic;

namespace ModelHelper.Core.Project.V1
{
    public class ProjectV1 
    {
        public ProjectV1()
        {
            var Version = "1.0.0";
        }

        public string Customer { get; set; }

        public string RootPath { get; set; }
        public ProjectSourceSectionV1 DataSource { get; set; } = new ProjectSourceSectionV1();
       
       
    }

}

