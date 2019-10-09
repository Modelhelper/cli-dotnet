using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using ModelHelper.Core;
using ModelHelper.Core.Extensions;
using ModelHelper.Core.Rules;
using ModelHelper.Core.Templates;
using ModelHelper.Extensions;
using ModelHelper.Core.CommandLine;

namespace ModelHelper.Commands
{
    [Export(typeof(ICommand))]    
    public class TemplateCommand : BaseCommand
    {
        public TemplateCommand()
        {
            Key = "template";
            Alias = "t";
            Help = HelpFactory.Help();
        }

        [Option(Key = "--show")]
        public bool ShowTemplateContent { get; set; }

        [Option(Key = "--grouped", Aliases = new[] { "-g", "--group" }, ParameterProperty = "GroupName")]
        public bool ShowTemplatesByGroup { get; set; }

        [Option(Key = "--export-type", Aliases = new[] { "-et", "--type" }, ParameterProperty = "ExportName")]
        public bool ShowTemplatesByExportType { get; set; }

        public string GroupName { get; set; }
        public string ExportName { get; set; }

        [Option(Key = "--verbose", Aliases = new[] { "-v" })]
        public bool Verbose { get; set; }

        [Option(Key = "--overwrite", Aliases = new[] { "-o" })]
        public bool Overwrite { get; set; }

        [Option(Key = "--simple", Aliases = new[] { "-s"})]
        public bool SimpleList { get; set; }

        [Option(Key = "--fetch-remote", Aliases = new[] { "-fr" })]
        public bool FetchRemote { get; set; }


        public override bool EvaluateArguments(IRuleEvaluator<Dictionary<string, string>> evaluator)
        {
            return true;
        }

        public override void Execute(List<string> args)
        {
            
            var argumentMap = this.Parse(args);

            var showList = true; //s.Contains("--list") || s.Contains("-l");
            //var showContent = args.Contains("--show");
            var templateName = args.Count > 0 && !args[0].StartsWith("-") ? args[0] : "";
            //var fetchRemote = args.Contains("--fetch-remote") || args.Contains("-fr");
            //var overwrite = args.Contains("--overwrite");
            //var verbose = args.Contains("--verbose");
            //var simple = args.Contains("--simple");

            var modelHelperData = ConsoleExtensions.UserTemplateDirectory();

            if (!Directory.Exists(modelHelperData))
            {
                Directory.CreateDirectory(modelHelperData);
            }

            if (args.Contains("--new"))
            {

            }

            if (FetchRemote)
            {
                var remoteLocation = ModelHelperConfig.RemoteTemplateLocation;

                ModelHelperExtensions.FetchRemoteTemplates(remoteLocation, modelHelperData, Overwrite,
                    (index, total) => ConsoleExtensions.ShowPercentProgress("downloading templates... ", index, total));
            }

            var templateReader = new JsonTemplateReader();
            var customTemplatePath = Path.Combine(Directory.GetCurrentDirectory(), "templates");  //ModelHelperConfig.TemplateLocation; // 
            var rootTemplates = ModelHelperConfig.TemplateLocation;
            var templateFiles = new List<TemplateFile>();

            templateFiles.AddRange(customTemplatePath.GetTemplateFiles("project"));
            templateFiles.AddRange(rootTemplates.GetTemplateFiles("mh"));


            if (string.IsNullOrEmpty(templateName))
            {


                var maxLenName = (templateFiles.Count > 0 ? templateFiles.Max(f => f.Name.Length) : 10) + 5;

                if (!SimpleList)
                {
                    
                    if (ShowTemplatesByExportType)
                    {
                        var templateGroups = templateFiles
                        .Select(t => new { Name = t.Name, t.Scope, Template = templateReader.Read(t.FileInfo.FullName, t.Name) }).ToList()
                        .Select(t => new { t.Name, t.Scope, Groups =t.Template?.Groups?.AsCommaSeparatedString() ?? "", ExportType = t.Template?.ExportType ?? "no type", CanExport = t.Template?.CanExport ?? false }).ToList();  //{Name = t.Name, t.Scope, t.}})
                        //.GroupBy(g => string.Join(",", g.Groups)).Select(gg => gg.First()).ToArray();
                        //.GroupBy(g => g.Groups).GroupBy(g2 => g2).Select(grp => new { Name = grp.Key, Items = grp.ToList() });
                        var groups = (from p in templateGroups
                                   //from a in p.ExportType
                                   group p by p.ExportType into g
                                   select new
                                   {
                                       Name = g.Key,
                                       Templates = g.ToList()
                                   }
                                   ).ToList();

                        if (string.IsNullOrEmpty(ExportName))
                        {


                            if (!Verbose)
                            {
                                groups
                                    .Select(t => new { t.Name, Count = t.Templates.Count() })
                                    .ToConsoleTable()
                                    .WriteToConsole();
                            }
                            else
                            {

                                foreach (var group in groups)
                                {

                                    Console.WriteLine($"\nExport type: {group.Name}\n");

                                    group.Templates
                                        .Select(t => new { t.Name, t.Scope, t.Groups, t.CanExport })
                                        .ToConsoleTable()
                                        .WriteToConsole();

                                    Console.WriteLine();
                                    
                                    //Console.WriteLine();
                                }
                            }
                        }
                        else
                        {
                            var group = groups.FirstOrDefault(g => g.Name.Equals(GroupName));
                            Console.WriteLine($"\nExport type: {group.Name}\n");

                            group.Templates
                                .Select(t => new { t.Name, t.Scope, t.Groups, t.CanExport })
                                .ToConsoleTable()
                                .WriteToConsole();

                            Console.WriteLine();
                        }

                        return;
                    }
                    
                    if (!ShowTemplatesByGroup)
                    {
                        templateFiles
                        .Select(t => new { Name = t.Name, t.Scope, Template = templateReader.Read(t.FileInfo.FullName, t.Name) }).ToList()
                        .Select(t => new { t.Name, t.Scope, Groups = t.Template?.Groups?.AsCommaSeparatedString() ?? "", ExportType = t.Template?.ExportType ?? "", CanExport = t.Template?.CanExport ?? false })  //{Name = t.Name, t.Scope, t.}})
                        .ToConsoleTable()
                        .WriteToConsole();
                    }
                    
                    else
                    {
                        
                        var templateGroups = templateFiles
                        .Select(t => new { Name = t.Name, t.Scope, Template = templateReader.Read(t.FileInfo.FullName, t.Name) }).ToList()
                        .Select(t => new { t.Name, t.Scope, Groups = t.Template != null && t.Template.Groups.Any(g => g.Length > 0) ? t.Template?.Groups : new List<string> { "no group" }, ExportType = t.Template?.ExportType ?? "", CanExport = t.Template?.CanExport ?? false }).ToList();  //{Name = t.Name, t.Scope, t.}})
                        //.GroupBy(g => string.Join(",", g.Groups)).Select(gg => gg.First()).ToArray();
                        //.GroupBy(g => g.Groups).GroupBy(g2 => g2).Select(grp => new { Name = grp.Key, Items = grp.ToList() });
                        var groups = (from p in templateGroups
                                   from a in p.Groups
                                   group p by a into g
                                   select new
                                   {
                                       Name = g.Key,
                                       Templates = g.ToList()
                                   }
                                   ).ToList();

                        if (string.IsNullOrEmpty(GroupName))
                        {
                            if (!Verbose)
                            {
                                groups
                                    .Select(t => new { t.Name, Count = t.Templates.Count() })
                                    .ToConsoleTable()
                                    .WriteToConsole();
                            }
                            else
                            {
                                foreach (var group in groups)
                                {

                                    Console.WriteLine($"\nGroup name: {group.Name}\n");

                                    group.Templates
                                        .Select(t => new { t.Name, t.Scope, t.ExportType, t.CanExport })
                                        .ToConsoleTable()
                                        .WriteToConsole();

                                    Console.WriteLine();
                                        //Console.WriteLine();
                                    
                                }
                            }
                        }
                        else
                        {
                            var group = groups.FirstOrDefault(g => g.Name.Equals(GroupName));
                            Console.WriteLine($"\nGroup name: {group.Name}\n");

                            group.Templates
                                .Select(t => new { t.Name, t.Scope, t.ExportType, t.CanExport })
                                .ToConsoleTable()
                                .WriteToConsole();

                            Console.WriteLine();
                        }
                    }
                }
                                                
                else
                {                    

                    templateFiles.OrderBy(t =>t.Name).Select(t => new { Name = t.Name })                   
                        .ToConsoleTable()
                        .WriteToConsole(false);
                }                

            }
            else
            {
               
                var selectedTemplate = templateFiles.FirstOrDefault(t =>
                    t.Name.Equals(templateName, StringComparison.InvariantCultureIgnoreCase));

                if (selectedTemplate != null)
                {
                    ("Selected template " + templateName).WriteConsoleTitle();
                    var template = templateReader.Read(selectedTemplate.Location, selectedTemplate.Name);

                    if (template != null)
                    {
                        Console.WriteLine(template.Name);
                        Console.WriteLine(template.Body);
                    }
                }
                else
                {
                    "Could not find this template".WriteConsoleWarning();
                }
                
            }

        }
    }

