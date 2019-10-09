using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ModelHelper.Core.Drops;
using ModelHelper.Core.Templates;
using ModelHelper.Extensions;
using DotLiquid;
using DotLiquid.NamingConventions;

namespace ModelHelper.Core.Extensions
{
    public static class TemplateExtensions
    {
        public static void InitializeTemplate()
        {
            Template.NamingConvention = new CSharpNamingConvention();
            Template.RegisterFilter(typeof(TextFilter));
            Template.RegisterTag<SqlColumnToPropertyList>("columnList");
            Template.RegisterTag<PrimaryKeyList>("primaryKeyList");
            Template.RegisterTag<PropertyList>("propertyList");
            Template.RegisterTag<Snippet>("snippet");

            Template.RegisterTag<SqlInsert>("sqlInsert");
            Template.RegisterTag<SqlUpdate>("sqlUpdate");
            Template.RegisterTag<SqlDelete>("sqlDelete");
            Template.RegisterTag<SqlSelectAll>("sqlSelectAll");
            Template.RegisterTag<SqlSelectSingle>("sqlSelectSingle");
            Template.RegisterTag<DictionaryValue>("dictionary");
        }
        public static string Render(this ITemplate template)
        {
            var output = "";

            if (template != null)
            {
                InitializeTemplate();

                var parsedTemplate = Template.Parse(template.Body);

                output = parsedTemplate.Render();

                
            }

            return output;
        }

        public static string Render(this string template, ITemplateModel model)
        {
            var output = "";

            if (!string.IsNullOrEmpty(template))
            {
                
                InitializeTemplate();

                var parsedTemplate = Template.Parse(template);
                var dropModel = model.CreateDrop();
                //parsedTemplate.MakeThreadSafe();
                try
                {
                    output = parsedTemplate.Render(dropModel);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    //throw;
                }

            }


            return output;
        }
        public static string Render(this ITemplate template, ITemplateModel model)
        {
            var output = "";

            if (template.Dictionary != null)
            {
                if (model.Dictionary == null)
                {
                    model.Dictionary = new Dictionary<string, string>();
                }

                foreach (var pair in template.Dictionary)
                {
                    var key = pair.Key.ToLowerInvariant();

                    if (!model.Dictionary.ContainsKey(key))
                    {
                        model.Dictionary.Add(key, pair.Value);
                    }
                    else
                    {
                        model.Dictionary[key] = pair.Value;
                    }
                }
                
            }
            if (template != null)
            {
                output = Render(template.Body, model);                                
            }                

            return output.Replace("ASCII123", "{").Replace("ASCII124", "}");
        }

        public static string ColumnsForSelect(this List<DataColumnDrop> columns, string alias)
        {
            var stringBuilder = new StringBuilder();
            var index = 0;

            foreach (var column in columns.Where(c => !c.IsIgnored))
            {
                var comma = index++ == 0 ? "  " : ", ";
                var columnText = string.Equals(column.PropertyName, column.Name, StringComparison.CurrentCultureIgnoreCase)
                    ? $"{alias}.[{column.Name}]"
                    : $"{column.PropertyName} = {alias}.[{column.Name}]";

                stringBuilder.AppendLine($"\t{comma}{columnText}");
            }

            //for (int i = 0; i < columns.Count; i++)
            //{
            //    var comma = i == 0 ? "  " : ", ";
            //    var column = columns[i];
            //    var columnText = string.Equals(column.PropertyName, column.Name, StringComparison.CurrentCultureIgnoreCase)
            //        ? $"{alias}.[{column.Name}]"
            //        : $"{column.PropertyName} = {alias}.[{column.Name}]";

            //    stringBuilder.AppendLine($"\t{comma}{columnText}");
            //}

            return stringBuilder.ToString();
        }

        public static string ColumnsForUpdate(this List<DataColumnDrop> columns)
        {
            var stringBuilder = new StringBuilder();

            var index = 0;

            foreach (var column in columns.Where(c => !c.IsPrimaryKey))
            {               
                var comma = index == 0 ? "  " : ", ";
                var columnText = $"[{column.Name}] = @{column.PropertyName}";

                if (column.IsModifiedDate)
                {
                    columnText = $"[{column.Name}] = @Date";
                }
                else if (column.IsModifiedByUser)
                {
                    columnText = $"[{column.Name}] = @User";
                }
                else if (column.IsDeletedMarker)
                {
                    columnText = $"[{column.Name}] = @{column.PropertyName}";
                }
                else if (column.IsIgnored)
                {
                    
                    continue;
                }
                
                stringBuilder.AppendLine($"\t{comma}{columnText}");

                    index++;
                
            }
            
            return stringBuilder.ToString();
        }

