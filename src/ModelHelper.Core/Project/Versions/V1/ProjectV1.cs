using System.Collections.Generic;

namespace ModelHelper.Core.Project.V1
{
    public class Project1 
    {
        public Project1()
        {
            var Version = "1.0.0";
        }

        public string Customer { get; set; }

        public string RootPath { get; set; }
        public ProjectSourceSectionV1 DataSource { get; set; } = new ProjectSourceSectionV1();
       
       
    }

}

