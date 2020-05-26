using System.Collections.Generic;

namespace ModelHelper.Data
{
    public sealed class EntityGroup
    {
        public string Name { get; set; }
        public List<EntityName> Entities { get; set; }
    }
}