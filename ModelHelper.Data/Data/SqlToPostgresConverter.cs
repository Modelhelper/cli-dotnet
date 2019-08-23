using System.Collections.Generic;
using System.ComponentModel.Composition;
using ModelHelper.Extensibility;

namespace ModelHelper.Data
{
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