using System.Collections.Generic;
using ModelHelper.Core.Project.V1;

namespace ModelHelper.Core.Project
{
    public class ProjectCodeSection
    {
        public bool UseQueryOptions { get; set; }

        public QueryOption QueryOptions { get; set; }

        public List<ProjectCodeStructure> Locations { get; set; }

        public CodeConnectionSection Connection { get; set; }
    }
}