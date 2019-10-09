using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using ModelHelper.Core.Database;
using ModelHelper.Core.Drops;
using ModelHelper.Core.Extensions;
using ModelHelper.Core.Models;
using ModelHelper.Core.Rules;
using ModelHelper.Core.Templates;
using ModelHelper.Extensibility;
using Xunit;

namespace ModelHelper.Tests
{
    public class TemplateExtensionsPrimaryColumnTests
    {
        [Fact]
        public void A_List_Of_Columns_Should_Return_Valid_list()
        {
            var columns = new List<DataColumnDrop> { new DataColumnDrop(new Column { DataType = "int", IsPrimaryKey = true, Name = "Test", PropertyName = "Test" }) };

            var result = columns.PrimaryKeyList("apiParam");
            var expect = "ASCII123testASCII124";

            Assert.NotNull(columns);
            Assert.True(columns.Any());
            Assert.Equal(expect, result);
        }

        [Fact]
        public void A_List_Of_Columns_With_Two_Items_Should_Return_Valid_list()
        {
            var columns = new List<DataColumnDrop>
            {
                new DataColumnDrop(new Column { DataType = "int", IsPrimaryKey = true, Name = "Test", PropertyName = "Test" }),
                new DataColumnDrop(new Column { DataType = "int", IsPrimaryKey = true, Name = "Test2", PropertyName = "Test2" })
            };

            Assert.NotNull(columns);
            Assert.True(columns.Any());

            var result = columns.PrimaryKeyList("apiParam");
            var expect = "ASCII123testASCII124/ASCII123test2ASCII124";

            Assert.Equal(expect, result);

            var resultCs1 = columns.PrimaryKeyList("cs");
            var resultCs2 = columns.PrimaryKeyList("csharp");
            var expectCs = "int test, int test2";

            Assert.Equal(resultCs1, resultCs2);
            Assert.Equal(expectCs, resultCs2);


            var resultTs = columns.PrimaryKeyList("ts");
            var expectTs = "test: number, test2: number";

            Assert.Equal(resultTs, columns.PrimaryKeyList("typescript"));
            Assert.Equal(expectTs, resultTs);
        }                
    }

    public class TestingTableRules
    {
        [Fact]
        public void A_Table_With_OneIdentity_That_Is_Also_Primary_Should_Pass()
        {
            var table = new Table();
            table.Columns = new List<IColumn>
            {
                new Column{IsPrimaryKey = true, IsIdentity = true}
            };

            var ruleToTest = new ModelHelper.Core.Rules.IdentityIsAlsoPrimary();
            var result = ruleToTest.Evaluate(table);
            var expect = EvaluationResultOption.Passes; 

            Assert.NotNull(result);
            Assert.Equal(expect, result.Result);
        }

        [Fact]
        public void A_Table_With_OneIdentity_That_Is_Not_Primary_Should_Fail()
        {
            var table = new Table();
            table.Columns = new List<IColumn>
            {
                new Column{IsPrimaryKey = false, IsIdentity = true}
            };

            var ruleToTest = new ModelHelper.Core.Rules.IdentityIsAlsoPrimary();
            var result = ruleToTest.Evaluate(table);
            var expect = EvaluationResultOption.Failed;

            Assert.NotNull(result);
            Assert.Equal(expect, result.Result);
        }

        [Fact]
        public void A_Table_With_Identity_With_Multi_Primary_Should_Fail()
        {
            var table = new Table();
            table.Columns = new List<IColumn>
            {
                new Column{IsPrimaryKey = true, IsIdentity = true},
                new Column{IsPrimaryKey = true, IsIdentity = false},
            };

            var ruleToTest = new ModelHelper.Core.Rules.TableWithIdentityCannotHaveMultiPrimary();
            var result = ruleToTest.Evaluate(table);
            var expect = EvaluationResultOption.Failed;

            Assert.NotNull(result);
            Assert.Equal(expect, result.Result);
        }

