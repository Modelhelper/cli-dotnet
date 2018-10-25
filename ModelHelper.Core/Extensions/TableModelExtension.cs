using System;
using System.Linq;
using System.Text;
using DotLiquid;
using ModelHelper.Core.Database;
using ModelHelper.Core.Drops;
using ModelHelper.Core.Templates;

namespace ModelHelper.Core.Extensions
{
    public static class TableModelExtension
    {
        public static Hash CreateDrop(this ITemplateModel model)
        {
            var modelDrop = new ModelDrop(model);
            return Hash.FromAnonymousObject(new
            {
                model = modelDrop
            });
        }       
    }
}