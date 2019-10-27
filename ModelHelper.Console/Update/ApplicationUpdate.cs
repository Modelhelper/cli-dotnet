using ModelHelper.Core;
using ModelHelper.Core.Configuration;
using ModelHelper.Core.Remote;
using ModelHelper.Extensions;
using ShellProgressBar;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ModelHelper.Update
{
    public class ApplicationUpdater
    {
        private readonly string manifestLocation;

        public bool UpdateNeeded { get; private set; }
        public bool ContinueWithCommand { get; private set; }

        public ApplicationUpdater()
        {
            //var currentExec = new FileInfo(Assembly.GetExecutingAssembly().Location);
            this.manifestLocation = Path.Combine(ApplicationDefaults.RootDirectory.FullName, "updatebin", "update.manifest.yaml");

            Manifest = LoadManifest(this.manifestLocation);

            UpdateNeeded = Manifest != null;
        }
        public UpdateManifest Manifest { get; internal set; }
        private UpdateManifest LoadManifest(string path)
        {
            if (File.Exists(path))
            {
                var content = File.ReadAllText(path);
                var serializer = new YamlDotNet.Serialization.Deserializer();
                return serializer.Deserialize<UpdateManifest>(content);
            }

            return null;
        }

        public bool Run(string[] args)
        {
            var updated = false;
            if (UpdateNeeded)
            {
                Console.Write("The content of this application needs to update. Update now [Y/n]?: ");
                var updateAnswer = Console.ReadLine();
                var runUpdater = string.IsNullOrEmpty(updateAnswer) ||
                    (!string.IsNullOrEmpty(updateAnswer) &&
                    updateAnswer.Equals("y", StringComparison.InvariantCultureIgnoreCase));

                if (runUpdater)
                {

                    if (Manifest != null && Manifest.Tasks != null && Manifest.Tasks.Any())
                    {
                        const int totalTicks = 10;
                        var options = new ProgressBarOptions
                        {
                            // ProgressCharacter = '─',
                            ProgressBarOnBottom = true,
                            ForegroundColor = ConsoleColor.Yellow,
                            ForegroundColorDone = ConsoleColor.DarkGreen,
                            BackgroundColor = ConsoleColor.DarkGray,
                            BackgroundCharacter = '\u2593'
                        };

                        try
                        {
                            var taskCount = Manifest.Tasks.Count(t => t.Value == true);

                            using (var pbar = new ProgressBar(taskCount, "Update tasks", options ))
                            {
                                var tick = 0;
                                foreach (var task in Manifest.Tasks.Where(t => t.Value == true))
                                {
                                    tick++;
                                    pbar.Tick($"Task {tick} of {taskCount}: Update: [{task.Key}]");
                                    DoTask(task.Key, pbar);
                                } 
                            }
                            updated = true;

                            if (updated)
                            {
                                "\n\nApplication was updated".WriteConsoleSuccess();

                                if (args != null && args.Length > 0)
                                {
                                    //if (args[0].Equals("get", StringComparison.InvariantCultureIgnoreCase))
                                    //{
                                    //    ContinueWithCommand = false;
                                    //}
                                    //else
                                    {
                                        var q = $"\nContinue with current command '{args[0]}' [Y/n]: ";
                                        var continueWithCommandAnswer = ReadLine.Read(q, "y");

                                        ContinueWithCommand = continueWithCommandAnswer.Equals("y", StringComparison.InvariantCultureIgnoreCase);
                                    }
                                    
                                }

                            }
                        }
                        catch (Exception)
                        {

                            updated = false;
                            throw;
                        }
                        
                    }


                    
                }

            }
            

            return updated;
        }

        private void DoTask(string task, ProgressBar pbar)
        {
            bool success = false;

            var childOptions = new ProgressBarOptions
            {
                ForegroundColor = ConsoleColor.Green,
                BackgroundColor = ConsoleColor.DarkGreen,
                ProgressCharacter = '─',
                CollapseWhenFinished = false,
                
            };

            try
            {
                switch (task.ToLowerInvariant())
                {
                    case "updatebinaries":
                        success= true;

                        using (var child = pbar.Spawn(100, "child actions", childOptions))
                        {
                            var templateTicker = 0;

                            for(int i = 0; i < 100; i++)
                            {
                                child.Tick($"{templateTicker++}% of 100%: Download binaries");
                                System.Threading.Thread.Sleep(100);
                            }
                        }
                        //WriteMessage("Update Binaries", success);

                        break;
                    case "updateconfig":
                        success = true;
                        using (var child = pbar.Spawn(100, "child actions", childOptions))
                        {
                            var templateTicker = 0;

                            for (int i = 0; i < 100; i++)
                            {
                                child.Tick($"{templateTicker++}% of 100%: update config");
                                System.Threading.Thread.Sleep(100);
                            }
                        }
                        //WriteMessage("Update Configuration", success);
                        break;
                    case "updatecodedefinitions":
                        using (var child = pbar.Spawn(100, "child actions", childOptions))
                        {
                            var templateTicker = 0;

                            for (int i = 0; i < 100; i++)
                            {
                                child.Tick($"{templateTicker++}% of 100%: Update Code Def");
                                System.Threading.Thread.Sleep(100);
                            }
                        }
                        //WriteMessage("Update Code Definitions", success);

                        break;
                    case "updatetemplates":
                        using (var child = pbar.Spawn(100, "child actions", childOptions))
                        {
                            var templateTicker = 0;
                            
                        

                        var config = GlobalConfig.Load();
                            Download.Templates(new Uri(config.RemoteTemplateDownloadUrl),
                                p => {
                                    while (templateTicker <= p)
                                    {
                                        child.Tick($"{templateTicker++}% of 100%: Download templates");
                                    } 
                                },
                                info => {
                                    child.Tick($"Done 100% of 100%: Download templates");
                                    success = true; 
                                    //info.ExtractFiles()
                                });
                        }
                        //WriteMessage("Update Templates", success);
                        break;
                    case "updateprojectdefinitions":
                        //WriteMessage("Update Project Definitions", success);
                        break;
                    case "deletemanifest":

                        File.Delete(this.manifestLocation);
                        success = true;

                        //WriteMessage("Delete manifest file", success);
                        break;


                }


            }
            catch (Exception e)
            {
                success = false;
                Console.WriteLine(e.Message);
                throw;
            }

            
            
        }


        private void WriteMessage(string message, bool success)
        {
            Console.Write(message + " => ");

            if (success)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("Done");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("ERROR");
                Console.ResetColor();
            }
            
            Console.WriteLine();
        }
    }

    public class UpdateResult
    {
        public bool Updated { get; set; }

    }
    public class UpdateManifest
    {
        public int TargetVersion { get; set; }
        public Dictionary<string, bool> Tasks { get; set; }
    }
}
