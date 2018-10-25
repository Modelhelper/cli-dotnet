using ModelHelper.Core.Project;
using DotLiquid;

namespace ModelHelper.Core.Drops
{
    public class ProjectDrop : Drop
    {
        public string Name { get; }
        public string Customer { get; }
        public string Description { get; }
        public ProjectDrop(IProject project)
        {
            Name = project.Name;
            Customer = project.Customer;
            Description = project.Customer;
        }
    }
}