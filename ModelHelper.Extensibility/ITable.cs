using System.Collections.Generic;

namespace ModelHelper.Extensibility
{
    public interface IEntity
    {
        string Name { get; set; }

        string ModelName { get; set; }

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

        IEnumerable<ITableRelation> ParentRelations { get; set; }
        IEnumerable<ITableRelation> ChildRelations { get; set; }

        IEnumerable<IIndex> Indexes { get; set; }

        string Description { get; set; }
    }

    public interface IIndex
    {
        string Id { get;  }
        string Name { get; }

        double Size { get;  }
        double AvgFragmentationPercent { get;  }

        bool IsClustered { get;  }
        bool IsPrimaryKey { get;  }

        bool IsUnique { get; }

        double AvgPageSpacePercent { get; }
        double AvgRecordSize { get; }
        int Rows { get; } 

    IEnumerable<IIndexColumn> Columns { get; }

 
    }

    public interface IIndexColumn
    {
        string Name { get; }
        bool IsDescending { get; }
        bool IsNullable { get; }
        bool IsIdentity { get; }
        double PartitionOrginal { get; }

        
    }
}