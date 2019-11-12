using ModelHelper.Core.Templates;
using System.Linq;
using Xunit;

namespace ModelHelper.UnitTests
{
    public class Valid_template_Version3_tests
    {
        private Template3 _template;

        public Valid_template_Version3_tests()
        {
            _template = ModelHelper.Extensions.TemplateExtensions.LoadTemplateFromContent(validYaml);
        }
        [Fact]
        public void Should_Load_template()
        {
            // arrange

            // act
            

            // assert

            Assert.NotNull(_template);

        }

        [Fact]
        public void template_key_should_be_correct()
        {
            // arrange
            var expected = "api.interface";
            // act

            var actual = _template.Key;
            // assert
            Assert.Equal(expected, actual);
            
        }

        [Fact]
        public void template_language_should_be_correct()
        {
            // arrange
            var expected = "cs";
            // act

            var actual = _template.Language;
            // assert
            Assert.Equal(expected, actual);
            
            
        }

        [Fact]
        public void template_should_have_5_models()
        {
            // arrange
            var expected = 6;
            // act            
            var actual = _template.Models.Count();
            // assert            
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void template_version_should_be_3()
        {
            // arrange
            var expected = "3";
            // act            
            var actual = _template.Version;
            // assert            
            Assert.Equal(expected, actual);
        }



        public static string validYaml = @"
# optional (former ExportType). A key cannot contain .
Key: api.interface 
Version: 3 #mandatory
Language: cs # mandatory field. connects with correct language definition
Description: |
    Long description if needed. Will show when 'mh template <template>'

# Should show then the user enter the 'mh template' command
# optional
ShortDescription: This should be short and concice

# optional
Tags: [tag1, tag2, tag3] 

# what groups this template is included in
# is used to fetch templates withing a group when using -tg in generate command
# optional
Groups: [group1, group2, group3]

# indicates the model classes to be injected
# optional
Models: [table, tables, project, options, template, context]

# optional
ExportType: snippet | file | none

# can use template syntax to use information from the models
# if snippet is used, this will be the filename to inject the snippets to
# optional if ExportType = none
ExportFileName: file-to-export {{ table.name | singular }}

# optional (if ExportType is file or none)
SnippetIdentifier: // MH-TRANSIENT-PLACEHOLDER,

# the body of the template
# mandatory
Body: |
    This is the template body 
";
    }

}
