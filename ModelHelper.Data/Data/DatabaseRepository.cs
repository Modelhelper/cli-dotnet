using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ModelHelper.Core;
using ModelHelper.Core.Database;
using ModelHelper.Core.Extensions;
using ModelHelper.Core.Models;
using ModelHelper.Core.Project;
using ModelHelper.Core.Templates;
using Dapper;
using ModelHelper.Extensibility;
using System.IO;

namespace ModelHelper.Data
{
    [Export(typeof(IDatabaseRepository))]
    [ExportMetadata("Key", "mssql")]
    public class SqlServerRepository : IDatabaseRepository
    {
        private readonly string _connectionString;
        private readonly IProject _project;

        public bool CanReorganizeIndexes => true;

        public bool CanRebuildIndexes => true;

        public SqlServerRepository()
        {

        }

        public SqlServerRepository(string connectionString, IProject project)
        {
            _connectionString = connectionString;
            _project = project;
        }
        public async Task<IEnumerable<ITableRelation>> GetParentEntityRelations(string tableName, bool includeColumns = false)
        {
            if (string.IsNullOrEmpty(_connectionString))
            {
                throw new Exception("The connection string cannot be null");
            }


            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = @"        
select 
	p1.name , GroupIndex = row_number() over (partition by p1.name order by p1.create_date desc),
    type = CASE when p1.type = 'U' then 'Table' when p1.type = 'V' then 'View' end,  	
	ParentColumnName = cp.name, 
	ParentColumnType = type_name(cp.user_type_id),
    ChildColumnName = cc.name,
    ChildColumnType = type_name(cc.user_type_id),
	ConstraintName = o1.name
	--, refName = r1.name
	, parentName = p1.name
	, [Schema] = SCHEMA_NAME(p1.schema_id)
, IsSelfJoin = cast(case when fkc.parent_object_id = fkc.referenced_object_id then 1 else 0 end as bit )
from sys.foreign_key_columns fkc

join sys.objects o1 on o1.object_id = fkc.constraint_object_id
join sys.objects r1 on r1.object_id = fkc.parent_object_id
join sys.objects p1 on p1.object_id = fkc.referenced_object_id
join sys.columns cc on fkc.parent_column_id = cc.column_id and cc.object_id = fkc.parent_object_id
join sys.columns cp on fkc.referenced_column_id = cp.column_id and cp.object_id = fkc.referenced_object_id
where fkc.parent_object_id = OBJECT_ID(@tableName)
";

                connection.Open();

                try
                {
                    var relations = await connection.QueryAsync<ModelHelper.Core.Models.TableRelation>(sql, new { tableName });

                    var list = relations.ToList();
                    if (includeColumns)
                    {
                        foreach (var relation in list)
                        {
                            relation.Columns = new List<IColumn>(await GetColumns(relation.Schema, relation.Name));
                        }

                    }

                    connection.Close();
                    return list;
                }
                catch (Exception exception)
                {
                    return new List<ITableRelation>();
                    //throw;
                }




            }
        }

