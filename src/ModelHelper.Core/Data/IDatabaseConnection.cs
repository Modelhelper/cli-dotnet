using System.Collections.Generic;
using System.Threading.Tasks;

namespace ModelHelper.Data
{
    public interface IDatabaseConnection
    {
        string DatabaseType { get; }
        Task<IEnumerable<EntityRelation>> GetParentEntityRelations(string entityName, bool includeColumns = false);
        Task<IEnumerable<EntityRelation>> GetRelatedChildren(string entityName, bool includeColumns = false);
        Task<IEnumerable<Column>> GetColumns(string schema, string entityName);

        //[Obsolete]
        //Task<IEnumerable<IEntity>> GetTables(bool includeViews = false, string filter = "");
        Task<IEnumerable<Entity>> GetEntities(bool tablesOnly = false, bool viewsOnly = false, string filter = "", string columnName = "");

        Task<IEnumerable<Index>> GetIndexes(string entityName);

        Task<Entity> GetEntity(string entityName, bool includeChildRelations = false);

        Task<IEnumerable<EntityGroup>> SuggestEntityGroups();
        Task<EntityGroup> SuggestEntityGroup(string schema, string entityName);

        Task<IEnumerable<EntityName>> SuggestEntityGroupName(string entityName);

        IEnumerable<string> ReservedKeywords();

        Task<bool> OptimizeDatabase();

        Task<bool> DumpContentAsJson(string entityName, string path, int limit = -1);
        Task<int> ImportJsonData(Entity entity, string json);

        bool CanReorganizeIndexes { get; }
        bool CanRebuildIndexes { get; }
        bool CanTraverseRelations { get; }

        Task<bool> ReorganizeIndex(string indexName, string entityName);
        Task<bool> RebuildIndex(string indexName, string entityName, double fillFactor = 80);

        Task<IEnumerable<Relation>> TraverseRelations(string baseTable, int depth = 1, int maxLevel = -1);

        Task<IDatabaseInformation> TestConnectionAsync();
    }

    public interface IConnectionFactory
    {
        IDatabaseConnection Create(string connectionName);
    }
}