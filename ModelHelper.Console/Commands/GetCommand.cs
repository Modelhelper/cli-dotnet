using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Reflection;
using ModelHelper.Core.CommandLine;
using ModelHelper.Core.Configuration;
using ModelHelper.Core.Extensions;
using ModelHelper.Core.Remote;
using ModelHelper.Core.Rules;
using ModelHelper.Extensions;
using ShellProgressBar;

namespace ModelHelper.Commands
{
    [Export(typeof(ICommand))]
    [ExportMetadata("Key", "get")] 
    public class GetCommand : BaseCommand
    {
        public GetCommand()
        {
            Key = "get";
            IsPublic = true;
        }

        public override bool EvaluateArguments(IRuleEvaluator<Dictionary<string, string>> evaluator)
        {
            return true;
        }

        public override void Execute(Core.ApplicationContext context)
        {
            var map = ArgumentParser.Parse(this, context.Options);

            if (context.Options.Count > 0)
            {
                var type = context.Options[0];
                var options = new ProgressBarOptions
                {
                    // ProgressCharacter = '─',
                    ProgressBarOnBottom = true,
                    ForegroundColor = ConsoleColor.DarkYellow,
                    ForegroundColorDone = ConsoleColor.DarkGreen,
                    BackgroundColor = ConsoleColor.DarkGray,
                    BackgroundCharacter = '\u2593'
                };

                using (var pbar = new ProgressBar(100, "Download", options))
                {
                    var tick = 0;
                    

                if (type.Equals("templates", StringComparison.InvariantCultureIgnoreCase))
                {
                    $"Download templates from {context.Config.RemoteTemplateDownloadUrl}".WriteConsoleWarning();
                    var uri = new Uri(context.Config.RemoteTemplateDownloadUrl);
                    Download
                        .Templates(uri, (int progress) =>
                        {
                            while(tick <= progress)
                            {
                                pbar.Tick($"Downloading {tick++}% of 100%: Download templates");
                            }
                            

                            //ConsoleExtensions.ShowPercentProgress("Downloading... ", progress, 100);
                            //Console.WriteLine(progress);
                        }, (DownloadInfo info) =>
                        {
                            pbar.Tick($"Done 100% of 100%: Download templates");

                            "\nDownload complete".WriteConsoleWarning();
                            "\nExtract files".WriteConsoleWarning();
                            info.ExtractFiles(context.Config.Global.Template);
                        });
                        
                        
                        
                        
                   
                }
                }

            }
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