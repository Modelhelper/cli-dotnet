using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using ModelHelper.Core.Extensions;
using ModelHelper.Core.Rules;
using ModelHelper.Core;
using ModelHelper.Core.Database;
using ModelHelper.Core.Extensions;
using ModelHelper.Core.Models;
using ModelHelper.Core.Project;
using ModelHelper.Core.Templates;
using ModelHelper.Data;
using ModelHelper.Extensions;
using DotLiquid.Util;
using ModelHelper.Core.Rules;
using Newtonsoft.Json;
using ModelHelper.Commands;
using ModelHelper.Core.Help;
using System.IO;
using System.Reflection;

namespace ModelHelper.Commands
{
    [Export(typeof(ICommand))]
    [ExportMetadata("Key", "init")]    
    public class InitCommand : BaseCommand
    {
        public InitCommand()
        {
            Key = "init";
            IsPublic = false;
        }

        public override bool EvaluateArguments(IRuleEvaluator<Dictionary<string, string>> evaluator)
        {
            return true;
        }

        public override void Execute(List<string> arguments)
        {
            var modelHelperLocal = ModelHelperExtensions.RootFolder;
            ConsoleExtensions.WriteConsoleLogo();
            ConsoleExtensions.WriteConsoleTitle(Help.GetMessage(Constants.HELP_INIT_TITLE));
            Console.WriteLine(Help.GetMessage(Constants.HELP_INIT_WELCOME));

            Console.Write(Help.GetMessage("runSetup"));
            var result = Console.ReadLine();


            
            var emptyArgs = new List<string>();

            if (string.IsNullOrEmpty(result) || result.ToLower() == "y")
            {
                
                ModelHelperExtensions.CreateRootDirectory();

                ModelHelperConfig.ReadConfig();

                // add to PATH
                Console.Write("Add the path to this application in %PATH% variable? (Y/n): ");
                var pathAnswer = Console.ReadLine();
                var addToPath = string.IsNullOrEmpty(pathAnswer) || pathAnswer.ToLowerInvariant() == "y";

                if (addToPath)
                {
                    AddToPathVariable();
                }
                //download templates?

                Console.Write($"Fetch templates from {ModelHelperConfig.RemoteTemplateLocation} (Y/n)? ");
                var downloadTemplateAnswer = Console.ReadLine();
                var downloadTemplates = string.IsNullOrEmpty(downloadTemplateAnswer) || downloadTemplateAnswer.ToLowerInvariant() == "y";

                if (downloadTemplates)
                {
                    GetTemplates(modelHelperLocal);
                }


                //ListTemplates();

                //ListCommands();
            }

            
        }

        

        internal void ListCommands()
        {
            Console.Write(Help.GetMessage("showValidCommands"));
            var helpResult = Console.ReadLine();

            if (string.IsNullOrEmpty(helpResult) || helpResult.ToLower() == "y")
            {
                var helpCommand = new HelpCommand();
                helpCommand.Execute(new List<string>());
            }

        }

        internal void GetTemplates(string rootFolder)
        {
            var templateCount = ModelHelperExtensions.FetchRemoteTemplates(ModelHelperConfig.RemoteTemplateLocation,
                    Path.Combine(rootFolder, "templates"), true,
                    (index, total) => ConsoleExtensions.ShowPercentProgress(Help.GetMessage("downloadTemplate"), index, total));

            var afterInstall = Help.GetMessage("afterInstall", rootFolder, templateCount.ToString());

            Console.WriteLine(afterInstall);

        }
        internal void ListTemplates()
        {
            Console.Write(Help.GetMessage("listTemplates"));
            var hemplateResult = Console.ReadLine();

            if (string.IsNullOrEmpty(hemplateResult) || hemplateResult.ToLower() == "y")
            {
                var templateCommand = new TemplateCommand();
                templateCommand.Execute(new List<string>());
            }
        }
        internal void AddToPathVariable()
        {
            const string name = "PATH";
            var target = EnvironmentVariableTarget.User;
            var myPath = new FileInfo(Assembly.GetExecutingAssembly().Location);
            var pathvar = System.Environment.GetEnvironmentVariable(name, target);
            var isInPath = pathvar.Contains(myPath.DirectoryName);

            if (!isInPath)
            {
                                
                var value = pathvar + $";{myPath.DirectoryName}";
                System.Environment.SetEnvironmentVariable(name, value, target);
                Console.WriteLine(myPath.DirectoryName + " added to current user %PATH% environment variable");
            }
        }
        internal void CreateConfiguration()
        {

        }
    }

   
}