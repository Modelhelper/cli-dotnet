using System.Collections.Generic;
using ModelHelper.Core.Database;
using ModelHelper.Extensibility;

namespace ModelHelper.Core.Models
{
    public class EntityGroup : IEntityGroup
    {
        public EntityGroup()
        {
            Entities = new List<IEntityName>();
        }
        public string Name { get; set; }
        public List<IEntityName> Entities { get; set; }
    }
}