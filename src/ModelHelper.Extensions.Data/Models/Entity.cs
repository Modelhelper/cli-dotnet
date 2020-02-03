using System.Collections.Generic;

namespace ModelHelper.Data.Models
{
    public class Entity : IEntity
    {
        public string Name {get;set;}
        public string ModelName {get;set;}
        public string ContextualName {get;set;}
        public string Type {get;set;}
        public string Schema {get;set;}
        public string Alias {get;set;}
        public int RowCount {get;set;}
        public string ApiResourceAddress {get;set;}
        public bool UsesIdentityColumn {get;set;}
        public bool UsesGuidAsPrimaryKey {get;set;}
        public bool UsesDeletedColumn {get;set;}
        public string DeletedColumnName {get;set;}
        public List<IColumn> Columns {get;set;}
        public IEnumerable<IEntityRelation> ParentRelations {get;set;}
        public IEnumerable<IEntityRelation> ChildRelations {get;set;}
        public IEnumerable<IIndex> Indexes {get;set;}
        public string Description {get;set;}
    }

    public class Column : IColumn
    {
        public string Name {get;set;}
        public string PropertyName {get;set;}
        public string ContextualName {get;set;}
        public string DbType {get;set;}
        public string DataType {get;set;}
        public string Collation {get;set;}
        public bool IsPrimaryKey {get;set;}
        public bool IsForeignKey {get;set;}
        public bool IsNullable {get;set;}
        public bool IsIdentity {get;set;}
        public bool IsIgnored {get;set;}
        public bool IsCreatedByUser {get;set;}
        public bool IsCreatedDate {get;set;}
        public bool IsModifiedByUser {get;set;}
        public bool IsModifiedDate {get;set;}
        public bool IsDeletedMarker {get;set;}
        public bool Selected {get;set;}
        public int Precision {get;set;}
        public int Scale {get;set;}
        public int Length {get;set;}
        public bool UsePrecision {get;set;}
        public bool UseLength {get;set;}
        public bool UseInViewModel {get;set;}
        public bool IsReserved {get;set;}
        public string ReferencesTable {get;set;}
        public string ReferencesColumn {get;set;}
        public string Description {get;set;}
    }

    public class Relation : IEntityRelation
    {
        public int GroupIndex {get;set;}
        public string ConstraintName {get;set;}
        public string ParentColumnName {get;set;}
        public string ParentColumnType {get;set;}
        public string ChildColumnName {get;set;}
        public string ChildColumnType {get;set;}
        public bool ParentColumnNullable {get;set;}
        public bool IsSelfJoin {get;set;}
        public bool ChildColumnNullable {get;set;}
        public IEntity Entity {get;set;}
    }

    public class Index : IIndex
    {
        public string Id => throw new System.NotImplementedException();

        public string Name => throw new System.NotImplementedException();

        public double Size => throw new System.NotImplementedException();

        public double AvgFragmentationPercent => throw new System.NotImplementedException();

        public bool IsClustered => throw new System.NotImplementedException();

        public bool IsPrimaryKey => throw new System.NotImplementedException();

        public bool IsUnique => throw new System.NotImplementedException();

        public double AvgPageSpacePercent => throw new System.NotImplementedException();

        public double AvgRecordSize => throw new System.NotImplementedException();

        public int Rows => throw new System.NotImplementedException();

        public IEnumerable<IIndexColumn> Columns => throw new System.NotImplementedException();
    }
}