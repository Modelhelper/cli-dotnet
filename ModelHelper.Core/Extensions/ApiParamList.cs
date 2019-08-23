using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ModelHelper.Core.Drops;
using DotLiquid;
using ModelHelper.Core.CommandLine;

namespace ModelHelper.Core.Extensions
{
    public class ApiParamList : DotLiquid.Tag
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

    public class Snippet : DotLiquid.Tag
    {
        string _snippetName;
        public override void Initialize(string tagName, string markup, List<string> tokens)
        {
            base.Initialize(tagName, markup, tokens);

            if (!string.IsNullOrEmpty(markup))
            {
                _snippetName = markup.ToLower().Trim();
            }
        }

        public override void Render(Context context, TextWriter result)
        {

            var snippet = ModelHelperConfig.Templates.FirstOrDefault(t =>
                t.Name.Equals(_snippetName, StringComparison.InvariantCultureIgnoreCase));

            if (snippet != null)
            {
                var snippetContent = snippet.Body;
                //var stringBuilder = new StringBuilder();
                //var table = context.model.SelectedTable.
                if (context["model"] is ModelDrop drop)
                {
                    var resultSnippetCode = snippetContent.Render(drop.TemplateModel);
                    result.Write(resultSnippetCode);
                }
            }
            else
            {
                result.Write("");
            }
            
            //result.Write(stringBuilder.ToString());
        }
    }
}