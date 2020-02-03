using System.Collections.Generic;
using System.Threading.Tasks;

namespace ModelHelper.Data.Demo.MsSql
{
    public class MsSqlDemoConnection : ModelHelper.Data.IDatabaseConnection
    {
        internal List<IEntity> _entities;
        public MsSqlDemoConnection()
        {
            _entities = new List<IEntity>{
                DemoTables.PersonEntity(),
                DemoTables.PhoneEntity(),
                DemoTables.PersonPhoneEntity()
            };
        }
        public string DatabaseType => "mssql";

        public bool CanReorganizeIndexes => true;

        public bool CanRebuildIndexes => true;

        public bool CanTraverseRelations => true;

        public Task<bool> DumpContentAsJson(string entityName, string path, int limit = -1)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<IColumn>> GetColumns(string schema, string entityName)
        {
            throw new System.NotImplementedException();
        }

        public async Task<IEnumerable<IEntity>> GetEntities(bool tablesOnly = false, bool viewsOnly = false, string filter = "", string columnName = "")
        {
            return _entities;
        }

        public Task<IEntity> GetEntity(string entityName, bool includeChildRelations = false)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<IIndex>> GetIndexes(string entityName)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<IEntityRelation>> GetParentEntityRelations(string entityName, bool includeColumns = false)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<IEntityRelation>> GetRelatedChildren(string entityName, bool includeColumns = false)
        {
            throw new System.NotImplementedException();
        }

        public Task<int> ImportJsonData(IEntity entity, string json)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> OptimizeDatabase()
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> RebuildIndex(string indexName, string entityName, double fillFactor = 80)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> ReorganizeIndex(string indexName, string entityName)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<string> ReservedKeywords()
        {
            throw new System.NotImplementedException();
        }

        public Task<IEntityGroup> SuggestEntityGroup(string schema, string entityName)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<IEntityName>> SuggestEntityGroupName(string entityName)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<IEntityGroup>> SuggestEntityGroups()
        {
            throw new System.NotImplementedException();
        }

        public Task<IDatabaseInformation> TestConnectionAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<IRelation>> TraverseRelations(string baseTable, int depth = 1, int maxLevel = -1)
        {
            throw new System.NotImplementedException();
        }
    }

    public static class DemoTables
    {
        public static IEntity PersonEntity()
        {
            var entity = new DemoEntity
            {
                Name = "Person",
                ModelName = "Person",
                ContextualName = "Person",
                Type = "table",
                Schema = "dbo",
                Alias = "p",
                RowCount = 1020,
                UsesIdentityColumn = true,
                UsesGuidAsPrimaryKey = false,
                UsesDeletedColumn = true,
                DeletedColumnName = "IsActive",
                Columns = new List<IColumn>{
                    new DemoColumn{Name = "Id", PropertyName = "Id", IsPrimaryKey = true, IsIdentity = true},
                    new DemoColumn{Name = "FirstName", PropertyName = "FirstName", DbType = "varchar", Length = 100},
                    new DemoColumn{Name = "LastName", PropertyName = "LastName", DbType = "varchar", Length = 100},
                },
                ChildRelations = new List<IEntityRelation>
                {
                    new DemoRelation{},
                }
                
            };
            

            return entity;
        }

        public static IEntity PersonPhoneEntity()
        {
            var entity = new DemoEntity
            {
                Name = "PersonPhone",
                ModelName = "PersonPhone",
                ContextualName = "PersonPhone",
                Type = "table",
                Schema = "dbo",
                Alias = "pp",
                RowCount = 1020,
                UsesIdentityColumn = true,
                Columns = new List<IColumn>{
                    new DemoColumn{Name = "Id", PropertyName = "Id", IsPrimaryKey = true, IsIdentity = true},
                    new DemoColumn{Name = "PhoneId", DbType = "int"},
                    new DemoColumn{Name = "PersonId", DbType = "int"},
                }
                
            };
            

            return entity;
        }
        public static IEntity PhoneEntity()
        {
            var entity = new DemoEntity
            {
                Name = "Phone",
                ModelName = "Phone",
                ContextualName = "Phone",
                Type = "table",
                Schema = "dbo",
                Alias = "p",
                RowCount = 1020,
                UsesIdentityColumn = true,
                UsesGuidAsPrimaryKey = false,
                UsesDeletedColumn = true,
                DeletedColumnName = "IsActive",
                Columns = new List<IColumn>{
                    new DemoColumn{Name = "Id", PropertyName = "Id", IsPrimaryKey = true, IsIdentity = true},
                    new DemoColumn{Name = "PhoneType", PropertyName = "PhoneType", DbType = "varchar", Length = 100},
                    new DemoColumn{Name = "Number", PropertyName = "Number", DbType = "varchar", Length = 100},
                }
            };
            

            return entity;
        }
    }

    public class DemoRelation : IEntityRelation
    {
        public DemoRelation()
        {
        }
        public DemoRelation(IEntity parent, IEntity child, IColumn parentColumn, IColumn childColumn)
        {
            ParentColumnName = parentColumn.Name;
            ParentColumnType = parentColumn.DataType;
            ParentColumnNullable = parentColumn.IsNullable;
            ChildColumnName = childColumn.Name;
            ChildColumnNullable = childColumn.IsNullable;
            ChildColumnType = childColumn.DataType;
            Entity = parent;
            
        }

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

    public class DemoColumn : IColumn
    {
        public string Name {get; set;}
        public string PropertyName {get; set;} //nødvendig?
        public string ContextualName {get; set;} //nødvendig?
        public string DbType {get; set;} //nødvendig?
        public string DataType {get; set;}
        public string Collation {get; set;}
        public bool IsPrimaryKey {get; set;}
        public bool IsForeignKey {get; set;}
        public bool IsNullable {get; set;}
        public bool IsIdentity {get; set;}
        public bool IsIgnored {get; set;}
        public bool IsCreatedByUser {get; set;}
        public bool IsCreatedDate {get; set;}
        public bool IsModifiedByUser {get; set;}
        public bool IsModifiedDate {get; set;}
        public bool IsDeletedMarker {get; set;}
        public bool Selected {get; set;}
        public int Precision {get; set;}
        public int Scale {get; set;}
        public int Length {get; set;}
        public bool UsePrecision {get; set;}
        public bool UseLength {get; set;}
        public bool UseInViewModel {get; set;}
        public bool IsReserved {get; set;}
        public string ReferencesTable {get; set;}
        public string ReferencesColumn {get; set;}
        public string Description {get; set;}
    }

    public class DemoEntity : IEntity
    {
        public string Name {get; set;}
        public string ModelName {get; set;}
        public string ContextualName {get; set;}
        public string Type {get; set;}
        public string Schema {get; set;}
        public string Alias {get; set;}
        public int RowCount {get; set;}
        public string ApiResourceAddress {get; set;}
        public bool UsesIdentityColumn {get; set;}
        public bool UsesGuidAsPrimaryKey {get; set;}
        public bool UsesDeletedColumn {get; set;}
        public string DeletedColumnName {get; set;}
        public List<IColumn> Columns {get; set;}
        public IEnumerable<IEntityRelation> ParentRelations {get; set;}
        public IEnumerable<IEntityRelation> ChildRelations {get; set;}
        public IEnumerable<IIndex> Indexes {get; set;}
        public string Description {get; set;}
    }
}