using System.Collections.Generic;
using ModelHelper.Core.Database;
using ModelHelper.Extensibility;

namespace ModelHelper.Core.Models
{
    public class Table : IEntity
    {
        public Table()
        {
            Columns = new List<IColumn>();
            ParentRelations = new List<ITableRelation>();
            ChildRelations = new List<ITableRelation>();
        }
        public string Name { get; set; }
        public string ModelName { get; set; }
        public string Type { get; set; }
        public string Schema { get; set; }
        public string Alias { get; set; }
        public int RowCount { get; set; }
        public string ApiResourceAddress { get; set; }

        public bool UsesIdentityColumn { get; set; }
        public bool UsesGuidAsPrimaryKey { get; set; }
        public bool UsesDeletedColumn { get; set; }
        public string DeletedColumnName { get; set; }

        public List<IColumn> Columns { get; set; }
        public IEnumerable<ITableRelation> ParentRelations { get; set; }
        public IEnumerable<ITableRelation> ChildRelations { get; set; }
        public IEnumerable<IIndex> Indexes { get; set; } = new List<Index>();
    }

    public class Index : IIndex
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public double Size { get; set; } = 0;

        public double AvgFragmentationPercent { get; set; } = 0;

        public bool IsClustered { get; set; }

        public bool IsPrimaryKey { get; set; }

        public bool IsUnique { get; set; }

        public double AvgPageSpacePercent { get; set; } = 0;

        public double AvgRecordSize { get; set; } = 0;

        public int Rows { get; set; } = 0;

        public IEnumerable<IIndexColumn> Columns { get; set; } = new List<IndexColumn>();
    }

    public class IndexColumn : IIndexColumn
    {
        public string Name { get; set; }

        public bool IsDescending { get; set; }

        public bool IsNullable { get; set; }

        public bool IsIdentity { get; set; }

        public double PartitionOrginal { get; set; } = 0;
    }
}