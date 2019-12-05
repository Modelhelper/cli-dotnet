using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ModelHelper.UnitTests
{
    public class filter_tests
    {
        [Theory]
        [Xunit.InlineData("SnakeCase", "snake_case")]
        public void Given_text_should_render_snake_case(string input, string expected)
        {
            
            Assert.Equal(expected, input);
        }
    }
}
