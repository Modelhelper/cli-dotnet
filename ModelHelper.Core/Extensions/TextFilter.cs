using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Humanizer;
using ModelHelper.Extensibility;

namespace ModelHelper.Core.Extensions
{
    public static class TextFilter
    {
        public static List<IDatatypeConverter> Converters { get; set; } = new List<IDatatypeConverter>();
        public static string Datasource { get; set; } = "mssql";
        public static string TypeScript(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return "";
            }
            var dict = SqlTypeToTypeScript();

            if (dict.ContainsKey(input))
            {
                return dict[input];
            }

            return input;

        }

        public static string Dictionary(string input)
        {
            return input.ToUpper();
        }

        private static Dictionary<string, string> SqlTypeToTypeScript()
        {
            

                return new Dictionary<string, string>
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
            
        }

        public static string CSharp(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return "";
            }
            var dict = SqlTypeToSCharp();
            var testKey = input.ToLowerInvariant();

            //var converter = Converters.FirstOrDefault();
            if (dict.ContainsKey(testKey))
            {
                return dict[testKey];
            }

            return input;

        }

        private static Dictionary<string, string> SqlTypeToGraphQL()
        {


            return new Dictionary<string, string>
                {
                    { "bigint", "Int"},
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

        }

        public static string Graphql(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return "";
            }
            var dict = SqlTypeToGraphQL();
            var testKey = input.ToLowerInvariant();

            //var converter = Converters.FirstOrDefault();
            if (dict.ContainsKey(testKey))
            {
                return dict[testKey];
            }

            return input;

        }

        private static Dictionary<string, string> SqlTypeToSCharp()
        {
            var dict = new Dictionary<string, string>
            {
                {"bigint", "Int64"},
                {"binary", "Byte[]"},
                {"bit", "bool"},
                {"char", "char"},
                {"date", "DateTimeOffset"},
                {"datetime", "DateTimeOffset"},
                {"datetime2", "DateTimeOffset"},
                {"datetimeoffset", "DateTimeOffset"},
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
                {"time", "DateTimeOffset"},
                {"timestamp", "string"},
                {"tinyint", "number"},
                {"uniqueidentifier", "Guid"},
                {"varbinary", "Byte[]"},
                {"varchar", "string"},
                {"xml", "XElement"},
            };
            return dict;
        }

        public static string Words(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return "";
            }

            var words = input.AsWords();
            if (string.IsNullOrEmpty(words))
            {
                return input;
            }
            return words;
        }
        public static string Plural(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return "";
            }

            var pluralizied = input.PluralizeWord();
            if (string.IsNullOrEmpty(pluralizied))
            {
                return input;
            }
            return pluralizied;

        }

        public static string Singular(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return "";
            }

            var singular = input.SingularizeWord();
            if (string.IsNullOrEmpty(singular))
            {
                return input;
            }
            return singular;
        }

        public static string UpperCamel(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return "";
            }

            var isMultiWord = input.IsMultiWord();

            input = input.CleanInput();
            //!isMultiWord ? input :
            //var testcasing = input
            //    .Replace('_', ' ')
            //    .Replace('-', ' ');

            //if (testcasing.EndsWith("ID"))
            //{
            //    testcasing = input.Replace("ID", "Id");
            //}

            TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo; // new TextInfo(CultureInfo.CurrentCulture);
            var result = isMultiWord
                ? textInfo.ToTitleCase(input.ToLowerInvariant()).Trim().Replace(" ", "")
                : input.IsAllUppercase()
                    ? input[0].ToString().ToUpperInvariant() + input.Substring(1).ToLowerInvariant()
                    : input[0].ToString().ToUpperInvariant() + input.Substring(1);

            return result;
        }

        public static string LowerCamel(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return "";
            }

            input = input.UpperCamel();

            var result = input[0].ToString().ToLowerInvariant() + input.Substring(1);
            return result;
        }
        public static string KebabCase(string input)
        {
            var pascal = input.AsUpperCamelCase();
            return pascal.AsKebabCase();
        }        

        public static string SnakeCase(string input)
        {
            return input.AsSnakeCase();
        }

        
        public static string Abrevation(this string input, IEnumerable<string> forbiddenAbbrevations = null)
        {
           
            var sb = new StringBuilder();
            for (int i = 0; i < input.Length; i++)
            {
                if (i == 0 || char.IsUpper(input[i]))
                {
                    sb.Append(input[i]);
                }
            }           

            var result = sb.ToString().ToLower();
            var isForbidden = forbiddenAbbrevations?.Contains(result) ?? false;
            
            return isForbidden ? result + "0" : result;

        }
        
    }
}