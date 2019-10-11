using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using ModelHelper.Core.Rules;
using ModelHelper.Extensions;

namespace ModelHelper.Commands
{
    [Export(typeof(ICommand))]    
    public class HelpCommand : BaseCommand
    {
        [ImportMany]
        IEnumerable<Lazy<ICommand>> _commands;
        public HelpCommand()
        {
            Key = "help";
            Alias = "h";
        }

        public override void Execute(List<string> arguments)
        {            
            int cmdLen = 15;
            int descLen = 80;

            var options = arguments; //.AsArgumentDictionary();

            if (options == null || options.Count == 0)
            {
                $"Help".WriteConsoleTitle();
                "Valid Commands".WriteConsoleSubTitle();

                var validCommands = _commands.Where(c => c.Value.IsPublic)
                    .Select(c => new {Command = c.Value.Key, Description = c.Value.Help?.ShortDescription ?? ""})
                    .OrderBy(c => c.Command).ToList();

                validCommands.ToConsoleTable().WriteToConsole();
                
            }
            else
            {

                var cmd = options.FirstOrDefault();
                // 
                var helpItems = HelpFactory.Create();

                var helpItem = helpItems.FirstOrDefault(c =>
                    !string.IsNullOrEmpty(c.Key) && c.Key.Equals(cmd, StringComparison.InvariantCultureIgnoreCase));

                if (helpItem != null)
                {
                    helpItem.WriteToConsole();
                    
                }
                else
                {
                    var command = _commands.FirstOrDefault(c =>
                        !string.IsNullOrEmpty(c.Value.Key) &&
                        c.Value.Key.Equals(cmd, StringComparison.InvariantCultureIgnoreCase));

                    command?.Value.Help.WriteToConsole();
                }
                
                
            }

            Console.WriteLine("\n");
        }

        public override bool EvaluateArguments(IRuleEvaluator<Dictionary<string, string>> evaluator)
        {
            return true;
        }
    }
}