using System.ComponentModel.Composition;
using ModelHelper.Extensibility;

namespace ModelHelper.Data
{
    [Export(typeof(ISqlGenerator))]   
    public class CreateTableSqlGenerator : ISqlGenerator
    {
        public string Database { get; } = "msql";
        public SqlMethod Method { get; } = SqlMethod.CreateTable;

        public string Generate(IEntity table, bool includeRelations = false)
        {
            return "i will generate 'create table' sql";
        }
    }
}