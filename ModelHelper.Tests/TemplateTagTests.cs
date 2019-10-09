using System.Collections.Generic;
using ModelHelper.Core.Extensions;
using ModelHelper.Core.Models;
using ModelHelper.Core.Project;
using ModelHelper.Core.Templates;
using ModelHelper.Extensibility;
using Xunit;

namespace ModelHelper.Tests
{
    public class TemplateTagTests
    {

        [Fact]
        public void DictionaryTestShould_Return_Correct_Output()
        {
            var template = TemplateForDictionaryTest();
            var table = TestTable();
            var model = new TemplateModel();
            model.Table = table;
            model.Project = new Project{};

            var result = template.Render(model);

            Assert.Equal("DateTimeGraphType", result);
        }

        [Fact]
        public void Dictionary_Extended_TestShould_Return_Correct_Output()
        {
            var template = ExtendedTemplateForDictionaryTest();
            var table = TestTable();
            var model = new TemplateModel();
            model.Table = table;
            model.Project = new Project { };

            var result = template.Render(model);

            Assert.Equal("DateTimeGraphType", result);
        }
        internal ITemplate TemplateForDictionaryTest()
        {
            var root = RootTemplate();

            root.Body = "{% dictionary DateTime %}";


            return root;
        }
        internal ITemplate ExtendedTemplateForDictionaryTest()
        {
            var root = RootTemplate();

            root.Body = "{% for column in model.Table.Columns %} {{ column.DataType | CSharp | Dictionary }} {% endfor %}";


            return root;
        }

        internal Table TestTable()
        {
            var table = new Table
            {
                Schema = "dbo",
                Name = "Test",
                Columns = new List<IColumn>
                {
                    new Column{Name = "Test1", PropertyName = "Test1", DataType = "DateTime"},
                    new Column{Name = "Test2", PropertyName = "Test2", DataType = "NVarchar"},
                }
            };

            return table;
            //var drop = new TableDrop(table);
            //var result = drop.SqlForInsert();
            //var expected = builder.ToString();
        }

        internal ModelHelper.Core.Templates.ITemplate RootTemplate()
        {
            var reader = new JsonTemplateReader();
            var template = reader.ReadFromContent(rootJson, "test");

            return template;
        }


        internal static string rootJson = @"{
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
    }
}