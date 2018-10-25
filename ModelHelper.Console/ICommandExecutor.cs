using System.Collections.Generic;

namespace ModelHelper
{
    public interface ICommandExecutor
    {
        void Execute(string key, List<string> args);
    }
}