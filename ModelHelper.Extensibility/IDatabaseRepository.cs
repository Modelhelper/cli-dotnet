using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ModelHelper.Extensibility
{
    public interface IDatabaseRepository
    {
        string DatabaseType {get;}
        Task<IEnumerable<ITableRelation>> GetParentEntityRelations(string entityName, bool includeColumns = false);
        Task<IEnumerable<ITableRelation>> GetRelatedChildren(string entityName, bool includeColumns = false);
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

    public interface IDatabaseInformation
    {
        string Version {get;}
        string ServerName {get;}
        
    }
    public interface IDatabaseMetaData
    {
        string Database { get; }        
    }

    public interface IDatatypeConverter
    {
        string Convert(string from, string to);
    }

    public interface IDatatypeConverterMetaData
    {
        string From { get; }
        string ToLanguage { get; }
    }


    public interface ISqlGenerator
    {
        string Database { get; }
        SqlMethod Method { get; }
        string Generate(IEntity table, bool includeRelations = false);
    }

    public interface ISqlGeneratorMetaData
    {
        string Database { get; }
        SqlMethod Method { get; }
    }

    public enum SqlMethod
    {
        CreateTable,
        SelectOne,
        SelectMany,
        Insert,
        Update,
        Delete
    }
}