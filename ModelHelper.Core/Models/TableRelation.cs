using ModelHelper.Core.Database;
using ModelHelper.Extensibility;

namespace ModelHelper.Core.Models
{
    public class TableRelation : Table, ITableRelation
    {        
        public int GroupIndex { get; set; }
        public string ConstraintName { get; set; }
        public string ParentColumnName { get; set; }
        public string ChildColumnName { get; set; }
        public string ParentColumnType { get; set; }
        public string ChildColumnType { get; set; }
        public bool ParentColumnNullable { get; set; }
        public bool ChildColumnNullable { get; set; }
        public bool IsSelfJoin { get; set; }
    }

    public class Relation : IRelation
    {
        public int SortIndex { get; set; }
        public int Level { get; set; }
        public string FullPath { get; set; }
        public int Depth { get; set; }
        public string Family { get; set; }
        public string ReferenceName { get; set; }
        public string ParentTableName { get; set; }
        public string ParentColumnName { get; set; }
        public string ParentColumnType { get; set; }
        public string ReferencedTableName { get; set; }
        public string ReferencedColumnName { get; set; }
        public string ReferencedColumnType { get; set; }
        public bool IsSelfJoin { get; set; }
    }

    public class DatabaseInformation : IDatabaseInformation
    {
        public string Version {get; set;}

        public string ServerName {get; set;}
    }
}