        [Fact]
        public void A_Table_Without_any_primary_keys_Should_Fail()
        {
            var table = new Table();
            table.Columns = new List<IColumn>
            {
                new Column{IsPrimaryKey = false},
                new Column{IsPrimaryKey = false},
            };

            var ruleToTest = new ModelHelper.Core.Rules.TableImplementsPrimaryKey();
            var result = ruleToTest.Evaluate(table);
            var expect = EvaluationResultOption.Failed;

            Assert.NotNull(result);
            Assert.Equal(expect, result.Result);
        }

        [Fact]
        public void A_Table_Columns_Ending_With_Id_and_no_Constraints_Should_Fail()
        {
            var table = new Table();
            table.Columns = new List<IColumn>
            {
                new Column{Name = "Id"},
                new Column{Name = "ParentId"},
            };

            var ruleToTest = new ModelHelper.Core.Rules.CheckForConstraintRule();
            var result = ruleToTest.Evaluate(table);
            var expect = EvaluationResultOption.Warning;

            Assert.NotNull(result);
            Assert.Equal(expect, result.Result);
        }

        [Fact]
        public void A_Table_Using_Different_Collation_Should_Fail()
        {
            var table = new Table();
            table.Columns = new List<IColumn>
            {
                new Column{Collation = "COLL_1"},
                new Column{Collation = "COLL_2"},
            };

            var ruleToTest = new ModelHelper.Core.Rules.TableUseSameCollation();
            var result = ruleToTest.Evaluate(table);
            var expect = EvaluationResultOption.Failed;

            Assert.NotNull(result);
            Assert.Equal(expect, result.Result);
        }

        [Fact]
        public void A_Table_Using_Same_Collation_Should_Pass()
        {
            var table = new Table();
            table.Columns = new List<IColumn>
            {
                new Column{Collation = "COLL_1"},
                new Column{Collation = "COLL_1"},
            };

            var ruleToTest = new ModelHelper.Core.Rules.TableUseSameCollation();
            var result = ruleToTest.Evaluate(table);
            var expect = EvaluationResultOption.Passes;

            Assert.NotNull(result);
            Assert.Equal(expect, result.Result);
        }

        [Fact]
        public void A_Valie_Table_Should_Pass_all_rules()
        {
            var table = new Table();
            table.Columns = new List<IColumn>
            {
                new Column{Name = "Id", IsPrimaryKey = true, IsIdentity = true},
                new Column{Collation = "COLL_1", Name = "Test"},
            };

            var eval = new TableEvaluator();
            var results = eval.Evaluate(table);
            var expect = EvaluationResultOption.Passes;

            Assert.NotNull(results);
            
            
                Assert.Equal(expect, results.Result);
            

            
       
        }

        [Fact]
        public void An_InValid_Table_Should_Fail_all_rules()
        {
            var table = new Table();
            table.Columns = new List<IColumn>
            {
                new Column{Name = "Id", IsPrimaryKey = false, IsIdentity = true},
                new Column{Collation = "COLL_1", Name = "Test"},
            };

            var eval = new TableEvaluator();
            var results = eval.Evaluate(table);
            var expect = EvaluationResultOption.Passes;

            Assert.NotNull(results);
            
            var failCount = results.Result == EvaluationResultOption.Failed;
            Assert.True(failCount);



        }
    }

    public class TemplateJsonTests
    {
        [Fact]
        public void Template_Should_Load_Dictionary_Items()
        {
            var reader = new JsonTemplateReader();
            ModelHelper.Core.Templates.ITemplate template = reader.ReadFromContent(ValidTemplateJson(), "test");
            
            Assert.NotNull(template);

            Assert.Equal(2, template.Dictionary.Count());
        }


