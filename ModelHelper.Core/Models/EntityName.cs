using ModelHelper.Core.Database;
using ModelHelper.Extensibility;

namespace ModelHelper.Core.Models
{
    public class EntityName : IEntityName
    {
        public string Schema { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
    }
}