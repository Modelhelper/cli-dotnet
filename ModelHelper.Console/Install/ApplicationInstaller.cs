using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using ModelHelper.Core.Extensions;
using ModelHelper.Core.Rules;
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
using ModelHelper.Core.Configuration;
using ModelHelper.Core;
using System.IO.Compression;
using ModelHelper.Core.Remote;
using ShellProgressBar;

namespace ModelHelper.Commands
{
    //[Export(typeof(ICommand))]
    //[ExportMetadata("Key", "install")]    
    public class ApplicationInstaller //:BaseCommand
    {
        private HelpItem _help;
        public ApplicationInstaller()
        {
            //Key = "install";
            //IsPublic = false;
        }

        

        public void Run()
        {
            _help = HelpFactory.Init();

            //var modelHelperLocal = ModelHelperExtensions.RootFolder;
            ConsoleExtensions.WriteConsoleLogo();
            ConsoleExtensions.WriteConsoleTitle(_help.GetMessage(Constants.HELP_INIT_TITLE));
            Console.WriteLine(_help.GetMessage(Constants.HELP_INIT_WELCOME));

            Console.Write(_help.GetMessage("runSetup"));
            var result = Console.ReadLine();



            
            //var emptyArgs = new List<string>();

            if (string.IsNullOrEmpty(result) || result.ToLower() == "y")
            {
                // gather information

                var shared = new ConfigLocation();
                var taskCount = 3;

                if (SetupSharedLocations())
                {
                    $"Shared locations? ".WriteConsoleSubTitle();

                    shared.Template = SharedTemplateLocation();
                    shared.ProjectDefinitions = SharedProjectsDefinitions();
                    shared.CodeDefinitions = SharedCodeDefinitions();
                }

                var addToPath = AddPathToEnvironmental();

                if (addToPath) taskCount++;

                var options = new ProgressBarOptions
                {
                    // ProgressCharacter = '─',
                    ProgressBarOnBottom = true,
                    ForegroundColor = ConsoleColor.Yellow,
                    ForegroundColorDone = ConsoleColor.DarkGreen,
                    BackgroundColor = ConsoleColor.DarkGray,
                    BackgroundCharacter = '\u2593'
                };


                

                    using (var pbar = new ProgressBar(taskCount, "Update tasks", options))
                    {
                        var tick = 0;
                        pbar.Tick($"Task {tick += 1} of {taskCount}: Create Config File");
                    //1 config file

                    var config = ModelHelperSetup
                     .CreateRootDirectory()
                     .CreateConfigFile(shared)
                     .SaveConfigFile();

                    try
                    {
                        $"Download templates from {config.RemoteTemplateDownloadUrl}".WriteConsoleWarning();
                        var uri = new Uri(config.RemoteTemplateDownloadUrl);
                        Download
                            .Templates(uri, (int progress) =>
                            {

                                //Console.WriteLine(progress);
                            }, (DownloadInfo info) =>
                            {
                                pbar.Tick($"Task {tick += 1} of {taskCount}: Download and extract templates");                                
                                info.ExtractFiles(config.Global.Template);
                            });



                        //Download.CodeDefinitions(config).ExtractFiles();
                        //Download.ProjectDefinitions(config).ExtractFiles();
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Could not get global files");
                    }

                    if (addToPath)
                    {
                        AddToPathVariable();
                        pbar.Tick($"Task {tick += 1} of {taskCount}: Download and extract templates");
                    }

                    pbar.Tick($"Task {tick += 1} of {taskCount}: Installation Complete");
                }



                // 2 - 4 download stuff

                // 5 add to env
                // finish up




            }

            
        }

        //private void AddDefsTo
        

        private bool AddPathToEnvironmental()
        {
            Console.Write("Add the path to this application in %PATH% variable? (Y/n): ");
            var pathAnswer = Console.ReadLine();
            return string.IsNullOrEmpty(pathAnswer) || pathAnswer.ToLowerInvariant() == "y";
        }

        private bool SetupSharedLocations()
        {
            Console.Write("Setup shared locations? [y/N]");
            var setupShared = Console.ReadLine();

            return setupShared.Equals("y", StringComparison.InvariantCultureIgnoreCase);
        }
        
        private string SharedTemplateLocation()
        {
            

            Console.Write($"\nTemplate Location");
            var location = Console.ReadLine();
            if (!string.IsNullOrEmpty(location))
                return location;
            else
                return Core.Constants.UNSET;

        }

        private string SharedCodeDefinitions()
        {
            Console.Write($"\nCode Definitions Location");
            var location = Console.ReadLine();
            if (!string.IsNullOrEmpty(location))
                return location;
            else
                return Core.Constants.UNSET;
        }

        private string SharedProjectsDefinitions()
        {
            Console.Write($"\nProject Definitions Location");
            var location = Console.ReadLine();
            if (!string.IsNullOrEmpty(location))
                return location;
            else
                return Core.Constants.UNSET;
        }

        internal void ListCommands()
        {
            Console.Write(_help.GetMessage("showValidCommands"));
            var helpResult = Console.ReadLine();

            if (string.IsNullOrEmpty(helpResult) || helpResult.ToLower() == "y")
            {
                var helpCommand = new HelpCommand();
                helpCommand.Execute(null);
            }

        }

        

        //internal void GetTemplates(string rootFolder)
        //{
        //    var templateCount = ModelHelperExtensions.FetchRemoteTemplates(ModelHelperConfig.RemoteTemplateLocation,
        //            Path.Combine(rootFolder, "templates"), true,
        //            (index, total) => ConsoleExtensions.ShowPercentProgress(Help.GetMessage("downloadTemplate"), index, total));

        //    var afterInstall = Help.GetMessage("afterInstall", rootFolder, templateCount.ToString());

        //    Console.WriteLine(afterInstall);

        //}
        internal void ListTemplates()
        {
            Console.Write(_help.GetMessage("listTemplates"));
            var hemplateResult = Console.ReadLine();

            if (string.IsNullOrEmpty(hemplateResult) || hemplateResult.ToLower() == "y")
            {
                var templateCommand = new TemplateCommand();
                templateCommand.Execute(null);
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
        
    }

    
}