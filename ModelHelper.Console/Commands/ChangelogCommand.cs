using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using ModelHelper.Core.Rules;

namespace ModelHelper.Commands
{
    [Export(typeof(ICommand))]
    [ExportMetadata("Key", "changelog")]    
    public class ChangelogCommand : BaseCommand
    {
        public ChangelogCommand()
        {
            Key = "changelog";
            Alias = "cl";
            IsPublic = false;
        }

        public override bool EvaluateArguments(IRuleEvaluator<Dictionary<string, string>> evaluator)
        {
            return true;
        }

        public override void Execute(Core.ApplicationContext context)
        {

            Console.WriteLine("Changelog Command");
        }

    }
}