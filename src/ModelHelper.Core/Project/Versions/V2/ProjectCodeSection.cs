using System.Collections.Generic;

namespace ModelHelper.Project.V2
{
    public class ProjectCodeSection
    {
        public bool UseQueryOptions { get; set; }
        public bool InjectUserContext { get; set; }

        public QueryOption QueryOptions { get; set; }

        public UserContext UserContext { get; set; }

        public List<ProjectCodeStructure> Locations { get; set; }

        public CodeConnectionSection Connection { get; set; }
    }
}