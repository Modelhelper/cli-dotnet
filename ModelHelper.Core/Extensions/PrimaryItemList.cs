﻿using System.Collections.Generic;
using System.IO;
using System.Text;
using DotLiquid;
using ModelHelper.Core.Drops;

namespace ModelHelper.Core.Extensions
{
    public class PrimaryItemList : DotLiquid.Tag
    {
        private string _language = string.Empty;

        public override void Initialize(string tagName, string markup, List<string> tokens)
        {
            base.Initialize(tagName, markup, tokens);

            if (!string.IsNullOrEmpty(markup))
            {
                _language = markup.ToLower().Trim();
            }
        }

        public override void Render(Context context, TextWriter result)
        {

            var stringBuilder = new StringBuilder();
            //var table = context.model.SelectedTable.
            if (context["model"] is ModelDrop drop)
            {
                result.Write(drop.Table.PrimaryKeys.PrimaryKeyList(_language));
            }
            result.Write(stringBuilder.ToString());
        }
    }
}