        internal static string ValidTemplateJson()
        {
            var json = @"{
  ""Key"": ""a54200df-f7b3-4b33-9bf9-521a3a5af63f"",
  ""Name"": ""repositotory-model-full"",
  ""Description"": ""Creates a <Entity>Type file based on ObjectGraphType for GraphQL"",
  ""Language"": ""cs"",
  ""CanExport"": true,
  ""ExportFileName"": ""{{model.Table.Name | UpperCamel | Singular}}Type.cs"",
  ""ExportType"": ""gql.types"",
  ""TemplateType"": 0,
  ""Groups"": [
    ""GraphQLCore""
  ],
  ""Tags"": [""GraphQL"", ""Api"", ""Core""],
  ""typeMapper"": ""points to name"",
  ""dictionary"": [
    { ""key"": ""string"", ""value"": ""StringType""},
    { ""key"": ""DateTime"", ""value"": ""DateTimeGraphType""}
  ],
  ""Body"": [
    ""using System;"",
    ""using System.Collections.Generic;"",
    ""using GraphQL.Types;"",
    ""using GraphQL.DataLoader;"",
    ""using {{model.Namespaces[\""api.interfaces\""]}};"",
    ""using {{model.Namespaces[\""api.models\""]}};"",
    ""{% capture repo %}{{ model.Table.Name | LowerCamel | Singular }}Repository{% endcapture %}"",
    ""namespace {{model.Namespaces[\""gql.types\""]}}"",
    ""{"",
    ""\tpublic class {{model.Table.Name | UpperCamel | Singular}}Type : ObjectGraphType<{{model.Table.Name | UpperCamel | Singular}}>"",
    ""\t{"",
      ""\t\tpublic {{model.Table.Name | UpperCamel | Singular}}Type(IDataLoaderContextAccessor accessor, I{{ model.Table.Name | UpperCamel | Singular }}Repository {{ repo }})"", 
      ""\t\t{"", 
        ""\t\t\tName = \""{{model.Table.Name | UpperCamel | Singular}}\"";"", 
        ""\t\t\tDescription = \""{{model.Table.Description}}\"";"", 
        ""{%- for property in model.Table.Columns -%}{% capture dataType %}{{ property.DataType | CSharp }}{% endcapture -%}"",
        ""{%- capture nullable %}{%if property.IsNullable %}, nullable: true{% endif %}{% endcapture -%}"", 
        ""{%- capture description %}{%if property.Description.Length > 0 %}.Description(\""{{property.Description}}\""){% endif %}{% endcapture -%}"", 
          ""\t\t\t{% if property.IsIgnored %}// {% endif %}Field(t => t.{{ property.PropertyName }}{{ nullable }}){{description}};"",
          ""{%- endfor -%}"",            
          
          ""{%- for child in model.Table.ChildRelations -%}{%- if child.GroupIndex == 1 -%}"",
          ""\t\t\tField<ListGraphType<{{ child.Name | UpperCamel | Singular }}Type>, IEnumerable<{{ child.Name | UpperCamel | Singular }}>>()"",
          ""\t\t\t\t.Name(\""{{child.ModelName | LowerCamel | Plural}}\"")"",
          ""\t\t\t\t.ResolveAsync(ctx => "",
          ""\t\t\t\t{"",
          ""\t\t\t\t\tvar dataLoader = accessor.Context.GetOrAddCollectionBatchLoader<{{ child.ParentColumnType  | CSharp }}, {{ child.Name | UpperCamel | Singular }}>(\""Get{{child.ModelName | UpperCamel | Plural}}\"", {{ repo }}.Get{{child.ModelName | UpperCamel | Plural}});"",
          ""\t\t\t\t\treturn dataLoader.LoadAsync(ctx.Source.{{ child.ParentColumnName | UpperCamel | Singular }});"",            
          ""\t\t\t\t});"",
          ""{% endif -%}{%- endfor %}"",        
          
      ""\t\t}"", 
      """",

    ""\t}"",
    ""}""

  ]
}";
            return json;
        }
    }
}