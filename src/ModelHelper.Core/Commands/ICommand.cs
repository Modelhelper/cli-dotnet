using System.Collections.Generic;
using System.Threading.Tasks;
using ModelHelper.Core.Project;

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
        public Project3 CurrentProject { get; set; }
        public string Commands { get; set; }
        public string Options { get; set; }
    }
}