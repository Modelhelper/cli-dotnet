using System.Collections.Generic;
using System.Threading.Tasks;

namespace ModelHelper.Core.Commands
{
    public interface ICommand
    {
        string Name { get; }
        string Alias { get;  }
        
        bool IsPublic { get; }
        Task<CommandResult> ExecuteAsync(CommandContext context);
        List<ICommand> Commands { get; }
    }

    public class CommandResult
    {

    }

    public class CommandContext
    {
        
    }
}