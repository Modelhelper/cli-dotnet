using System.Collections.Generic;
using Xunit;

namespace ModelHelper.Tests
{
    public class TableExtensionsTests
    {
        [Fact]
        public void Table_With_Two_PK_Should_Render_Signature_With_Both()
        {
            var pk = new Dictionary<string, string>
            {
                {"id1", "int" },
                {"id2", "int" }
            };

            //var signature = TableModelExtensions.
        }
    }
}