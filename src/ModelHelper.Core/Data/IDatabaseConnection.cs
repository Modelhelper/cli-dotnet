using System.Collections.Generic;
using System.Threading.Tasks;

namespace ModelHelper.Data
{
    public interface IDatabaseConnection
    {
        string DatabaseType { get; }
        Task<IEnumerable<IEntityRelation>> GetParentEntityRelations(string entityName, bool includeColumns = false);
        Task<IEnumerable<IEntityRelation>> GetRelatedChildren(string entityName, bool includeColumns = false);
        Task<IEnumerable<IColumn>> GetColumns(string schema, string entityName);

        //[Obsolete]
        //Task<IEnumerable<IEntity>> GetTables(bool includeViews = false, string filter = "");
        Task<IEnumerable<IEntity>> GetEntities(bool tablesOnly = false, bool viewsOnly = false, string filter = "", string columnName = "");

        Task<IEnumerable<IIndex>> GetIndexes(string entityName);

        Task<IEntity> GetEntity(string entityName, bool includeChildRelations = false);

        Task<IEnumerable<IEntityGroup>> SuggestEntityGroups();
        Task<IEntityGroup> SuggestEntityGroup(string schema, string entityName);

        Task<IEnumerable<IEntityName>> SuggestEntityGroupName(string entityName);

        IEnumerable<string> ReservedKeywords();

        Task<bool> OptimizeDatabase();

        Task<bool> DumpContentAsJson(string entityName, string path, int limit = -1);
        Task<int> ImportJsonData(IEntity entity, string json);

        bool CanReorganizeIndexes { get; }
        bool CanRebuildIndexes { get; }
        bool CanTraverseRelations { get; }

        Task<bool> ReorganizeIndex(string indexName, string entityName);
        Task<bool> RebuildIndex(string indexName, string entityName, double fillFactor = 80);

        Task<IEnumerable<IRelation>> TraverseRelations(string baseTable, int depth = 1, int maxLevel = -1);

        Task<IDatabaseInformation> TestConnectionAsync();
    }

}