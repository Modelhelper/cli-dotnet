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
        public bool IsSelfJoin { get; set; }
    }
}