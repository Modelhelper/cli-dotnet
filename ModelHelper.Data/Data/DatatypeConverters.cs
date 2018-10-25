using System.Collections.Generic;
using System.ComponentModel.Composition;
using ModelHelper.Extensibility;

namespace ModelHelper.Data
{
    [Export(typeof(IDatatypeConverter))]
    [ExportMetadata("FromLanguage", "mssql")]
    [ExportMetadata("ToLanguage", "csharp")]
    public class SqlToCSharpDatatypeConvert : IDatatypeConverter
    {

        public string Convert(string from, string to)
        {
            var dict = new Dictionary<string, string>
            {
                {"bigint", "Int64"},
                {"binary", "Byte[]"},
                {"bit", "bool"},
                {"char", "char"},
                {"date", "DateTime"},
                {"datetime", "DateTime"},
                {"datetime2", "DateTime"},
                {"datetimeoffset", "DateTime"},
                {"decimal", "decimal"},
                {"float", "decimal"},
                {"geography", "string"},
                {"geometry", "string"},
                {"image", "string"},
                {"int", "int"},
                {"money", "decimal"},
                {"nchar", "string"},
                {"ntext", "string"},
                {"numeric", "decimal"},
                {"nvarchar", "string"},
                {"real", "decimal"},
                {"smalldatetime", "decimal"},
                {"smallint", "decimal"},
                {"smallmoney", "decimal"},
                {"text", "string"},
                {"time", "DateTime"},
                {"timestamp", "string"},
                {"tinyint", "number"},
                {"uniqueidentifier", "Guid"},
                {"varbinary", "Byte[]"},
                {"varchar", "string"},
                {"xml", "XElement"},
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
    }

    [Export(typeof(IDatatypeConverter))]
    [ExportMetadata("FromLanguage", "mssql")]
    [ExportMetadata("ToLanguage", "postgres")]
    public class SqlToPostgresConverter : IDatatypeConverter
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
[ExportMetadata("ToLanguage", "mysql")]
public class SqlToMySqlConverter : IDatatypeConverter
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
