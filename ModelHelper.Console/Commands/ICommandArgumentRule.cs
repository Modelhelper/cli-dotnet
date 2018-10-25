using System.Collections.Generic;
using ModelHelper.Core.Rules;

namespace ModelHelper.Commands
{
    public interface ICommandArgumentRule : IRule<Dictionary<string, string>>
    {
        List<CommandArgument> ValidArguments { get;set; }
    }
}