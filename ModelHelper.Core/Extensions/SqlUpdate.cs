using System;
using System.Collections.Generic;
using System.IO;
using ModelHelper.Core.Drops;
using DotLiquid;

namespace ModelHelper.Core.Extensions
{
    public class SqlUpdate : Tag
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
                var table = isIndex ? drop.Table.ChildRelations[index] as ITableDrop : drop.Table;
                result.Write(table.SqlForUpdate());

            }
        }
    }
}