using System.Collections.Generic;

namespace ModelHelper.Extensibility
{
    public interface IEntityGroup
    {
        string Name { get; set; }
        List<IEntityName> Entities { get; set; }
    }
}