using System;
using System.Linq;
using System.Text;
using DotLiquid;
using ModelHelper.Core.Database;
using ModelHelper.Core.Drops;
using ModelHelper.Core.Templates;
using ModelHelper.Extensibility;

namespace ModelHelper.Core.Extensions
{
    public static class TableModelExtension
    {
        public static Hash CreateDrop(this ITemplateModel model)
        {
            var modelDrop = new ModelDrop(model);
            return Hash.FromAnonymousObject(new
            {
                model = modelDrop
            });
        }       


    }

    public static class ColumnExtensions
    {
        public static string ExtractContextualName(this IColumn column, string tableName)
        {
            var length = tableName.Length;
            var startsWithTableName = column.Name.StartsWith(tableName, StringComparison.InvariantCultureIgnoreCase);

            if (!startsWithTableName || column.Name.Length == length)
            {
                return column.Name;
            }

            var contextual = column.Name.Substring(length);
            return contextual;
        }
    }
}