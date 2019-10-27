namespace ModelHelper.Data
{
    public interface IRelation
    {
        int SortIndex { get; set; }
        int Level { get; set; }

        string FullPath { get; set; }
        int Depth { get; set; }

        string Family { get; set; }

        string ReferenceName { get; set; }

        string ParentTableName { get; set; }
        string ParentColumnName { get; set; }
        string ParentColumnType { get; set; }

        string ReferencedTableName { get; set; }
        string ReferencedColumnName { get; set; }
        string ReferencedColumnType { get; set; }

        bool IsSelfJoin { get; set; }
    }
}