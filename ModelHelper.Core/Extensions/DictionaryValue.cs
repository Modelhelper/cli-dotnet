using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using DotLiquid;
using ModelHelper.Core.Drops;

namespace ModelHelper.Core.Extensions
{
    public class DictionaryValue : DotLiquid.Tag
    {
        private string _input;

        public override void Initialize(string tagName, string markup, List<string> tokens)
        {
            base.Initialize(tagName, markup, tokens);
            _input = markup; //!string.IsNullOrEmpty(markup) ? markup.Trim() : string.Empty;
        }

        public override void Render(Context context, TextWriter result)
        {
            if (context["model"] is ModelDrop drop)
            {
                
                var output = _input.Trim();
                var key = _input.Trim().ToLowerInvariant();
                if (drop.Dictionary != null && drop.Dictionary.ContainsKey(key))
                {
                    output = drop.Dictionary[key];
                }

                result.Write(output);
                
            }
        }




    }
}