        public async Task<IEnumerable<ITableRelation>> GetRelatedChildren(string tableName, bool includeColumns = false)
        {
            if (string.IsNullOrEmpty(_connectionString))
            {
                throw new Exception("The connection string cannot be null");
            }
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = @"        
select 	GroupIndex = row_number() over (partition by p1.name order by p1.create_date desc),
	p1.name ,
    type = CASE when p1.type = 'U' then 'Table' when p1.type = 'V' then 'View' end,  	
	ParentColumnName = cp.name, 
    ParentColumnType = type_name(cp.user_type_id),
	ChildColumnName = cc.name,
    ChildColumnType = type_name(cc.user_type_id),
	ConstraintName = o1.name
	--, refName = r1.name
	, parentName = p1.name
	, [Schema] = SCHEMA_NAME(p1.schema_id)

    , IsSelfJoin = cast(case when fkc.parent_object_id = fkc.referenced_object_id then 1 else 0 end as bit )
from sys.foreign_key_columns fkc

join sys.objects o1 on o1.object_id = fkc.constraint_object_id
join sys.objects r1 on r1.object_id = fkc.referenced_object_id
join sys.objects p1 on p1.object_id = fkc.parent_object_id
join sys.columns cc on fkc.parent_column_id = cc.column_id and cc.object_id = fkc.parent_object_id
join sys.columns cp on fkc.referenced_column_id = cp.column_id and cp.object_id = fkc.referenced_object_id
where fkc.referenced_object_id = OBJECT_ID(@tableName)
";

                connection.Open();


                var relations = await connection.QueryAsync<ModelHelper.Core.Models.TableRelation>(sql, new { tableName });

                var list = relations.ToList();
                if (includeColumns)
                {
                    foreach (var relation in list)
                    {
                        relation.Columns = new List<IColumn>(await GetColumns(relation.Schema, relation.Name));
                    }

                }


                connection.Close();
                return list;
            }
        }

        public async Task<IEnumerable<IColumn>> GetColumns(string schema, string tableName)
        {
            if (string.IsNullOrEmpty(_connectionString))
            {
                throw new Exception("The connection string cannot be null");
            }


            //var translation = GetUnionList("select Name = '{0}', Translation = '{1}'", _translations);
            //var nameMap = GetUnionList("select Name = '{0}', Aka = '{1}'", _nameMap);
            // var ignoreColumns = GetUnionList("select Name = '{0}'", _project.Database.IgnoredColumns);

            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = $@"
        with Reserved as (
            select Name = 'database' union
            select Name = 'version' union
            select Name = 'new' union
            select Name = 'tran' union
            select Name = 'add' union
            select Name = 'insert' union
            select Name = 'inner' union
            select Name = 'index' union
            select Name = 'column' union
            select Name = 'commit' union
            select Name = 'return'        
        ),PrimaryKeyColumns as (

            SELECT  
				i.name AS IndexName
				, OBJECT_NAME(ic.OBJECT_ID) AS TableName
				, COL_NAME(ic.OBJECT_ID,ic.column_id) AS PrimaryColumnName
				, ColumnId = ic.column_id
				, ObjectId = ic.object_id
            FROM    sys.indexes AS i 
            INNER JOIN sys.index_columns AS ic ON  i.OBJECT_ID = ic.OBJECT_ID AND i.index_id = ic.index_id
            WHERE i.is_primary_key = 1 and i.object_id = object_id(@table)
        ), ForeignKeyColumns as (
            select 
                  ColumnName = cc.name
				, ColumnId = cc.column_id
				, ObjectId = cc.object_id
                , ReferencedColumn = pcc.name  
                , ReferencedObjectId = pcc.object_id
                , IsSelfJoin = cast(case when fkc.parent_object_id = fkc.referenced_object_id then 1 else 0 end as bit )
            from sys.foreign_key_columns fkc
            join sys.columns cc on fkc.parent_column_id = cc.column_id and cc.object_id = fkc.parent_object_id
           join sys.columns pcc on fkc.referenced_column_id = pcc.column_id and pcc.object_id = fkc.referenced_object_id
            where fkc.parent_object_id = OBJECT_ID(@table)
        )
        select
	          Name = c.name	        
            , ModelName = c.Name
	        , DataType = TYPE_NAME(c.user_type_id)
	        , DbType = TYPE_NAME(c.user_type_id)
	        , IsNullable = c.is_nullable	        
            , IsIdentity = c.is_identity             
            , IsPrimaryKey = cast (case when pkc.PrimaryColumnName is null then 0 else 1 end as bit)
            , IsForeignKey = cast (case when fkc.ColumnName is null then 0 else 1 end as bit)
            --, IsIgnored = case when s.name is null then 0 else 1 end
            , IsReserved = cast (case when r.name is null then 0 else 1 end as bit)
			, Selected = cast (1 as bit) --case when s.name is null then 1 else 0 end
            , [Collation] = c.collation_name
            , Length = case 
                when c.user_type_id = 231 and c.max_length > 0 then c.max_length / 2
                when left(c.name, 1) = 'n' and st.max_length = 8000 then c.max_length / 2
                else c.max_length end
            , UseLength = cast(case when st.precision = 0 and c.collation_name is not null then 1 else 0 end as bit)
            , c.Precision
            , c.Scale
            , UsePrecision = cast(case when st.user_type_id in (108,106) then 1 else 0 end as bit)
            , ReferencesColumn = fkc.ReferencedColumn
            , ReferencesTable = object_name(fkc.ReferencedObjectId)
            , Description = isnull(ep.value, '')
        from sys.columns c         
        left join sys.types st on st.user_type_id = c.user_type_id
       -- left join IgnoredColumns s on s.Name = c.name
        left join Reserved r on r.name = c.name
        left join PrimaryKeyColumns pkc on pkc.ColumnId = c.column_id and pkc.ObjectId = c.object_id -- c.name
        left join ForeignKeyColumns fkc on fkc.ColumnId = c.column_id and c.object_id = fkc.ObjectId
        left join sys.extended_properties ep on c.object_id = ep.major_id and minor_id = c.column_id and ep.name = 'MS_description'
        where object_id = object_id(@table)
        order by c.column_id";

                connection.Open();

                var table = $"[{schema}].[{tableName}]";
                var items = await connection.QueryAsync<Column>(sql, new { table });

                var list = new List<string>
                {
                    "title",
                    "name",
                    "subject"
                };

                var reserved = ReservedKeywords().ToList();
                //TODO need the name map here
                foreach (var column in items)
                {
                    // TODO fix
                    var propertyName = column.Name.NameMapValue(_project.DataSource.ColumnMapping).CleanInput()
                        .AsUpperCamelCase();

                    column.UseInViewModel = list.Any(f => propertyName.ToLowerInvariant().EndsWith(f));


                    var columnExtra = _project.DataSource.ColumnMapping.FirstOrDefault(ce =>
                        ce.Name.Equals(column.Name, StringComparison.InvariantCultureIgnoreCase));

                    if (columnExtra != null)
                    {
                        column.IsIgnored = columnExtra.IsIgnored;
                        column.IsDeletedMarker = columnExtra.IsDeletedMarker;
                        column.IsCreatedByUser = columnExtra.IsCreatedByUser;
                        column.IsCreatedDate = columnExtra.IsCreatedDate;
                        column.IsModifiedByUser = columnExtra.IsModifiedByUser;
                        column.IsModifiedDate = columnExtra.IsModifiedDate;

                        column.UseInViewModel = column.UseInViewModel || columnExtra.IncludeInViewModel;

                    }



                    column.IsReserved = reserved.Contains(column.Name.ToLowerInvariant());

                    column.PropertyName = propertyName;

                    if (column.UseLength)
                    {
                        var len = column.Length == -1 ? "max" : column.Length.ToString();
                        column.DbType = $"{column.DataType} ({len})";
                    }

                    if (column.UsePrecision)
                    {
                        column.DbType = $"{column.DataType} ({column.Precision},{column.Scale})";
                    }

                }
                connection.Close();
                return items;
            }
        }

        public async Task<IEntity> GetEntity(string entityName, bool includeRelations = false)
        {
            if (string.IsNullOrEmpty(_connectionString))
            {
                throw new Exception("The connection string cannot be null");
            }
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = @"        
                    select 
	o.name
	,type = CASE when o.type = 'U' then 'Table' when o.type = 'V' then 'View' end  
	,[Schema] = s.name
    , description =  isnull(ep.value, '')
from sys.objects o
join sys.schemas s on s.schema_id = o.schema_id
left join sys.extended_properties ep on o.object_id = ep.major_id and minor_id = 0 and ep.name = 'MS_description'
where o.object_id = object_id(@entityName)";

                connection.Open();

                try
                {
                    var entity = await connection.QueryFirstOrDefaultAsync<Table>(sql, new { entityName });

                    if (entity == null)
                    {
                        //throw new Exception($"{entityName} does not exist for this data source '{connection.Database}'");
                        return null;
                    }
                    var markAsDeletedColumns = new List<string>
            {
                "deleted".ToLowerInvariant(), "delete".ToLowerInvariant(), "removed".ToLowerInvariant(), "notActive".ToLowerInvariant()
            };

                    entity.Columns = new List<IColumn>(await GetColumns(entity.Schema, entity.Name));
                    entity.Alias = entity.Name.Abrevation(ReservedKeywords());
                    entity.UsesIdentityColumn = entity.Columns.Any(c => c.IsIdentity);
                    entity.UsesGuidAsPrimaryKey = entity.Columns.Any(c => c.IsPrimaryKey && c.DataType.ToLowerInvariant() == "uniqueidentifier");

                    var deleteColumn = entity.Columns.FirstOrDefault(c => markAsDeletedColumns.Contains(c.Name.ToLowerInvariant()));

                    entity.UsesDeletedColumn = deleteColumn != null;
                    entity.DeletedColumnName = deleteColumn != null ? deleteColumn.Name : "";

                    entity.Indexes = new List<IIndex>(await GetIndexes(entityName));

                    if (includeRelations)
                    {
                        // get relations
                        entity.ChildRelations = new List<ITableRelation>(await GetRelatedChildren(entityName, true));
                        entity.ParentRelations = new List<ITableRelation>(await GetParentEntityRelations(entityName, true));

                        //remove duplicates
                        var childIndex = 1;
                        foreach (var child in entity.ChildRelations)
                        {
                            var modelName = child.Name;
                            if (child.Name.ToLower().Contains(entity.Name.ToLower()))
                            {
                                modelName = child.Name.Replace(entity.Name, ""); // Remove(0,entity.Name.Length);
                            }

                            child.ModelName = string.IsNullOrEmpty(modelName)
                                ? child.Name.AsUpperCamelCase()
                                : modelName.AsUpperCamelCase();
                            child.Alias = $"{child.Name.Abrevation()}{childIndex++}"; //
                        }

                        var parentAliasIndex = 1;
                        foreach (var parent in entity.ParentRelations)
                        {
                            var modelName = parent.Name;
                            if (parent.Name.ToLower().Contains(entity.Name.ToLower()))
                            {
                                modelName = parent.Name.Replace(entity.Name, ""); //.Remove(0, entity.Name.Length);
                            }

                            parent.ModelName = string.IsNullOrEmpty(modelName) ? parent.Name.AsUpperCamelCase() : modelName.AsUpperCamelCase();
                            parent.Alias = $"{parent.Name.Abrevation()}{parentAliasIndex++}";// parent.Name.Abrevation();
                        }
                    }

                    connection.Close();
                    return entity;
                }
                catch (Exception exception)
                {
                    throw new Exception("Could not load table " + entityName, exception);
                    
                }

            }
        }

        [Obsolete("Will be removed in version 3, Use GetEntities instead")]
        public async Task<IEnumerable<IEntity>> GetTables(bool includeViews = false, string filter = "")
        {
            if (string.IsNullOrEmpty(_connectionString))
            {
                throw new Exception("The connection string cannot be null");
            }
            using (var connection = new SqlConnection(_connectionString))
            {
                var entityFilter = includeViews ? "('U', 'V')" : "('U')";
                var tableFilter = !string.IsNullOrEmpty(filter) ? "AND o.name like @filter" : "";

                var sql = $@"        
                    with rowcnt (object_id, rowcnt) as (
SELECT p.object_id, SUM(CASE WHEN (p.index_id < 2) AND (a.type = 1) THEN p.rows ELSE 0 END) 
FROM sys.partitions p 
INNER JOIN sys.allocation_units a ON p.partition_id = a.container_id
join sys.objects o on p.object_id = o.object_id and o.type = 'U'
--where p.object_id = object_id('Add')
group by p.object_id
)
select 
	o.name
	,type = CASE when o.type = 'U' then 'Table' when o.type = 'V' then 'View' end  
	,[Schema] = s.name
    , Alias = Left(o.name, 1)
	, [RowCount] = isnull(rc.RowCnt, 0)
    , Description = isnull(ep.value, '')
from sys.objects o
join sys.schemas s on s.schema_id = o.schema_id
left join rowcnt rc on rc.object_id = o.object_id    
left join sys.extended_properties ep on o.object_id = ep.major_id and minor_id = 0 and ep.name = 'MS_description'
where o.name not in ('sysdiagrams') 
    and type in {entityFilter}
    {tableFilter}
order by s.name, o.[type], o.name";

                connection.Open();
                var entityItems = await connection.QueryAsync<Table>(sql, new { filter });
                var items = entityItems.ToList();

                foreach (var table in items)
                {
                    table.ModelName = table.Name.AsUpperCamelCase();
                    table.Alias = table.Name.Abrevation();
                }
                connection.Close();

                //if (!string.IsNullOrEmpty(filter))
                //{
                //    var regex = new Regex(filter);
                //    var filtered = items.Where(t => regex.IsMatch(t.Name));
                //    return filtered;
                //}

                return items;
            }
        }

        public string GetUnionList(string format, List<string> list)
        {
            var sb = new StringBuilder();
            var counter = 0;
            var union = "union ";

            if (list == null || (list != null && !list.Any()))
            {
                list = new List<string> { "HUMPYBUMPYDUMP" };
            }

            foreach (var value in list)
            {
                counter++;

                if (counter == list.Count)
                {
                    union = "";
                }
                sb.AppendLine(string.Format(format + " " + union, value));
            }

            return sb.ToString().Trim();
        }

        public async Task<IEnumerable<IEntityGroup>> SuggestEntityGroups()
        {
            var groups = new List<IEntityGroup>();
            var tables = await GetTables();

            foreach (var item in tables.Where(t => t.ChildRelations.Any()))
            {
                var group = await SuggestEntityGroup(item.Schema, item.Name);
                if (group != null)
                {
                    groups.Add(group);
                }
            }
            return groups;
        }

        public async Task<IEntityGroup> SuggestEntityGroup(string schema, string tableName)
        {
            var entityGroup = new EntityGroup();

            entityGroup.Name = tableName;
            var entities = await SuggestEntityGroupName($"{schema}.{tableName}");
            entityGroup.Entities = entities.ToList();

            return entityGroup;
        }

        public async Task<IEnumerable<IEntityName>> SuggestEntityGroupName(string tableName)
        {
            if (string.IsNullOrEmpty(_connectionString))
            {
                throw new Exception("The connection string cannot be null");
            }
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = @"        
 select [Schema] = SCHEMA_NAME(t.schema_id), Name = t.name, [Type] = t.[type] from sys.tables t where object_id = object_id(@table)
union
select SCHEMA_NAME(t.schema_id), t.name, t.[type] from sys.tables t where object_id in (
    select fkc.parent_object_id from sys.foreign_key_columns fkc where fkc.referenced_object_id = OBJECT_ID(@table)
)                   

";

                //connection.Open();
                var entityItems = await connection.QueryAsync<EntityName>(sql, new { table = tableName });
                //connection.Close();
                return entityItems.ToList();

                //
            }

        }

        public IEnumerable<string> ReservedKeywords()
        {
            return new List<string>
            {
                "a",
                "abort",
                "abs",
                "absolute",
                "access",
                "action",
                "ada",
                "add",
                "admin",
                "after",
                "aggregate",
                "alias",
                "all",
                "allocate",
                "also",
                "alter",
                "always",
                "analyse",
                "analyze",
                "and",
                "any",
                "are",
                "array",
                "as",
                "asc",
                "asensitive",
                "assertion",
                "assignment",
                "asymmetric",
                "at",
                "atomic",
                "attribute",
                "attributes",
                "audit",
                "authorization",
                "auto_increment",
                "avg",
                "avg_row_length",
                "backup",
                "backward",
                "before",
                "begin",
                "bernoulli",
                "between",
                "bigint",
                "binary",
                "bit",
                "bit_length",
                "bitvar",
                "blob",
                "bool",
                "boolean",
                "both",
                "breadth",
                "break",
                "browse",
                "bulk",
                "by",
                "c",
                "cache",
                "call",
                "called",
                "cardinality",
                "cascade",
                "cascaded",
                "case",
                "cast",
                "catalog",
                "catalog_name",
                "ceil",
                "ceiling",
                "chain",
                "change",
                "char",
                "char_length",
                "character",
                "character_length",
                "character_set_catalog",
                "character_set_name",
                "character_set_schema",
                "characteristics",
                "characters",
                "check",
                "checked",
                "checkpoint",
                "checksum",
                "class",
                "class_origin",
                "clob",
                "close",
                "cluster",
                "clustered",
                "coalesce",
                "cobol",
                "collate",
                "collation",
                "collation_catalog",
                "collation_name",
                "collation_schema",
                "collect",
                "column",
                "column_name",
                "columns",
                "command_function",
                "command_function_code",
                "comment",
                "commit",
                "committed",
                "completion",
                "compress",
                "compute",
                "condition",
                "condition_number",
                "connect",
                "connection",
                "connection_name",
                "constraint",
                "constraint_catalog",
                "constraint_name",
                "constraint_schema",
                "constraints",
                "constructor",
                "contains",
                "containstable",
                "continue",
                "conversion",
                "convert",
                "copy",
                "corr",
                "corresponding",
                "count",
                "covar_pop",
                "covar_samp",
                "create",
                "createdb",
                "createrole",
                "createuser",
                "cross",
                "csv",
                "cube",
                "cume_dist",
                "current",
                "current_date",
                "current_default_transform_group",
                "current_path",
                "current_role",
                "current_time",
                "current_timestamp",
                "current_transform_group_for_type",
                "current_user",
                "cursor",
                "cursor_name",
                "cycle",
                "data",
                "database",
                "databases",
                "date",
                "datetime",
                "datetime_interval_code",
                "datetime_interval_precision",
                "day",
                "day_hour",
                "day_microsecond",
                "day_minute",
                "day_second",
                "dayofmonth",
                "dayofweek",
                "dayofyear",
                "dbcc",
                "deallocate",
                "dec",
                "decimal",
                "declare",
                "default",
                "defaults",
                "deferrable",
                "deferred",
                "defined",
                "definer",
                "degree",
                "delay_key_write",
                "delayed",
                "delete",
                "delimiter",
                "delimiters",
                "dense_rank",
                "deny",
                "depth",
                "deref",
                "derived",
                "desc",
                "describe",
                "descriptor",
                "destroy",
                "destructor",
                "deterministic",
                "diagnostics",
                "dictionary",
                "disable",
                "disconnect",
                "disk",
                "dispatch",
                "distinct",
                "distinctrow",
                "distributed",
                "div",
                "do",
                "domain",
                "double",
                "drop",
                "dual",
                "dummy",
                "dump",
                "dynamic",
                "dynamic_function",
                "dynamic_function_code",
                "each",
                "element",
                "else",
                "elseif",
                "enable",
                "enclosed",
                "encoding",
                "encrypted",
                "end",
                "end-exec",
                "enum",
                "equals",
                "errlvl",
                "escape",
                "escaped",
                "every",
                "except",
                "exception",
                "exclude",
                "excluding",
                "exclusive",
                "exec",
                "execute",
                "existing",
                "exists",
                "exit",
                "exp",
                "explain",
                "external",
                "extract",
                "false",
                "fetch",
                "fields",
                "file",
                "fillfactor",
                "filter",
                "final",
                "first",
                "float",
                "float4",
                "float8",
                "floor",
                "flush",
                "following",
                "for",
                "force",
                "foreign",
                "fortran",
                "forward",
                "found",
                "free",
                "freetext",
                "freetexttable",
                "freeze",
                "from",
                "full",
                "fulltext",
                "function",
                "fusion",
                "g",
                "general",
                "generated",
                "get",
                "global",
                "go",
                "goto",
                "grant",
                "granted",
                "grants",
                "greatest",
                "group",
                "grouping",
                "handler",
                "having",
                "header",
                "heap",
                "hierarchy",
                "high_priority",
                "hold",
                "holdlock",
                "host",
                "hosts",
                "hour",
                "hour_microsecond",
                "hour_minute",
                "hour_second",
                "identified",
                "identity",
                "identity_insert",
                "identitycol",
                "if",
                "ignore",
                "ilike",
                "immediate",
                "immutable",
                "implementation",
                "implicit",
                "in",
                "include",
                "including",
                "increment",
                "index",
                "indicator",
                "infile",
                "infix",
                "inherit",
                "inherits",
                "initial",
                "initialize",
                "initially",
                "inner",
                "inout",
                "input",
                "insensitive",
                "insert",
                "insert_id",
                "instance",
                "instantiable",
                "instead",
                "int",
                "int1",
                "int2",
                "int3",
                "int4",
                "int8",
                "integer",
                "intersect",
                "intersection",
                "interval",
                "into",
                "invoker",
                "is",
                "isam",
                "isnull",
                "isolation",
                "iterate",
                "join",
                "k",
                "key",
                "key_member",
                "key_type",
                "keys",
                "kill",
                "lancompiler",
                "language",
                "large",
                "last",
                "last_insert_id",
                "lateral",
                "leading",
                "least",
                "leave",
                "left",
                "length",
                "less",
                "level",
                "like",
                "limit",
                "lineno",
                "lines",
                "listen",
                "ln",
                "load",
                "local",
                "localtime",
                "localtimestamp",
                "location",
                "locator",
                "lock",
                "login",
                "logs",
                "long",
                "longblob",
                "longtext",
                "loop",
                "low_priority",
                "lower",
                "m",
                "map",
                "match",
                "matched",
                "max",
                "max_rows",
                "maxextents",
                "maxvalue",
                "mediumblob",
                "mediumint",
                "mediumtext",
                "member",
                "merge",
                "message_length",
                "message_octet_length",
                "message_text",
                "method",
                "middleint",
                "min",
                "min_rows",
                "minus",
                "minute",
                "minute_microsecond",
                "minute_second",
                "minvalue",
                "mlslabel",
                "mod",
                "mode",
                "modifies",
                "modify",
                "module",
                "month",
                "monthname",
                "more",
                "move",
                "multiset",
                "mumps",
                "myisam",
                "name",
                "names",
                "national",
                "natural",
                "nchar",
                "nclob",
                "nesting",
                "new",
                "next",
                "no",
                "no_write_to_binlog",
                "noaudit",
                "nocheck",
                "nocompress",
                "nocreatedb",
                "nocreaterole",
                "nocreateuser",
                "noinherit",
                "nologin",
                "nonclustered",
                "none",
                "normalize",
                "normalized",
                "nosuperuser",
                "not",
                "nothing",
                "notify",
                "notnull",
                "nowait",
                "null",
                "nullable",
                "nullif",
                "nulls",
                "number",
                "numeric",
                "object",
                "octet_length",
                "octets",
                "of",
                "off",
                "offline",
                "offset",
                "offsets",
                "oids",
                "old",
                "on",
                "online",
                "only",
                "open",
                "opendatasource",
                "openquery",
                "openrowset",
                "openxml",
                "operation",
                "operator",
                "optimize",
                "option",
                "optionally",
                "options",
                "or",
                "order",
                "ordering",
                "ordinality",
                "others",
                "out",
                "outer",
                "outfile",
                "output",
                "over",
                "overlaps",
                "overlay",
                "overriding",
                "owner",
                "pack_keys",
                "pad",
                "parameter",
                "parameter_mode",
                "parameter_name",
                "parameter_ordinal_position",
                "parameter_specific_catalog",
                "parameter_specific_name",
                "parameter_specific_schema",
                "parameters",
                "partial",
                "partition",
                "pascal",
                "password",
                "path",
                "pctfree",
                "percent",
                "percent_rank",
                "percentile_cont",
                "percentile_disc",
                "placing",
                "plan",
                "pli",
                "position",
                "postfix",
                "power",
                "preceding",
                "precision",
                "prefix",
                "preorder",
                "prepare",
                "prepared",
                "preserve",
                "primary",
                "print",
                "prior",
                "privileges",
                "proc",
                "procedural",
                "procedure",
                "process",
                "processlist",
                "public",
                "purge",
                "quote",
                "raid0",
                "raiserror",
                "range",
                "rank",
                "raw",
                "read",
                "reads",
                "readtext",
                "real",
                "recheck",
                "reconfigure",
                "recursive",
                "ref",
                "references",
                "referencing",
                "regexp",
                "regr_avgx",
                "regr_avgy",
                "regr_count",
                "regr_intercept",
                "regr_r2",
                "regr_slope",
                "regr_sxx",
                "regr_sxy",
                "regr_syy",
                "reindex",
                "relative",
                "release",
                "reload",
                "rename",
                "repeat",
                "repeatable",
                "replace",
                "replication",
                "require",
                "reset",
                "resignal",
                "resource",
                "restart",
                "restore",
                "restrict",
                "result",
                "return",
                "returned_cardinality",
                "returned_length",
                "returned_octet_length",
                "returned_sqlstate",
                "returns",
                "revoke",
                "right",
                "rlike",
                "role",
                "rollback",
                "rollup",
                "routine",
                "routine_catalog",
                "routine_name",
                "routine_schema",
                "row",
                "row_count",
                "row_number",
                "rowcount",
                "rowguidcol",
                "rowid",
                "rownum",
                "rows",
                "rule",
                "save",
                "savepoint",
                "scale",
                "schema",
                "schema_name",
                "schemas",
                "scope",
                "scope_catalog",
                "scope_name",
                "scope_schema",
                "scroll",
                "search",
                "second",
                "second_microsecond",
                "section",
                "security",
                "select",
                "self",
                "sensitive",
                "separator",
                "sequence",
                "serializable",
                "server_name",
                "session",
                "session_user",
                "set",
                "setof",
                "sets",
                "setuser",
                "share",
                "show",
                "shutdown",
                "signal",
                "similar",
                "simple",
                "size",
                "smallint",
                "some",
                "soname",
                "source",
                "space",
                "spatial",
                "specific",
                "specific_name",
                "specifictype",
                "sql",
                "sql_big_result",
                "sql_big_selects",
                "sql_big_tables",
                "sql_calc_found_rows",
                "sql_log_off",
                "sql_log_update",
                "sql_low_priority_updates",
                "sql_select_limit",
                "sql_small_result",
                "sql_warnings",
                "sqlca",
                "sqlcode",
                "sqlerror",
                "sqlexception",
                "sqlstate",
                "sqlwarning",
                "sqrt",
                "ssl",
                "stable",
                "start",
                "starting",
                "state",
                "statement",
                "static",
                "statistics",
                "status",
                "stddev_pop",
                "stddev_samp",
                "stdin",
                "stdout",
                "storage",
                "straight_join",
                "strict",
                "string",
                "structure",
                "style",
                "subclass_origin",
                "sublist",
                "submultiset",
                "substring",
                "successful",
                "sum",
                "superuser",
                "symmetric",
                "synonym",
                "sysdate",
                "sysid",
                "system",
                "system_user",
                "table",
                "table_name",
                "tables",
                "tablesample",
                "tablespace",
                "temp",
                "template",
                "temporary",
                "terminate",
                "terminated",
                "text",
                "textsize",
                "than",
                "then",
                "ties",
                "time",
                "timestamp",
                "timezone_hour",
                "timezone_minute",
                "tinyblob",
                "tinyint",
                "tinytext",
                "to",
                "toast",
                "top",
                "top_level_count",
                "trailing",
                "tran",
                "transaction",
                "transaction_active",
                "transactions_committed",
                "transactions_rolled_back",
                "transform",
                "transforms",
                "translate",
                "translation",
                "treat",
                "trigger",
                "trigger_catalog",
                "trigger_name",
                "trigger_schema",
                "trim",
                "true",
                "truncate",
                "trusted",
                "tsequal",
                "type",
                "uescape",
                "uid",
                "unbounded",
                "uncommitted",
                "under",
                "undo",
                "unencrypted",
                "union",
                "unique",
                "unknown",
                "unlisten",
                "unlock",
                "unnamed",
                "unnest",
                "unsigned",
                "until",
                "update",
                "updatetext",
                "upper",
                "usage",
                "use",
                "user",
                "user_defined_type_catalog",
                "user_defined_type_code",
                "user_defined_type_name",
                "user_defined_type_schema",
                "using",
                "utc_date",
                "utc_time",
                "utc_timestamp",
                "vacuum",
                "valid",
                "validate",
                "validator",
                "value",
                "values",
                "var_pop",
                "var_samp",
                "varbinary",
                "varchar",
                "varchar2",
                "varcharacter",
                "variable",
                "variables",
                "varying",
                "verbose",
                "view",
                "volatile",
                "waitfor",
                "when",
                "whenever",
                "where",
                "while",
                "width_bucket",
                "window",
                "with",
                "within",
                "without",
                "work",
                "write",
                "writetext",
                "x509",
                "xor",
                "year",
                "year_month",
                "zerofill",
                "zone"
            };
        }

        public async Task<IEnumerable<IEntity>> GetEntities(bool tablesOnly = false, bool viewsOnly = false, string filter = "", string columnName = "")
        {
            if (string.IsNullOrEmpty(_connectionString))
            {
                throw new Exception("The connection string cannot be null");
            }
            using (var connection = new SqlConnection(_connectionString))
            {
                var entityFilter = tablesOnly ? "('U')" : viewsOnly ? "('V')" : "('U', 'V')";
                var tableFilter = !string.IsNullOrEmpty(filter) ? "AND o.name like @filter" : "";
                var columnFilter = !string.IsNullOrEmpty(columnName) ? "AND o.object_id in (select object_id from sys.columns where name like @columnName)": "";
                var sql = $@"        
                    with rowcnt (object_id, rowcnt) as (
SELECT p.object_id, SUM(CASE WHEN (p.index_id < 2) AND (a.type = 1) THEN p.rows ELSE 0 END) 
FROM sys.partitions p 
INNER JOIN sys.allocation_units a ON p.partition_id = a.container_id
join sys.objects o on p.object_id = o.object_id and o.type = 'U'
--where p.object_id = object_id('Add')
group by p.object_id
)
select 
	o.name
	,type = CASE when o.type = 'U' then 'Table' when o.type = 'V' then 'View' end  
	,[Schema] = s.name
    , Alias = Left(o.name, 1)
	, [RowCount] = isnull(rc.RowCnt, 0)
    , Description = isnull(ep.value, '')
from sys.objects o
join sys.schemas s on s.schema_id = o.schema_id
left join rowcnt rc on rc.object_id = o.object_id    
left join sys.extended_properties ep on o.object_id = ep.major_id and minor_id = 0 and ep.name = 'MS_description'
where o.name not in ('sysdiagrams') 
    and type in {entityFilter}
    {tableFilter}
    {columnFilter}
order by s.name, o.[type], o.name";

                connection.Open();
                var entityItems = await connection.QueryAsync<Table>(sql, new { filter, columnName });
                var items = entityItems.ToList();

                foreach (var table in items)
                {
                    table.ModelName = table.Name.AsUpperCamelCase();
                    table.Alias = table.Name.Abrevation();
                }
                connection.Close();

                //if (!string.IsNullOrEmpty(filter))
                //{
                //    var regex = new Regex(filter);
                //    var filtered = items.Where(t => regex.IsMatch(t.Name));
                //    return filtered;
                //}

                return items;
            }
        }

        public async Task<bool> DumpContentAsJson(string entityName, string path, int limit = -1)
        {

            if (string.IsNullOrEmpty(_connectionString))
            {
                throw new Exception("The connection string cannot be null");
            }
            using (var connection = new SqlConnection(_connectionString))
            {
                // var entityFilter = tablesOnly ? "('U')" : viewsOnly ? "('V')" : "('U', 'V')";
                // var tableFilter = !string.IsNullOrEmpty(filter) ? "AND o.name like @filter" : "";

                var sql = $@"select * from {entityName} for json auto, INCLUDE_NULL_VALUES";

                connection.Open();
                var entityItems = await connection.QueryAsync<string>(sql);
                var items = entityItems.ToList();
                
                var sb = new StringBuilder();

                foreach (var i in items)
                {
                    sb.Append(i);
                }

                var file = Path.Combine(path, entityName + "-dump.json");
                System.IO.File.WriteAllText(file, sb.ToString());

                
                connection.Close();
               
                return true;
            }

        }

        public async Task<int> ImportJsonData(IEntity entity, string json)
        {
            var result = -1;
            
            if (string.IsNullOrEmpty(_connectionString))
            {
                throw new Exception("The connection string cannot be null");
            }
            if (!string.IsNullOrEmpty(json) && entity != null)
            {
                var columnSql = new StringBuilder();
                var insertSql = new StringBuilder();
                var index = 0;
                foreach (var column in entity.Columns)
                {
                    var comma = index++ == 0 ? "  " : ", ";
                    columnSql.AppendLine($"{comma}[{column.Name}]\t{column.DbType}\t'$.{column.Name}'");
                    insertSql.Append($"{comma}[{column.Name}]");
                }

                

                using (var connection = new SqlConnection(_connectionString))
                {


                    // var entityFilter = tablesOnly ? "('U')" : viewsOnly ? "('V')" : "('U', 'V')";
                    // var tableFilter = !string.IsNullOrEmpty(filter) ? "AND o.name like @filter" : "";

                    var sql = $@"
insert into {entity.Schema}.{entity.Name} ({insertSql.ToString()})
SELECT {insertSql.ToString()}
FROM OPENJSON(@json)
With (
    {columnSql.ToString()}
);
";

                    //Console.WriteLine(sql);m
                    connection.Open();

                    try
                    {
                        var count = await connection.ExecuteAsync(sql, new { json });
                        return count;
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                    finally
                    {
                        connection.Close();
                    }
                    
                }




            }


            return result;
        }
        public async Task<IEnumerable<IIndex>> GetIndexes(string entityName)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                
                // var entityFilter = tablesOnly ? "('U')" : viewsOnly ? "('V')" : "('U', 'V')";
                // var tableFilter = !string.IsNullOrEmpty(filter) ? "AND o.name like @filter" : "";

                var sql = $@"
SELECT 
    a.index_id as 'id'
    , b.name
    , isnull(avg_fragmentation_in_percent, 0) as 'avgFragmentationPercent'
    , b.is_unique as 'isUnique', is_primary_key as 'IsPrimaryKey'
    , isnull(a.avg_page_space_used_in_percent, 0) as 'AvgPageSpacePercent'
    , isnull(a.avg_record_size_in_bytes, 0) as 'AvgRecordSize'
    , isnull(a.record_count, 0) as 'Rows'
    --, ic.*
    , case when avg_fragmentation_in_percent between 10 and 30 then 'alter index ' + b.name + ' on ' + @table + ' reorganize'
        when avg_fragmentation_in_percent > 30 then N'alter index ' + b.name + ' on ' + @table + ' reorganize' + NCHAR(10) + '
        alter index ' + b.name + ' on ' + @table + ' rebuild WITH (FILLFACTOR = 80, SORT_IN_TEMPDB = ON, STATISTICS_NORECOMPUTE = ON)' 
        else 'all good :-)'  
        end as MaintScript
        ,(
            select 
                c.name
                , type_name(c.user_type_id) as 'type'
                , ic.is_descending_key as 'IsDescending'
                , ic.partition_ordinal as 'PartitionOrdinal'
                , c.is_nullable as 'IsNullable' 
                , c.is_identity as 'IsIdentity'            
            from sys.index_columns ic 
            join sys.columns c on ic.column_id = c.column_id and c.object_id = ic.object_id
            where ic.index_id = b.index_id and ic.object_id = b.object_id   for json path
        ) as Columns
FROM sys.dm_db_index_physical_stats (DB_ID(@database), OBJECT_ID(@table), NULL, NULL, NULL) AS a  
JOIN sys.indexes AS b ON a.object_id = b.object_id AND a.index_id = b.index_id

--left join sys.columns c on ic.column_id = c.column_id and c.object_id = ic.object_id
for json path, INCLUDE_NULL_VALUES
;

";

                connection.Open();

                try
                {
                    var entityItems = await connection.QueryAsync<string>(sql, new { connection.Database, Table = entityName });
                    var items = entityItems.ToList();

                    var sb = new StringBuilder();

                    foreach (var i in items)
                    {
                        sb.Append(i);
                    }

                    var input = sb.ToString();
                    var json = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Index>>(sb.ToString());

                    
                    return json;
                }
                catch (Exception)
                {

                    return new List<Index>();
                }
                finally
                {
                    connection.Close();
                }
                
            }
        }

        public async Task<bool> OptimizeDatabase()
        {
            using (var connection = new SqlConnection(_connectionString))
            {

                // var entityFilter = tablesOnly ? "('U')" : viewsOnly ? "('V')" : "('U', 'V')";
                // var tableFilter = !string.IsNullOrEmpty(filter) ? "AND o.name like @filter" : "";

                var sql = $@"

SELECT  RowId = ROW_NUMBER() over (order by s.object_id)
		-- , s.object_id AS objectid ,
        -- , s.index_id AS indexid ,
        -- , s.partition_number AS partitionnum ,
        , s.avg_fragmentation_in_percent AS frag
        -- s.page_count AS page_count,
		, OBJECT_NAME(s.object_id) as table_name
		, i.name as index_name

into #t_maintenance

FROM    sys.dm_db_index_physical_stats(DB_ID(@database), NULL, NULL, NULL, null) s
join	sys.indexes i on i.index_id = s.index_id and i.object_id = s.object_id
WHERE   avg_fragmentation_in_percent > 9
        AND s.index_id > 0
        AND s.page_count > 0; 

declare @rows int = @@ROWCOUNT;
declare @currentRow int = 1;

while @currentRow <= @rows
begin
	declare 
		@indexName nvarchar(200),		
		@table nvarchar(200),
		@fragmentation float;
		
	select @indexName = index_name, @fragmentation = frag, @table = table_name
	from #t_maintenance where RowId = @currentRow
	
	exec(N'alter index [' + @indexName + '] on ' + @table + ' reorganize');
			
	if @fragmentation > 30
	begin	
		exec('alter index [' + @indexName + '] on ' + @table + ' rebuild WITH (SORT_IN_TEMPDB = ON, STATISTICS_NORECOMPUTE = ON)');			
	end
	
	set @currentRow += 1;
end

drop table #t_maintenance  
";

                connection.Open();

                try
                {
                    var result = await connection.ExecuteAsync(sql, new { connection.Database});

                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
                finally
                {
                    connection.Close();
                }

            }
        }

        public async Task<bool> ReorganizeIndex(string indexName, string entityName)
        {
            using (var connection = new SqlConnection(_connectionString))
            {

                // var entityFilter = tablesOnly ? "('U')" : viewsOnly ? "('V')" : "('U', 'V')";
                // var tableFilter = !string.IsNullOrEmpty(filter) ? "AND o.name like @filter" : "";

                var sql = $@"alter index {indexName} on {entityName} reorganize";

                connection.Open();

                try
                {
                    var result = await connection.ExecuteAsync(sql);

                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
                finally
                {
                    connection.Close();
                }

            }
        }
        public async Task<bool> RebuildIndex(string indexName, string entityName, double fillFactor = 80)
        {
            using (var connection = new SqlConnection(_connectionString))
            {

                // var entityFilter = tablesOnly ? "('U')" : viewsOnly ? "('V')" : "('U', 'V')";
                // var tableFilter = !string.IsNullOrEmpty(filter) ? "AND o.name like @filter" : "";

                var sql = $@"alter index {indexName} on {entityName} rebuild WITH (FILLFACTOR = {fillFactor}, SORT_IN_TEMPDB = ON, STATISTICS_NORECOMPUTE = ON) ";

                connection.Open();

                try
                {
                    var result = await connection.ExecuteAsync(sql);
                    
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
                finally
                {
                    connection.Close();
                }

            }
        }
    }
}