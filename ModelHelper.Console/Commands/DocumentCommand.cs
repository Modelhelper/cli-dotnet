using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using ModelHelper.Core.Rules;

namespace ModelHelper.Commands
{
    [Export(typeof(ICommand))]
    [ExportMetadata("Key", "document")]
    public class DocumentCommand : BaseCommand
    {
        public DocumentCommand()
        {
            Key = "document";
            Alias = "doc";
            IsPublic = false;
        }

        public override bool EvaluateArguments(IRuleEvaluator<Dictionary<string, string>> evaluator)
        {
            return true;
        }

        public override void Execute(Core.ApplicationContext context)
        {

            Console.WriteLine("Document Command");
        }

    }
}