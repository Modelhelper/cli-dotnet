using System.Collections.Generic;
using System.IO;
using ModelHelper.Core.Drops;
using DotLiquid;

namespace ModelHelper.Core.Extensions
{
    public class SqlColumnToPropertyList : DotLiquid.Tag
    {
        private int _max;
        private string _sqlCommand = "select";

        public override void Initialize(string tagName, string markup, List<string> tokens)
        {
            base.Initialize(tagName, markup, tokens);
            _sqlCommand = string.IsNullOrEmpty(markup) ? "select" : markup;
        }

        public override void Render(Context context, TextWriter result)
        {
            if (context["model"] is ModelDrop drop )
            {
                
                var alias = drop.Table.Alias;

                switch (_sqlCommand.ToLower())
                {
                    case "insert":
                        result.Write(drop.Table.Columns.ColumnsForInsert());
                        break;
                    case "update":
                        result.Write(drop.Table.Columns.ColumnsForUpdate());
                        break;
                    case "delete":
                        result.Write(drop.Table.Columns.ColumnsForSelect(alias));
                        break;
                    default:
                        result.Write(drop.Table.Columns.ColumnsForSelect(alias));
                        break;
                }
                
            }            
        }

        

        
    }
}