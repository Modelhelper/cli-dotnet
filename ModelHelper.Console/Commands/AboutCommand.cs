﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Reflection;
using ModelHelper.Core.Configuration;
using ModelHelper.Core.Extensions;
using ModelHelper.Core.Rules;
using ModelHelper.Extensions;

namespace ModelHelper.Commands
{
    [Export(typeof(ICommand))]
    [ExportMetadata("Key", "about")]
    [ExportMetadata("Alias", "a")]
    public class AboutCommand : BaseCommand
    {
        public AboutCommand()
        {
            Key = "about";
            IsPublic = false;
        }

        public override bool EvaluateArguments(IRuleEvaluator<Dictionary<string, string>> evaluator)
        {
            return true;
        }

        public override void Execute(Core.ApplicationContext context)
        {
            Assembly execAssembly = Assembly.GetCallingAssembly();

            AssemblyName name = execAssembly.GetName();

            ConsoleExtensions.WriteConsoleLogo();            
           
            Console.WriteLine($"Version: \t\t{name.Version.Major.ToString()}.{name.Version.Minor.ToString()}"); //  for .Net ({execAssembly.ImageRuntimeVersion})
            Console.WriteLine($"App name: \t\t{name.Name}.exe");
            Console.WriteLine($"Location: \t\t{Assembly.GetExecutingAssembly().Location}");
            Console.WriteLine($"Configuration: \t\t{ModelHelperExtensions.RootFolder}");
            Console.WriteLine($"Templates: \t\t{ModelHelperConfig.TemplateLocation}");
            
            //Console.WriteLine($"Remote Templates: \t\t{ModelHelperConfig.RemoteTemplateLocation}");

            Console.WriteLine("");
            CommandExtensions.PrintProjectInfo();
            Console.WriteLine("");            

            CheckForNewVersion(execAssembly);            

            

                    }


        internal void CheckForNewVersion(Assembly app)
        {
            var currentApp = new FileInfo(app.Location);
            var remoteLocation = ModelHelperConfig.RemoteBinaryLocation;

            if (Directory.Exists(remoteLocation))
            {
                var remoteApp = new FileInfo(Path.Combine(remoteLocation, "mh.exe"));

                if (remoteApp.Exists && currentApp.Exists && remoteApp.LastWriteTime > currentApp.LastWriteTime)
                {
                    ConsoleExtensions.WriteConsoleWarning($"A newer mh.exe exists here: {remoteLocation}");
                    
                }
            }
        }
    }
}