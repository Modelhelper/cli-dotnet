using System.Collections.Generic;

namespace ModelHelper.Data
{
    public interface IEntity
    {
        string Name { get; set; }

        string ModelName { get; set; }        
        string ContextualName { get; set; }
        string Type { get; set; }
        string Schema { get; set; }

        string Alias { get; set; }

        int RowCount { get; set; }

        string ApiResourceAddress { get; set; }

        bool UsesIdentityColumn { get; set; }

        bool UsesGuidAsPrimaryKey { get; set; }

        bool UsesDeletedColumn { get; set; }

        string DeletedColumnName { get; set; }

        List<IColumn> Columns { get; set; }

        IEnumerable<IEntityRelation> ParentRelations { get; set; }
        IEnumerable<IEntityRelation> ChildRelations { get; set; }

        IEnumerable<IIndex> Indexes { get; set; }

        string Description { get; set; }
    }
}