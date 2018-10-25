using System.Collections.Generic;
using System.IO;
using ModelHelper.Core.Drops;
using DotLiquid;

namespace ModelHelper.Core.Extensions
{
    public class PropertyList : DotLiquid.Tag
    {
        private int _max;
        private string _itemOwner = "item";

        public override void Initialize(string tagName, string markup, List<string> tokens)
        {
            base.Initialize(tagName, markup, tokens);
            _itemOwner = string.IsNullOrEmpty(markup) ? "item" : markup;
        }

        public override void Render(Context context, TextWriter result)
        {
            if (context["model"] is ModelDrop drop)
            {
                
                result.Write(drop.Table.Columns.PropertyList(_itemOwner));                

            }
        }




    }
}