    [Export(typeof(ICommand))]
    public class ConfigCommand : BaseCommand
    {
        public ConfigCommand()
        {
            Key = "config";
            Help = HelpFactory.Config();
        }
        public override bool EvaluateArguments(IRuleEvaluator<Dictionary<string, string>> evaluator)
        {
            throw new NotImplementedException();
        }

        [Option(Key = "--set-remote", ParameterProperty = "RemotePath", Aliases = new[] { "-sr" })]
        public bool SetRemote { get; set; }

        public string RemotePath { get; set; }

        [Option(Key = "--get-remote", Aliases = new[] { "-gr" })]
        public bool GetRemote { get; set; }


        public override void Execute(List<string> arguments)
        {
            var options = new Dictionary<string, Action<string>>();
            //var configReader = new JsonConfigReader();
            var argumentList = this.Parse(arguments); // arguments.AsArgumentDictionary();
            var config = ModelHelperConfig.ReadConfig();

            if (SetRemote)
            {
                SetRemotePath(config);
                
                ModelHelperConfig.SaveConfig(config);
            }
            else if (GetRemote)
            {
                WriteRemote(config);
            }
        }

        internal void SetRemotePath(Config config)
        {            
            if (string.IsNullOrEmpty(RemotePath))
            {
                if (config != null && !string.IsNullOrEmpty(config.RemoteTemplateLocation))
                {
                    $"\nCurrent remote location is '{config.RemoteTemplateLocation}'".WriteConsoleGray();
                }

                Console.Write("\nEnter remote path: ");
                config.RemoteTemplateLocation = Console.ReadLine();
            }
            else
            {
                config.RemoteTemplateLocation = RemotePath;
            }
        }

        internal void WriteRemote(Config config)
        {
            if (config != null && !string.IsNullOrEmpty(config.RemoteTemplateLocation))
            {
                $"\nCurrent remote location is '{config.RemoteTemplateLocation}'".WriteConsoleGray();
            }

        }
    }
}