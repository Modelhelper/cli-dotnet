using System.Collections.Generic;
using Xunit;

namespace ModelHelper.Tests
{
    
    public class ConsoleCommandTests
    {
        [Fact]
        public void ListOfClassShouldReturnValidConsoleTable()
        {
            var list = new List<TestClass>
            {
                new TestClass {Key = "key1", Value = "Value1"},
                new TestClass {Key = "key2", Value = "Value2"},
                new TestClass {Key = "key3", Value = "Value3"},

            };

            //var result = list.ToConsoleTable()
        }

        [Fact]        
        public void Argument_List_Should_Return_valid_command_list()
        {
            Assert.True(true);
        }

        /*
         [TestCase(1,1)]
         [TestCase(1,2)]
         [TestCase(2,2)]
         [TestCase(1,3)]
         [TestCase(2,1)]
         public void Should_return_False(int param1, int param2)
            Assert.False(param1,param2);
         */
    }

    public class TestClass
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }

    public class ProjectConverterTests
    {
        [Fact]
        public void Beta1ProjectShouldGiveCorrectProjectSetup()
        {
            
        }
    }
}
