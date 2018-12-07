using ModelHelper.Core.Database;
using ModelHelper.Extensibility;

namespace ModelHelper.Core.Models
{
    public class Column : IColumn
    {
        public string Name { get; set; }
        public string PropertyName { get; set; }
        public string DbType { get; set; }
        public string DataType { get; set; }
        public string Collation { get; set; }
        public bool IsPrimaryKey { get; set; }
        public bool IsForeignKey { get; set; }
        public bool IsNullable { get; set; }
        public bool IsIdentity { get; set; }
        public bool IsIgnored { get; set; }
        public bool IsCreatedByUser { get; set; }
        public bool IsCreatedDate { get; set; }
        public bool IsModifiedByUser { get; set; }
        public bool IsModifiedDate { get; set; }
        public bool IsDeletedMarker { get; set; }
        public bool Selected { get; set; }
        public int Precision { get; set; }
        public int Scale { get; set; }
        public int Length { get; set; }
        public bool UsePrecision { get; set; }
        public bool UseLength { get; set; }
        public bool UseInViewModel { get; set; }
        public bool IsReserved { get; set; }
        public string ReferencesTable { get; set; }
        public string ReferencesColumn { get; set; }

        public string Description { get; set; }
    }
}