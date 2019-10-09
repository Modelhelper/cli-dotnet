using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using ModelHelper.Commands;
using ModelHelper.Core.CommandLine;
using ModelHelper.Core.Extensions;
using ModelHelper.Core.Help;
using ModelHelper.Core.Project;
using ModelHelper.Core.Project.V1;
using ModelHelper.Extensibility;
using ModelHelper.Extensions;

namespace ModelHelper
{
    [Export(typeof(ICommandExecutor))]
    public class CommandExecutor : ICommandExecutor
    {
        [ImportMany]
        IEnumerable<Lazy<ICommand>> _commands;

        

        public void Execute(string key, List<string> args)
        {
            
            var command = _commands?.FirstOrDefault(c => c.Value.Key == key || c.Value.Alias == key);
            var projectReader = new DefaultProjectReader();
            var projectPath = Path.Combine(Directory.GetCurrentDirectory(), ".model-helper");

            if (command != null)
            {
                //var options = args.AsArgumentDictionary();
                var showHelp = args.Contains("-h") || args.Contains("--help");
                try
                {
                    if (!showHelp)
                    {
                        if (File.Exists(projectPath))
                        {
                            var version = projectReader.CheckVersion(projectPath);
                            var runUpgrade = version != null && version.MustUpdate;

                            if (runUpgrade)
                            {
                                var upgradeMessage = $@"
The current project file is too old and needs to be upgraded.
Please run the following command to upgrade (use ctrl + v to paste the command)";

                                upgradeMessage.WriteConsoleWarning();
                                var commandPrompt = "mh project --upgrade";
                                commandPrompt.WriteConsoleCommand();
                                ModelHelperExtensions.ToClipboard(commandPrompt);

                                Console.WriteLine("\n\n...or...");
                                Console.Write("Upgrade right away by answering Y (default is no) [y/N]: ");
                                var doUpgrade = Console.ReadLine();

                                if (!string.IsNullOrEmpty(doUpgrade) && doUpgrade.Equals("y", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    var upgradeResult = ProjectFactory.Upgrade(projectPath, version);

                                    if (upgradeResult.Converted)
                                    {
                                        $"The project was successfully upgraded".WriteConsoleSuccess();
                                    }
                                }

                                
                            }
                        }

                        command.Value.Execute(args);
                    }
                    else
                    {
                        var t = command.Value.GetType();
                        //Console.WriteLine(t.ToString());
                        Documentation.FromAttributes(t).WriteToConsole();
                        
                        //command.Value.Help.WriteToConsole();

                    }
                    
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);                    
                }
                
            }
            else
            {
                Console.WriteLine($"The '{key}' is not a valid command. Run mh help for a list of valid commands and arguments");
            }


            CommandExtensions.WriteSlogan();
        }
    }
}