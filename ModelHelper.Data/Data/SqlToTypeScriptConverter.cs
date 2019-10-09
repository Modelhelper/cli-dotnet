using System.Collections.Generic;
using System.ComponentModel.Composition;
using ModelHelper.Extensibility;

namespace ModelHelper.Data
{
    [Export(typeof(IDatatypeConverter))]
    [ExportMetadata("FromLanguage", "mssql")]
    [ExportMetadata("ToLanguage", "typescript")]
    public class SqlToTypeScriptConverter : IDatatypeConverter
    {
        public string Convert(string from, string to)
        {
            var dict = new Dictionary<string, string>
            {
                {"bigint", "number"},
                {"binary", "string"},
                {"bit", "boolean"},
                {"char", "string"},
                {"date", "Date"},
                {"datetime", "Date"},
                {"datetime2", "Date"},
                {"datetimeoffset", "Date"},
                {"decimal", "number"},
                {"float", "number"},
                {"geography", "string"},
                {"geometry", "string"},
                {"image", "string"},
                {"int", "number"},
                {"money", "number"},
                {"nchar", "string"},
                {"ntext", "string"},
                {"numeric", "number"},
                {"nvarchar", "string"},
                {"real", "number"},
                {"smalldatetime", ""},
                {"smallint", "number"},
                {"smallmoney", "number"},
                {"text", "string"},
                {"time", "string"},
                {"timestamp", "string"},
                {"tinyint", "number"},
                {"uniqueidentifier", "string"},
                {"varbinary", "string"},
                {"varchar", "string"},
                {"xml", "string"},
            };

            if (dict.ContainsKey(from.ToLowerInvariant()))
            {
                return dict[from.ToLowerInvariant()];
            }
            else
            {
                return from;
            }
        }
        
    }

    [Export(typeof(IDatatypeConverter))]
    [ExportMetadata("FromLanguage", "mssql")]
    [ExportMetadata("ToLanguage", "graphql")]
    public class SqlToGraphQLConverter : IDatatypeConverter
    {
        public string Convert(string from, string to)
        {
            var dict = new Dictionary<string, string>
            {
                {"bigint", "Int"},
                {"binary", "String"},
                {"bit", "Boolean"},
                {"char", "String"},
                {"date", "Date"},
                {"datetime", "Date"},
                {"datetime2", "Date"},
                {"datetimeoffset", "Date"},
                {"decimal", "Float"},
                {"float", "Float"},
                {"geography", "String"},
                {"geometry", "String"},
                {"image", "String"},
                {"int", "Int"},
                {"money", "Float"},
                {"nchar", "String"},
                {"ntext", "String"},
                {"numeric", "Float"},
                {"nvarchar", "String"},
                {"real", "Float"},
                {"smalldatetime", ""},
                {"smallint", "Int"},
                {"smallmoney", "Float"},
                {"text", "String"},
                {"time", "String"},
                {"timestamp", "String"},
                {"tinyint", "Int"},
                {"uniqueidentifier", "String"},
                {"varbinary", "String"},
                {"varchar", "String"},
                {"xml", "String"},
            };

            if (dict.ContainsKey(from.ToLowerInvariant()))
            {
                return dict[from.ToLowerInvariant()];
            }
            else
            {
                return from;
            }
        }

    }
}