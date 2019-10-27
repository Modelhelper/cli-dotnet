using System.Collections.Generic;

namespace ModelHelper.Data
{
    public interface IEntityGroup
    {
        string Name { get; set; }
        List<IEntityName> Entities { get; set; }
    }
}