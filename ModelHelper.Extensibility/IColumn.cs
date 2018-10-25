namespace ModelHelper.Extensibility
{
    public interface IColumn
    {
        string Name { get; set; }
        string PropertyName { get; set; }

        string DbType { get; set; }
        string DataType { get; set; }

        string Collation { get; set; }

        bool IsPrimaryKey { get; set; }

        bool IsForeignKey { get; set; }
        bool IsNullable { get; set; }
        bool IsIdentity { get; set; }

        bool IsIgnored { get; set; }
        bool IsCreatedByUser { get; set; }
        bool IsCreatedDate { get; set; }

        bool IsModifiedByUser { get; set; }
        bool IsModifiedDate { get; set; }
        bool IsDeletedMarker { get; set; }


        bool Selected { get; set; }

        int Precision { get; set; }
        int Scale { get; set; }
        int Length { get; set; }
        
        bool UsePrecision { get; set; }
        bool UseLength { get; set; }

        bool UseInViewModel { get; set; }

        bool IsReserved { get; set; }

        string ReferencesTable { get; set; }
        string ReferencesColumn { get; set; }

    }
}