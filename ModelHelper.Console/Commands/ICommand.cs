using System.Collections.Generic;
using ModelHelper.Core.Help;
using ModelHelper.Core.Rules;

namespace ModelHelper.Commands
{
    public interface ICommand
    {
       
        string Key { get; set; }
        string Alias { get; set; }
        
        bool IsPublic { get; set; }

        HelpItem Help { get;set; }

        List<CommandArgument> ValidArguments { get; set; }
        bool EvaluateArguments(IRuleEvaluator<Dictionary<string, string>> evaluator);
        void Execute(Core.ApplicationContext context);
    }

    /*
     * commands.Add(New());
            
            
            commands.Add(Version());
            
            commands.Add(Script());
       
     */
}