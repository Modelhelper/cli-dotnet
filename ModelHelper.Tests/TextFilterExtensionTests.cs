using System.Collections.Generic;
using ModelHelper.Core.Extensions;
using ModelHelper.Extensions;
using Xunit;

namespace ModelHelper.Tests
{
    public class TextFilterExtensionTests
    {
        internal static Dictionary<string, string> SqlDatatypesToTs()
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

        internal static Dictionary<string, string> SqlDatatypesToCs()
        {
            return new Dictionary<string, string>
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
        }
        [Fact]
        public void A_Sql_Type_Should_Map_Correct_TypeScript_Type()
        {
            var types = SqlDatatypesToTs();

            foreach (var type in types)
            {
                string expected = type.Value;
                var actual = TextFilter.TypeScript(type.Key);

                Assert.Equal(expected, actual);
            }
            
           
        }

        [Fact]
        public void A_Sql_Type_Should_Map_Correct_CSharp_Type()
        {
            var types = SqlDatatypesToCs();

            foreach (var type in types)
            {
                string expected = type.Value;
                var actual = TextFilter.CSharp(type.Key);

                Assert.Equal(expected, actual);
            }

        }

        
    }

    public class CommandArgumentExtensionTests
    {
        [Fact]
        public void Only_Arguments_Starting_With_Dash_Should_Be_Key()
        {
            var args = new List<string>
            {
                "--key-1",
                "param 1",
                "param 2"
            };

            var array = args.ToArray();

            var argumentDictionary = array.AsArgumentDictionary();
            
            Assert.NotNull(argumentDictionary);
            Assert.True(argumentDictionary.ContainsKey("--key-1"));
            Assert.Equal(1, argumentDictionary.Count);
        }
    }
}