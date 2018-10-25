using System;
using System.Collections.Generic;
using System.IO;
using Xunit;
using ModelHelper.Data;
using ModelHelper.Core.Extensions;
using ModelHelper.Core.Models;
using ModelHelper.Core.Templates;

namespace ModelHelper.Tests
{
    public class TextExtensionsTests
    {

        
        [Fact]
        public void Text_Should_Be_ICompanyRepository()
        {
            var expexted = "ICompanyRepository";
            var template = "I{0}Repository";
            var modelName = "Company";
            var actual = string.Format(template, modelName);

            Assert.Equal(expexted, actual);
        }

        [Fact(DisplayName = "Should be UpperCamelCase")]
        public void Words_With_UnderScore_Should_Return_UpperCamelCase()
        {
            var expected = "ThisIsTest";
            var test = "this_is_test".AsUpperCamelCase();
            Assert.Equal(expected, test);
        }

        [Fact(DisplayName = "Should be LowerCamelCase")]
        public void Words_With_UnderScore_ShouldReturn_LowerCamelCase()
        {
            var expected = "thisIsTest";
            var test = "this_is_test".AsLowerCamelCase();
            Assert.Equal(expected, test);
        }

        [Fact(DisplayName = "String where words are separatet with space should be kebab Case")]
        public void String_separated_with_space_Should_Return_KebabCase()
        {
            var expected = "this-is-test";
            var test = "this is test".AsKebabCase();
            Assert.Equal(expected, test);
        }

        [Fact(DisplayName = "String where words are separatet with space should be kebab Case")]
        public void String_as_pascal_Should_Return_KebabCase()
        {
            var expected = "this-is-test";
            var test = "thisIsTest".AsKebabCase();
            Assert.Equal(expected, test);
        }

        [Fact(DisplayName = "String where words are separatet with space should be snake Case")]
        public void String_separated_with_space_Should_Return_SnakeCase()
        {
            var expected = "this_is_test";
            var test = "this is test".AsSnakeCase();
            Assert.Equal(expected, test);
        }

        [Fact(DisplayName = "String where words are separatet with space should be snake Case")]
        public void String_as_pascal_Should_Return_SnakeCase()
        {
            var expected = "this_is_test";
            var test = "thisIsTest".AsSnakeCase();
            Assert.Equal(expected, test);
        }


        [Fact]
        public void Words_With_Dashes_Should_Return_LowerCamelCase()
        {
            var expected = "thisIsTest";
            var test = "this-is-test".AsLowerCamelCase();
            Assert.Equal(expected, test);
        }

        [Fact]
        public void Words_With_Dashes_Should_Return_UpperCamelCase()
        {
            var expected = "ThisIsTest";
            var test = "this-is-test".AsUpperCamelCase();
            Assert.Equal(expected, test);
        }

        [Fact]
        public void Words_With_Space_And_Proper_Case_Return_LowerCamelCase()
        {
            var expected = "thisIsTest";
            var test = "this is test".AsLowerCamelCase();
            Assert.Equal(expected, test);
        }

        [Fact]
        public void Words_With_Space_And_Proper_Case_Return_UpperCamelCase()
        {
            var expected = "ThisIsTest";
            var test = "this is test".AsUpperCamelCase();
            Assert.Equal(expected, test);
        }

        [Fact]
        public void Words_With_Space_And_Mix_Case_Return_LowerCamelCase()
        {
            var expected = "thisIsTest";
            var test = "tHis iS teSt".AsLowerCamelCase();
            Assert.Equal(expected, test);
        }

        [Fact]
        public void Words_With_Space_And_Mix_Case_Return_UpperCamelCase()
        {
            var expected = "ThisIsTest";
            var test = "tHis iS teSt".AsUpperCamelCase();
            Assert.Equal(expected, test);
        }

        [Fact]
        public void One_Word_Mix_Case_Return_LowerCamelCase()
        {
            var expected = "this";
            var test = "this".AsLowerCamelCase();
            Assert.Equal(expected, test);
        }

        [Fact]
        public void One_Word_Mix_Mix_Case_Return_UpperCamelCase()
        {
            var expected = "This";
            var test = "This".AsUpperCamelCase();
            Assert.Equal(expected, test);
        }

