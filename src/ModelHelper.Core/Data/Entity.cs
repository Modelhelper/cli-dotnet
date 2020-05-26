using System.Collections.Generic;

namespace ModelHelper.Data
{
    public sealed class Entity
    {
        public string Name { get; set; }

        public string ModelName { get; set; }        
        public string ContextualName { get; set; }
        public string Type { get; set; }
        public string Schema { get; set; }

        public string Alias { get; set; }

        public int RowCount { get; set; }

        public string ApiResourceAddress { get; set; }

        public bool UsesIdentityColumn { get; set; }

        public bool UsesGuidAsPrimaryKey { get; set; }

        public bool UsesDeletedColumn { get; set; }

        public string DeletedColumnName { get; set; }

        public List<Column> Columns { get; set; }

        public IEnumerable<EntityRelation> ParentRelations { get; set; }
        public IEnumerable<EntityRelation> ChildRelations { get; set; }

        public IEnumerable<Index> Indexes { get; set; }

        public string Description { get; set; }
    }
}