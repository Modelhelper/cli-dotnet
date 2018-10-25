using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ModelHelper.Core.Database;
using ModelHelper.Core.Drops;
using ModelHelper.Core.Extensions;
using ModelHelper.Core.Models;
using ModelHelper.Extensibility;
using Xunit;

namespace ModelHelper.Tests
{
    public class SqlTemplateTests
    {
        [Fact]
        public async Task A_Table_With_Columns_SHould_Return_Correct_Insert_Statement()
        {
            var builder = new StringBuilder();
            builder.AppendLine("INSERT INTO [dbo].[Test] (");
            builder.AppendLine("\t  [Test1]");
            builder.AppendLine("\t, [Test2]");
            builder.AppendLine(") VALUES (");
            builder.AppendLine("\t  @Test1");
            builder.AppendLine("\t, @Test2");
            builder.Append(")");

            
            var table = new Table{Schema = "dbo", Name = "Test", Columns = new List<IColumn>
            {
                new Column{Name = "Test1", PropertyName = "Test1"},
                new Column{Name = "Test2", PropertyName = "Test2"},
            }};

            var drop = new TableDrop(table);
            var result = drop.SqlForInsert();
            var expected = builder.ToString();
            Assert.Equal(expected, result);
        }
            
    }
}