        [Fact]
        public void A_CamelCase_Word_Return_LowerCamelCase()
        {
            var expected = "thIs";
            var test = "thIs".AsLowerCamelCase();
            Assert.Equal(expected, test);
        }

        [Fact]
        public void A_CamelCase_Word_Return_UpperCamelCase()
        {
            var expected = "ThIs";
            var test = "ThIs".AsUpperCamelCase();
            Assert.Equal(expected, test);
        }

        [Fact]
        public void A_Word_Return_KebabCamelCase()
        {
            var expected = "this-is-test";
            var test = "ThisIsTest".AsUpperCamelCase();
            var result = test.AsKebabCase();
            Assert.Equal(expected, result);
        }


        [Fact]
        public void All_CAPS_With_UnderLine_Return_UpperCamelCase()
        {
            var expected = "ThisIsTest";
            var test = "THIS_IS_TEST".AsUpperCamelCase();
            Assert.Equal(expected, test);
        }

        [Fact]
        public void One_Word_All_CAPS_Return_UpperCamelCase()
        {
            var expected = "Test";
            var test = "TEST".AsUpperCamelCase();
            Assert.Equal(expected, test);
        }

        [Fact]
        public void One_Word_All_CAPS_Return_LowerCamelCase()
        {
            var expected = "test";
            var test = "TEST".AsLowerCamelCase();
            Assert.Equal(expected, test);
        }


        [Fact]
        public void All_CAPS_With_UnderLine_Return_LowerCamelCase()
        {
            var expected = "thisIsTest";
            var test = "THIS_IS_TEST".AsLowerCamelCase();
            Assert.Equal(expected, test);
        }

        [Fact]
        public void A_Word_Separated_With_Dashed_Is_Multiword()
        {            
            var test = "this-is-a-multiword".IsMultiWord();
            Assert.True(test);
        }

        [Fact]
        public void A_Word_Separated_With_Space_Is_Multiword()
        {
            var test = "this is a multiword".IsMultiWord();
            Assert.True(test);
        }

        [Fact]
        public void A_Word_Separated_With_Underline_Is_Multiword()
        {
            var test = "this_is_a_multiword".IsMultiWord();
            Assert.True(test);
        }

        [Fact]
        public void All_UPPERCASE_Word_Separated_With_Underline_Is_Multiword()
        {
            var test = "THIS_IS_A_MULTIWORD".IsMultiWord();
            Assert.True(test);
        }

        [Fact]
        public void A_CamelCaseWord_Is_Not_A_Multiword()
        {
            var test = "thisIsNotAMultiword".IsMultiWord();
            Assert.False(test);
        }

        [Fact]
        public void All_UPPERCASE_Word_IS_UpperCase()
        {
            var test = "ALLUPPER".IsAllUppercase();
            Assert.True(test);
        }

        [Fact]
        public void All_lowercase_Word_is_not_UpperCase()
        {
            var test = "allupper".IsAllUppercase();
            Assert.False(test);
        }

        [Fact]
        public void A_Mix_Case_Word_is_not_UpperCase()
        {
            var test = "allUppEr".IsAllUppercase();
            Assert.False(test);
        }

        [Fact]
        public void Company_Should_Pluralize_As_Companies()
        {
            var word = "company";
            var result = "companies";

            var actual = word.PluralizeWord().ToLowerInvariant();
            
            Assert.Equal(result, actual);
        }

        [Fact]
        public void Child_Should_Pluralize_As_Children()
        {
            var word = "child";
            var result = "children";

            var actual = word.PluralizeWord().ToLowerInvariant();

            Assert.Equal(result, actual);
        }

        [Fact]
        public void Attendee_Should_Pluralize_As_Attendees()
        {
            var word = "attendee";
            var result = "attendees";

            var actual = word.PluralizeWord().ToLowerInvariant();

            Assert.Equal(result, actual);
        }

        [Fact]
        public void Status_Should_Pluralize_As_Status()
        {
            var word = "status";
            var result = "statuses";

            var actual = word.PluralizeWord().ToLowerInvariant();

            Assert.Equal(result, actual);
        }

