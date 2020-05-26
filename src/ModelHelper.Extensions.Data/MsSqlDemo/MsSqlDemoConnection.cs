using System.Collections.Generic;
using System.Threading.Tasks;

namespace ModelHelper.Data.Demo.MsSql
{
    public class MsSqlDemoConnection : ModelHelper.Data.IDatabaseConnection
    {
        internal List<Entity> _entities;
        public MsSqlDemoConnection()
        {
            _entities = new List<Entity>{
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

        public Task<IEnumerable<Column>> GetColumns(string schema, string entityName)
        {
            throw new System.NotImplementedException();
        }

        public async Task<IEnumerable<Entity>> GetEntities(bool tablesOnly = false, bool viewsOnly = false, string filter = "", string columnName = "")
        {
            return _entities;
        }

        public Task<Entity> GetEntity(string entityName, bool includeChildRelations = false)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<Index>> GetIndexes(string entityName)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<EntityRelation>> GetParentEntityRelations(string entityName, bool includeColumns = false)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<EntityRelation>> GetRelatedChildren(string entityName, bool includeColumns = false)
        {
            throw new System.NotImplementedException();
        }

        public Task<int> ImportJsonData(Entity entity, string json)
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

        public Task<EntityGroup> SuggestEntityGroup(string schema, string entityName)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<EntityName>> SuggestEntityGroupName(string entityName)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<EntityGroup>> SuggestEntityGroups()
        {
            throw new System.NotImplementedException();
        }

        public Task<IDatabaseInformation> TestConnectionAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<Relation>> TraverseRelations(string baseTable, int depth = 1, int maxLevel = -1)
        {
            throw new System.NotImplementedException();
        }
    }

    public static class DemoTables
    {
        public static Entity PersonEntity()
        {
            var entity = new Entity
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
                Columns = new List<Column>{
                    new Column{Name = "Id", PropertyName = "Id", IsPrimaryKey = true, IsIdentity = true},
                    new Column{Name = "FirstName", PropertyName = "FirstName", DbType = "varchar", Length = 100},
                    new Column{Name = "LastName", PropertyName = "LastName", DbType = "varchar", Length = 100},
                },
                // ChildRelations = new List<EntityRelation>
                // {
                //     new Relation{},
                // }
                
            };
            

            return entity;
        }

        public static Entity PersonPhoneEntity()
        {
            var entity = new Entity
            {
                Name = "PersonPhone",
                ModelName = "PersonPhone",
                ContextualName = "PersonPhone",
                Type = "table",
                Schema = "dbo",
                Alias = "pp",
                RowCount = 1020,
                UsesIdentityColumn = true,
                Columns = new List<Column>{
                    new Column{Name = "Id", PropertyName = "Id", IsPrimaryKey = true, IsIdentity = true},
                    new Column{Name = "PhoneId", DbType = "int"},
                    new Column{Name = "PersonId", DbType = "int"},
                }
                
            };
            

            return entity;
        }
        public static Entity PhoneEntity()
        {
            var entity = new Entity
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
                Columns = new List<Column>{
                    new Column{Name = "Id", PropertyName = "Id", IsPrimaryKey = true, IsIdentity = true},
                    new Column{Name = "PhoneType", PropertyName = "PhoneType", DbType = "varchar", Length = 100},
                    new Column{Name = "Number", PropertyName = "Number", DbType = "varchar", Length = 100},
                }
            };
            

            return entity;
        }
    }

    
}