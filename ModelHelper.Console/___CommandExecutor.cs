using System;
using System.Collections.Generic;
using System.Linq;

namespace ModelHelper
{
    //public class CommandExecutor
    //{
    //    private string _projectFile;
    //    private List<string> _args;
    //    public CommandExecutor(string[] args)
    //    {
    //        _args = args.ToList();
    //        Commands = CommandFactory.Create();
    //    }

    //    public void Execute()
    //    {
    //        if (_args != null && _args.Any())
    //        {
    //            var arg = _args[0];
    //            var command = Commands.FirstOrDefault(c => c.Key == arg);

    //            if (command != null)
    //            {
    //                var arguments = new List<string>(_args.GetRange(1, _args.Count-1)).ToArray();
    //                command.Execute(arguments);
    //            }
    //            else
    //            {
    //                Console.WriteLine($"The '{arg}' is not a valid command. Run mh help for a list of valid commands and arguments");
    //            }
    //        }
    //        else
    //        {
    //            var c = CommandFactory.About();
    //            c.Execute(null);
    //        }
    //    }


    //    public List<Command> Commands { get; set; }
    //}
}