using ModelHelper.Core.Project;
using DotLiquid;
using ModelHelper.Core.Project.V1;

namespace ModelHelper.Core.Drops
{
    public class ProjectDrop : Drop
    {
        public string Name { get; }
        public string RootNamespace { get; }
        public string Description { get; }
        public ProjectDrop(IProject project)
        {
            Name = project.Name;
            RootNamespace = project.RootNamespace;
            Description = project.Description;
        }
    }
}