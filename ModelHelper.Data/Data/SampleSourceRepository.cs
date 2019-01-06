using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using ModelHelper.Extensibility;

namespace ModelHelper.Data
{
    [Export(typeof(IDatabaseRepository))]
    [ExportMetadata("Key", "sampledb")]
    public class SampleSourceRepository : IDatabaseRepository
    {
        public string DatabaseType => "sampledb";

        public bool CanReorganizeIndexes => false;

        public bool CanRebuildIndexes => false;

        public bool CanTraverseRelations => true;

        public Task<bool> DumpContentAsJson(string entityName, string path, int limit = -1)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<IColumn>> GetColumns(string schema, string entityName)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<IEntity>> GetEntities(bool tablesOnly = false, bool viewsOnly = false, string filter = "", string columnName = "")
        {
            throw new NotImplementedException();
        }

        public Task<IEntity> GetEntity(string entityName, bool includeChildRelations = false)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<IIndex>> GetIndexes(string entityName)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ITableRelation>> GetParentEntityRelations(string entityName, bool includeColumns = false)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ITableRelation>> GetRelatedChildren(string entityName, bool includeColumns = false)
        {
            throw new NotImplementedException();
        }

        public Task<int> ImportJsonData(IEntity entity, string json)
        {
            throw new NotImplementedException();
        }

        public Task<bool> OptimizeDatabase()
        {
            throw new NotImplementedException();
        }

        public Task<bool> RebuildIndex(string indexName, string entityName, double fillFactor = 80)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ReorganizeIndex(string indexName, string entityName)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> ReservedKeywords()
        {
            throw new NotImplementedException();
        }

        public Task<IEntityGroup> SuggestEntityGroup(string schema, string entityName)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<IEntityName>> SuggestEntityGroupName(string entityName)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<IEntityGroup>> SuggestEntityGroups()
        {
            throw new NotImplementedException();
        }

        public Task<IDatabaseInformation> TestConnectionAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<IRelation>> TraverseRelations(string baseTable, int depth = 1, int maxLevel = -1)
        {
            throw new NotImplementedException();
        }
    }
}