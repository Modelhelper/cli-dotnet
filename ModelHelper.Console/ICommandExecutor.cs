using System.Collections.Generic;

namespace ModelHelper
{
    public interface ICommandExecutor
    {
        void Execute(Core.ApplicationContext context);
    }
}