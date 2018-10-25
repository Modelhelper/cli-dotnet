using System;
using System.Collections.Generic;
using System.IO;
using ModelHelper.Core.Drops;
using DotLiquid;
using ModelHelper.Extensibility;
using System.ComponentModel.Composition;
using System.Linq;

namespace ModelHelper.Core.Extensions
{
    public class SqlSelectSingle : Tag
    {
        
        private string _markup;
        public override void Initialize(string tagName, string markup, List<string> tokens)
        {
            base.Initialize(tagName, markup, tokens);
            _markup = markup.Trim();
        }

        public override void Render(Context context, TextWriter result)
        {
            int index = 0;
            var isIndex = !string.IsNullOrEmpty(_markup)
                ? Int32.TryParse(context[_markup].ToString(), out index)
                : false;

            if (context["model"] is ModelDrop drop)
            {
                var source = drop.Database.DataSource.ToLowerInvariant();
                var method = SqlMethod.SelectOne;
                ISqlGenerator generator = null;

                if (drop.SqlScriptGenerators != null && drop.SqlScriptGenerators.Any())
                {
                    generator = drop.SqlScriptGenerators.FirstOrDefault(g =>
                        g.Database.ToLowerInvariant() == source && g.Method == method);
                }
                
                if (generator != null)
                {
                    var data = generator.Generate(null);
                    result.Write(data);
                }
                else
                {
                    var table = drop.Table;                
                    result.Write(table.SqlForSelectFromPrimaryKeys());
                    //var table = isIndex ? drop.Table.ChildRelations[index]  as TableDrop: drop.Table;

                }
                
                

            }
        }
    }
}