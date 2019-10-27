using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using ModelHelper.Core.CommandLine;
using ModelHelper.Core.Rules;

namespace ModelHelper.Commands
{
    [Export(typeof(ICommand))]
    [ExportMetadata("Key", "migration")]
    public class MigrationCommand : BaseCommand
    {

        [Option(Key = "--source", IsRequired = true, Aliases = new[] { "-src" })]
        public string Source { get; set; }

        [Option(Key = "--destination", IsRequired = true, Aliases = new[] { "-dest", "-d" })]
        public string Destination { get; set; }

        [Option(Key = "--script", IsRequired = true, Aliases = new[] { "-s"})]
        public string ScriptPath { get; set; }

        public MigrationCommand()
        {
            Key = "migration";
            Alias = "m";
            IsPublic = false;
        }

        public override bool EvaluateArguments(IRuleEvaluator<Dictionary<string, string>> evaluator)
        {
            return true;
        }

        public override void Execute(Core.ApplicationContext context)
        {
            var map = ArgumentParser.Parse(this, context.Options);
           
            Console.WriteLine("Migration Command");
            Console.WriteLine($"Source: {Source}");
            Console.WriteLine($"Destination: {Destination}");
            Console.WriteLine($"Script: {ScriptPath}");
        }

    }
}