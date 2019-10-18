using ModelHelper.Core.Configuration;
using ModelHelper.Core.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelHelper.Core.Templates
{
    public enum Scope
    {
        Global,
        Shared,
        Project
    }
    public static class TemplateFactory
    {
        public static List<ITemplate> LoadTemplates(GlobalConfig config, IProject currentProject = null)
        {
            return new List<ITemplate>();
        }

        //public static ITemplate Clone(Scope from, Scope to, string templateName)
        //{
        //    // cannot clone to Global
        //    if (to == Scope.Global)
        //    {
        //        throw new Exception("Cannot clone to the Global scope");
        //    }



        //    return null;
        //}
    }
}
