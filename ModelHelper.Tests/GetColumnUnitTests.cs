using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ModelHelper.Core.Models;
using ModelHelper.Core.Project;
using ModelHelper.Core.Project.V1;
using Xunit;
using ModelHelper.Data;


namespace ModelHelper.Tests
{
    public class GetColumnUnitTests
    {
        string connectionString = @"Server =.\SQLEXPRESS;Initial Catalog = ModelHelperTests; Persist Security Info=False;Trusted_Connection=true;Connection Timeout = 30;";

        [Fact]
        public async Task A_Database_Returns_Tables()
        {
            var table = "clients";
            var project = new ProjectV1();
            var repo = new ModelHelper.Data.SqlServerRepository(connectionString, project);
            var items = await repo.GetTables();

            Assert.True(items.Any());
        }

        [Fact]
        public async Task A_Table_Returns_Columns()
        {
            var table = "NoPrimaryKeys";
            var project = new ProjectV1();
            var repo = new ModelHelper.Data.SqlServerRepository(connectionString, project);
            var items = await repo.GetColumns("dbo", table);

            Assert.True(items.Any());
        }

        [Fact]
        public async Task An_Empty_Ignored_Columns_Creates_A_Union_List_of_One()
        {
            
            var project = new ProjectV1();
            var repo = new ModelHelper.Data.SqlServerRepository(connectionString, project);

            var actual = repo.GetUnionList("select Name = '{0}'", new List<string>());
            var expected = "select Name = 'HUMPYBUMPYDUMP'";

            Assert.Equal(expected, actual);

        }

        [Fact]
        public async Task An_Ignored_Columns_Creates_A_Matching_Union_List()
        {

            var project = new ProjectV1();
            var repo = new ModelHelper.Data.SqlServerRepository(connectionString, project);

            var actual = repo.GetUnionList("select Name = '{0}'", new List<string>{"CreatedBy", "ModifiedBy"});
            var expected = @"select Name = 'CreatedBy' union 
select Name = 'ModifiedBy'";

            Assert.Equal(expected, actual);

        }
    }
}