        [Fact]
        public void Species_Should_Pluralize_As_Species()
        {
            var word = "LocationSpecies";
            var result = "LocationSpecies".ToLowerInvariant();

            var actual = word.PluralizeWord().ToLowerInvariant();

            Assert.Equal(result, actual);
        }

        [Fact]
        public void Species_Should_Singularize_As_Species()
        {
            var word = "LocationSpecies";
            var result = "locationspecies";

            var actual = word.SingularizeWord().ToLowerInvariant();

            Assert.Equal(result, actual);
        }

        [Fact]
        public void Combined_Status_Should_Pluralize_As_Status()
        {
            var word = "combinded_status".AsUpperCamelCase();
            var result = "combindedstatuses";

            var actual = word.PluralizeWord().ToLowerInvariant();

            Assert.Equal(result, actual);
        }

        [Fact]
        public void Process_Should_Pluralize_As_Process()
        {
            var word = "process";
            var result = "processes";

            var actual = word.PluralizeWord().ToLowerInvariant();

            Assert.Equal(result, actual);
        }



        [Fact]
        public void Status_Should_Singularize_As_Status()
        {
            var word = "status";
            var result = "status";

            var actual = word.SingularizeWord().ToLowerInvariant();

            Assert.Equal(result, actual);
        }

        [Fact]
        public void Process_Should_Singularize_As_Process()
        {
            var word = "process";
            var result = "process";

            var actual = word.SingularizeWord().ToLowerInvariant();

            Assert.Equal(result, actual);
        }

        [Fact]
        public void Combined_Status_Should_Singularize_Correctly()
        {
            var word = "combinded_status".AsLowerCamelCase();
            var result = "combindedstatus";

            var actual = word.SingularizeWord().ToLowerInvariant();

            Assert.Equal(result, actual);
        }

        [Fact]
        public void Combined_Process_Should_Singularize_Correctly()
        {
            var word = "combinded_process".AsUpperCamelCase();
            var result = "combindedprocess";

            var actual = word.SingularizeWord().ToLowerInvariant();

            Assert.Equal(result, actual);
        }


        [Fact]
        public void Companies_Should_Singularize_As_Company()
        {
            var word = "companies";
            var result = "company";

            var actual = word.SingularizeWord().ToLowerInvariant();

            Assert.Equal(result, actual);
        }

        [Fact]
        public void Children_Should_Singularize_As_Child()
        {
            var word = "children";
            var result = "child";

            var actual = word.SingularizeWord().ToLowerInvariant();

            Assert.Equal(result, actual);
        }

        [Fact]
        public void Attendees_Should_Singularize_As_Attendee()
        {
            var word = "attendees";
            var result = "attendee";

            var actual = word.SingularizeWord().ToLowerInvariant();

            Assert.Equal(result, actual);
        }

        [Fact]
        public void Company_Should_Singularize_As_Company()
        {
            var word = "company";
            var result = "company";

            var actual = word.SingularizeWord().ToLowerInvariant();

            Assert.Equal(result, actual);
        }

        [Fact]
        public void Should_Write_To_File()
        {
            var word = "company";
            var result = "company";
            

            var actual = word.SingularizeWord().ToLowerInvariant();

            Assert.Equal(result, actual);
        }

        [Fact]
        public void Empty_Input_for_Multiword_Should_return_false()
        {
            string word = null;
            var result = false;


            var actual = word.IsMultiWord();
            Assert.NotNull(actual);
            Assert.Equal(result, actual);
        }

        [Fact]
        public void Empty_Input_for_CleanInput_Should_return_Empty()
        {
            string word = null;
            var result = "";


            var actual = word.CleanInput();
            Assert.NotNull(actual);
            Assert.Equal(result, actual);
        }

        [Fact(DisplayName = "Abbrevation should not return a forbidden text if a list of forbidden abbrevation is provided")]
        public void Text_Should_Not_Be_Abbrevated_as_AS()
        {
            var forbidden = new List<string> { "as", "from" };
            var input = "ApplicationStatus";
            var test = input.Abrevation(forbidden);
            var expected = "as0";

            Assert.Equal(expected, test);
        }

    }
}