using System.Collections.Generic;
using System.Linq;
using ModelHelper.Core.Database;
using DotLiquid;
using ModelHelper.Extensibility;

namespace ModelHelper.Core.Drops
{
    public class TableDrop : Drop, ITableDrop
    {
        private readonly IEntity _table;

        public TableDrop(IEntity table, bool includeChildRelations = true, bool includeParentRelations = true)
        {
            _table = table;

            Columns = new List<DataColumnDrop>();
            AllColumns = new List<DataColumnDrop>();
            IgnoredColumns = new List<DataColumnDrop>();
            PrimaryKeys = new List<DataColumnDrop>();
            UsedAsColumns = new List<DataColumnDrop>();

            ChildRelations = new List<RelatedTableDrop>();
            ParentRelations = new List<RelatedTableDrop>();
            IncludeChildRelations = includeChildRelations;
            IncludeParentRelations = includeParentRelations;

            foreach (var childRelation in table.ChildRelations)
            {
                ChildRelations.Add(new RelatedTableDrop(childRelation, table));
            }

            foreach (var parentRelation in table.ParentRelations)
            {
                ParentRelations.Add(new RelatedTableDrop(parentRelation, table));
            }


            //foreach (var column in _table.Columns.Where( c => !c.IsIgnored ))
            //{
            //    Columns.Add(new DataColumnDrop(column));
            //}

            foreach (var column in _table.Columns)
            {
                var drop = new DataColumnDrop(column);
                AllColumns.Add(drop);

                if (column.IsPrimaryKey)
                {
                    PrimaryKeys.Add(drop);
                }

                if (column.IsIgnored)
                {
                    IgnoredColumns.Add(drop);
                }
                else
                {
                    Columns.Add(drop);
                }

                if (column.IsCreatedByUser | column.IsCreatedDate | column.IsModifiedByUser | column.IsModifiedDate | column.IsDeletedMarker)
                {
                    UsedAsColumns.Add(drop);
                }
            }

            //foreach (var column in _table.Columns.Where(c => c.IsPrimaryKey))
            //{
            //    PrimaryKeys.Add(new DataColumnDrop(column));
            //}
            
        }

        public string Name => _table.Name;
        public string ModelName => _table.ModelName;
        public string Schema => _table.Schema;
        public string Alias => _table.Alias;

        public string Description => _table.Description;

        public bool UsesIdentityColumn => _table.UsesIdentityColumn;
        public bool UsesGuidAsPrimaryKey => _table.UsesGuidAsPrimaryKey;

        public bool UsesDeletedColumn => _table.UsesDeletedColumn;
        public string DeletedColumnName => _table.DeletedColumnName;

        public string ApiResourceAddress => "";

        public List<RelatedTableDrop> ChildRelations { get; }
        public List<RelatedTableDrop> ParentRelations { get; }
        public List<DataColumnDrop> Columns { get;  }
        public List<DataColumnDrop> AllColumns { get;  }
        public List<DataColumnDrop> IgnoredColumns { get;  }
        public List<DataColumnDrop> PrimaryKeys { get;  }

        public List<DataColumnDrop> UsedAsColumns { get; }
        
        public bool IncludeParentRelations { get; }
        public bool IncludeChildRelations { get; }
    }
}