        public static string SqlForInsert(this ITableDrop table)
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.AppendLine($"INSERT INTO [{table.Schema}].[{table.Name}] (");
            stringBuilder.Append(table.AllColumns.ColumnsForInsert());
            stringBuilder.Append(")");
            

            return stringBuilder.ToString();
        }

        public static string SqlForUpdate(this ITableDrop table)
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.AppendLine($"UPDATE [{table.Schema}].[{table.Name}] SET");
            stringBuilder.AppendLine(table.AllColumns.ColumnsForUpdate());
            stringBuilder.AppendLine("WHERE");
            stringBuilder.AppendLine($"\t{table.PrimaryKeys.PrimaryKeyList("sql")}");

            return stringBuilder.ToString();
        }
        
        public static string SqlForDelete(this ITableDrop table)
        {
            var stringBuilder = new StringBuilder();           
            
            if (table.UsesDeletedColumn)
            {
                stringBuilder.AppendLine($"UPDATE [{table.Schema}].[{table.Name}] Set {table.DeletedColumnName} = 0 WHERE {table.PrimaryKeys.PrimaryKeyList("sql")}");
            }
            else
            {
                stringBuilder.AppendLine($"DELETE FROM [{table.Schema}].[{table.Name}] WHERE {table.PrimaryKeys.PrimaryKeyList("sql")}");
            }

            return stringBuilder.ToString();
        }

        public static string PropertyList(this ModelDrop model, string itemOwner)
        {
            var stringBuilder = new StringBuilder();

            for (int i = 0; i < model.Table.AllColumns.Count; i++)
            {
                
                var comma = i == 0 ? "  " : ", ";
                var column = model.Table.AllColumns[i];

                var comment = column.IsIgnored ? "// " : "";
                var propertyText = column.IsIgnored ? $"{column.PropertyName} = ?? // TODO: Get proper insert value" : $"{itemOwner}.{column.PropertyName}";

                if (column.IsModifiedDate)
                {
                    stringBuilder.AppendLine($"\t{comma}Date = System.DateTime.UtcNow");
                }
                else if (column.IsModifiedByUser)
                { var user = "String.Empty";
                    
                    if (model.InjectUserContext && model.UserContext != null)
                    {
                        var userName = model.UserContext.UserProperty;
                        var userContext = $"_{model.UserContext.VariableName}";

                        user = $"{ userContext}.{userName}";
                    }
                    else if (model.UseQueryOptions && model.QueryOptions != null)
                    {
                        var userName = model.QueryOptions.UserIdProperty;
                        var userContext = $"{model.QueryOptions.ClassName}";

                        user = $"{ userContext}.{userName}";
                    }

                    stringBuilder.AppendLine($"\t{comma}User = {user}");
                }       
                else if (column.IsIgnored)
                {
                    // do nothing
                }
                else
                {
                    stringBuilder.AppendLine($"\t{comment}{comma}{propertyText}");
                }
                
            }

            return stringBuilder.ToString();
        }
        public static string SqlForSelectAll(this TableDrop table, bool includeParentRelations = true)
        {
            var stringBuilder = new StringBuilder();
            
            stringBuilder.AppendLine("SELECT");
            stringBuilder.AppendLine(table.Columns.ColumnsForSelect(table.Alias));
            if (includeParentRelations)
            {                
                foreach (var parentRelation in table.ParentRelations)
                {
                    
                    foreach (var relationColumn in parentRelation.ViewModelColumns)
                    {
                        if (relationColumn.IsCreatedByUser || relationColumn.IsModifiedByUser)
                        {
                            stringBuilder.AppendLine($"\t, {relationColumn.PropertyName} = {parentRelation.Alias}.[{relationColumn.Name}]");
                        }
                        else if (!relationColumn.IsIgnored)
                        {
                            stringBuilder.AppendLine($"\t, {relationColumn.PropertyName} = {parentRelation.Alias}.[{relationColumn.Name}]");
                        }
                        //stringBuilder.AppendLine($"\t, {parentRelation.Name.AsUpperCamelCase().SingularizeWord()}{relationColumn.PropertyName} = {parentRelation.Alias}.[{relationColumn.Name}]");
                        
                    }                    
                }

            }
            stringBuilder.AppendLine($"FROM [{table.Schema}].[{table.Name}] {table.Alias}");
            if (includeParentRelations)
            {
                
                foreach (var parentRelation in table.ParentRelations)
                {
                    var relationColumnObject = parentRelation.AllColumns.FirstOrDefault(c =>
                        c.Name.Equals(parentRelation.ChildColumnName, StringComparison.InvariantCultureIgnoreCase));

                    
                    stringBuilder.AppendLine($"LEFT JOIN {parentRelation.Name} {parentRelation.Alias} on {parentRelation.Alias}.{parentRelation.ParentColumnName} = {table.Alias}.{parentRelation.ChildColumnName} ");
                }
               
            }
            return stringBuilder.ToString();
        }

        public static string SqlForSelectFromPrimaryKeys(this TableDrop table)
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.AppendLine(table.SqlForSelectAll(false));
            stringBuilder.AppendLine("WHERE");            
            stringBuilder.AppendLine($"\t{table.PrimaryKeys.PrimaryKeyList("sql", table.Alias + ".")}");

            return stringBuilder.ToString();
        }
        public static string ColumnsForInsert(this List<DataColumnDrop> columns)
        {
            var sbColumns = new StringBuilder();
            var sbProperties = new StringBuilder();
            var combined = new StringBuilder();

            var index = 0;

            foreach (var column in columns.Where(c => !c.IsIdentity))
            {
                var runCounter = true;
                var comma = index == 0 ? "  " : ", ";                
                
                
                if (column.IsCreatedDate || column.IsModifiedDate)
                {
                    sbColumns.AppendLine($"\t{comma}[{column.Name}]");
                    sbProperties.AppendLine($"\t{comma}@Date");
                }
                else if (column.IsCreatedByUser || column.IsModifiedByUser)
                {
                    sbColumns.AppendLine($"\t{comma}[{column.Name}]");
                    sbProperties.AppendLine($"\t{comma}@User");
                }
                else if (column.IsDeletedMarker)
                {
                    sbColumns.AppendLine($"\t{comma}[{column.Name}]");
                    sbProperties.AppendLine($"\t{comma}@{column.PropertyName}");
                }
                else if (column.IsIgnored)
                {
                    runCounter = false;
                }
                else
                {
                    sbColumns.AppendLine($"\t{comma}[{column.Name}]");
                    sbProperties.AppendLine($"\t{comma}@{column.PropertyName}");
                }

                if (runCounter)
                {
                    index++;
                }
            }

            combined.Append(sbColumns);
            combined.AppendLine(") VALUES (");
            combined.Append(sbProperties);

            return combined.ToString();
        }

        public static string ColumnList(this List<DataColumnDrop> columns, string alias)
        {
            var sbColumns = new StringBuilder();
            
            for (int i = 0; i < columns.Count; i++)
            {
                var comma = i == 0 ? "  " : ", ";
                var column = columns[i];                

                sbColumns.AppendLine($"\t{comma}{ alias}.[{column.Name}]");
            }

            return sbColumns.ToString();
        }

        public static string PrimaryKeyList(this List<DataColumnDrop> columns, string language, string alias = "")
        {
            var stringBuilder = new StringBuilder();

            for (int i = 0; i < columns.Count; i++)
            {
                var comma = i == 0 ? "" : ", ";
                var slash = i == 0 ? "" : "/";
                var column = columns[i];
                
                switch (language.ToLowerInvariant())
                {
                    case "typescript":
                    case "ts":
                        stringBuilder.Append(
                            $"{comma}{TextFilter.LowerCamel(column.PropertyName)}: {TextFilter.TypeScript(column.DataType)}");
                        break;
                    case "sql":                        
                        stringBuilder.Append(
                            $"{comma}{alias}[{column.Name}] = @{column.PropertyName}");
                        break;
                    case "cs":                        
                    case "csharp":                        
                        stringBuilder.Append(
                            $"{comma}{TextFilter.CSharp(column.DataType)} {TextFilter.LowerCamel(column.PropertyName)}");
                        break;
                    case "apiparam":
                        var startTag = "ASCII123";
                        var endTag = "ASCII124";
                        stringBuilder.Append(
                            $"{slash}{startTag}{TextFilter.LowerCamel(column.PropertyName)}{endTag}");
                        break;
                    default:
                        stringBuilder.Append(
                            $"{comma}{TextFilter.LowerCamel(column.PropertyName)}");
                        break;
                }
            }

            return stringBuilder.ToString();
        }

        public static string AsCommaSeparatedString(this IEnumerable<string> items)
        {
            var stringBuilder = new StringBuilder();
            var i = 0;

            foreach (var item in items)
            {
                var comma = i == 0 ? "" : ", ";
                stringBuilder.Append(
                            $"{comma}{item}");
                i++;
            }
                        
            return stringBuilder.ToString();
        }
    }
}