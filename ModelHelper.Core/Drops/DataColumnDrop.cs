using ModelHelper.Core.Database;
using ModelHelper.Core.Extensions;
using DotLiquid;
using ModelHelper.Extensibility;

namespace ModelHelper.Core.Drops
{
    public class DataColumnDrop : Drop
    {
        private IColumn _column;

        public DataColumnDrop(IColumn column)
        {
            _column = column;
            
        }

        public string Description => _column.Description;
        public bool IsForeignKey => _column.IsForeignKey;
        public bool IsPrimaryKey => _column.IsPrimaryKey;
        public bool IsIdentity => _column.IsIdentity;
        public bool IsNullable => _column.IsNullable;
        public bool IsIgnored => _column.IsIgnored;
        public bool IsDeletedMarker => _column.IsDeletedMarker;
        public bool IsCreatedDate => _column.IsCreatedDate;
        public bool IsCreatedByUser => _column.IsCreatedByUser;
        public bool IsModifiedDate => _column.IsModifiedDate;
        public bool IsModifiedByUser => _column.IsModifiedByUser;

        public string Name => _column.Name;
        public string PropertyName => _column.PropertyName;
        public string Collation => _column.Collation;
        public string ReferencesColumn => _column.ReferencesColumn;
        public string ReferencesTable => _column.ReferencesTable;

        public string DataType => _column.DataType;
        public string DbType => _column.DbType;

        public int Length => _column.Length;        
        public int Precision => _column.Precision;        
        public int Scale => _column.Scale;        

        public bool UseLength => _column.UseLength;
        public bool UsePrecision => _column.UsePrecision;
        public bool UseInViewModel => _column.UseInViewModel;
    }
}