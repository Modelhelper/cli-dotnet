using System.ComponentModel.Composition;
using ModelHelper.Extensibility;

namespace ModelHelper.Data
{
    [Export(typeof(ISqlGenerator))]    
    public class SelectOneSqlGenerator : ISqlGenerator
    {
        public string Database { get; } = "mssql";
        public SqlMethod Method { get; } = SqlMethod.SelectOne;

        public string Generate(IEntity table, bool includeRelations = false)
        {
            return "i will generate 'select * from a table where someid = @somevalue' sql";
        }
    }
}