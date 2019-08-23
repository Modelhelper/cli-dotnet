using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text;
using DotLiquid.FileSystems;
using ModelHelper.Core;
using ModelHelper.Core.CommandLine;
using ModelHelper.Core.Database;
using ModelHelper.Core.Extensions;
using ModelHelper.Core.Help;
using ModelHelper.Core.Project;
using ModelHelper.Core.Project.V1;
using ModelHelper.Core.Rules;
using ModelHelper.Core.Templates;
using ModelHelper.Extensibility;
using ModelHelper.Extensions;

namespace ModelHelper.Commands
{

    [Export(typeof(ICommand))]    
    public class GenerateCommand : BaseCommand
    {
        public GenerateCommand()
        {
            Key = "generate";
            Alias = "g";
            Help = HelpFactory.Generate(); // GetHelp();
        }

        [ImportMany]
        IEnumerable<Lazy<ISqlGenerator>> _sqlGenerators;

        [ImportMany]
        IEnumerable<Lazy<IDatatypeConverter>> _dataTypeConverters;


        [Option(Key = "--include-relations", IsRequired = false, Aliases = new[] { "-ir", "-r" })]
        public bool IncludeRelations { get; set; }

        [Option(Key = "--show", Aliases = new[] { "-s" })]
        public bool ShowResult { get; set; } = false;

        [Option(Key = "--skip-clipboard", Aliases = new[] { "-sc" })]
        public bool SkipClipboard { get; set; } = false;

        [Option(Key = "--dry-run", IsRequired = false, Aliases = new[] { "-dr" })]
        public bool DryRun { get; set; }

        [Option(Key = "--verbose", IsRequired = false, Aliases = new []{"-v"})]
        public bool Verbose { get; set; } = false;

        [Option(Key = "--entity", IsRequired = false, Aliases = new []{"-e", "--table"})]
        public List<string> Entities { get; set; } = new List<string>();

        [Option(Key = "--except", Aliases = new []{"--except-table"})]
        public List<string> ExceptEntities { get; set; } = new List<string>();

        [Option(Key = "--entity-group", IsRequired = false, Aliases = new []{"-eg", "--table-group"})]
        public List<string> EntityGroups { get; set; } = new List<string>();

        [Option(Key = "--view-only", IsRequired = false, Aliases = new []{"-vo"})]
        public bool ViewsOnly { get; set; } = false;

        [Option(Key = "--table-only", IsRequired = false, Aliases = new []{"-to"})]
        public bool TablesOnly { get; set; } = false;


        // [Obsolete("Will be removed in version 3. Use --entity or -e instead")]
        // public List<string> Tables { get; set; } = new List<string>();

        // [Obsolete("Will be removed in version 3. Use --except instead")]
        // public List<string> ExceptTables { get; set; } = new List<string>();

        // [Obsolete("Will be removed in version 3. Use entity-group instead")]
        // public List<string> TableGroups { get; set; } = new List<string>();

        [Option(Key = "--template", Aliases = new []{"-t"})]
        public List<string> Templates { get; set; } = new List<string>();

        [Option(Key = "--template-group", Aliases = new []{"-tg"})]
        public List<string> TemplateGroups { get; set; } = new List<string>();

        [Option(Key = "--template-directory", Aliases = new []{"-td"})]
        public List<string> TemplateDirectories { get; set; } = new List<string>();

        [Option(Key = "--export-bykey", Aliases = new[] {"-ek"})]
        public bool ExportByKey { get; set; } = false;

        [Option(Key = "--overwrite", Aliases = new[] {"-o"})]
        public bool OverwriteFiles { get; set; } = false;

        [Option(Key = "--export", ParameterIsRequired = true, ParameterProperty = "ExportPath")]
        public bool Export { get; set; } = false;

        public string ExportPath { get; set; }

        [Option(Key = "--connection", IsRequired = false, ParameterIsRequired = true, ParameterProperty = "ConnectionName", Aliases = new[] { "-c" })]
        public bool WithConnection { get; set; } = false;

        public string ConnectionName { get; set; } = "";

        public override bool EvaluateArguments(IRuleEvaluator<Dictionary<string, string>> evaluator)
        {
            return true;
        }

        internal List<TemplateFile> GetTemplateFiles()
        {
            var templateFiles = new List<TemplateFile>();
            var customTemplatePath = ModelHelperConfig.TemplateLocation;
            templateFiles.AddRange(customTemplatePath.GetTemplateFiles("project"));
            templateFiles.AddRange(ConsoleExtensions.UserTemplateDirectory().GetTemplateFiles("core"));

            return templateFiles;
        }

        internal List<string> GetTemplateSelection(List<TemplateFile> templateFiles)
        {
            var list = new List<string>();            
            
            if (Templates.Any())
            {
                list = Templates;
            }
            else if (TemplateGroups.Any())
            {
                list = GetTemplatesFromGroups(TemplateGroups.ToArray(), templateFiles);
            }
            else if (TemplateDirectories.Any())
            {

            }
            else
            {
                return Templates;                
            }

            return list;
        }

        internal List<string> GetTableSelection(IProject project)
        {
            if (!Entities.Any() && !EntityGroups.Any())
            {
                throw new Exception(
                    "No table, list of tables or table groups are provided. Please provide this to generate");
            }

            var list = new List<string>();
            var repo = WithConnection ? project.CreateRepository(ConnectionName) : project.CreateRepository();
            
            if (Entities.Any() && Entities.Count() == 1 && Entities[0] == "*")
            {
                var allTables = repo.GetEntities(false, false).Result;
                list = allTables.Select(t => $"{t.Schema}.{t.Name}").ToList();
            }
            else if (EntityGroups.Any())
            {
                list = GetTablesFromGroups(EntityGroups.ToArray(), repo);
            }
            else
            {
                list = Entities;
            }

            if (ExceptEntities.Any())
            {
                list = list.Where(tbl => !ExceptEntities.Contains(tbl.ToLowerInvariant())).ToList();
            }

            return list;
        }

        internal List<LogItem> ExecutionLog { get; set; } = new List<LogItem>();

        internal void GenerateOutput(List<GeneratedTemplate> codeList)
        {
            var totalResult = new StringBuilder();
            foreach (var code in codeList)
            {
                
            }

            if (ShowResult)
            {
                Console.WriteLine(totalResult);
            }

            if (!SkipClipboard && totalResult.Length > 0)
            {
                ModelHelperExtensions.ToClipboard(totalResult.ToString());

                "\n\nNB! Content is also copied to clipboard. If needed, use ctrl + v to pase the selection"
                    .WriteConsoleSuccess();
                
            }
        }

        internal void WriteSummary(TimeSpan span, int totalItemsToGenerate)
        {
            Console.WriteLine("\n\nSummary:");
            foreach (var item in Statistics)
            {
                Console.WriteLine($"{item.Key.PadRight(25)}: {item.Value.ToString().PadLeft(5)}");
            }

            var timeSaved = Math.Sqrt(totalItemsToGenerate * span.TotalSeconds);
            var timeMessage = $"\nTime to complete: {span.TotalSeconds} seconds, or {span.TotalMilliseconds} milliseconds, or {Math.Round(timeSaved)} saved work hours \n(remember to invoice the client...) ";
            timeMessage.WriteConsoleSuccess();
        }
        internal Dictionary<string, int> Statistics { get; set; } = new Dictionary<string, int>();

        internal void AddStatistics(string key, int value)
        {
            if (!Statistics.ContainsKey(key))
            {
                Statistics.Add(key, value);
            }
            else
            {
                Statistics[key] = value;
            }
        }
        internal void InitializeStatistics()
        {

        }

        public override void Execute(List<string> arguments)
        {
            //var parser = new ArgumentParser<GenerateCommand>();
            var map = ArgumentParser.Parse(this, arguments);

            TextFilter.Converters = _dataTypeConverters?.Select(c => c.Value).ToList();
           
            var s = arguments;
            //var statistics = new Dictionary<string, int>();
            var log = new List<string>();
            //var dryRun = arguments.Contains("--dry-run") || arguments.Contains("-dr");
            var tableCountLabel = "Table count";
            var templateCountLabel = "Template count";
            var totalItemCountLabel = "Generated Code count";
            var totalExportCountLabel = "File export count";

            try
            {
                //var map = new Dictionary<string, string>();

                //var argLength = s.Count;

                //for (int i = 0; i < argLength; i++)
                //{
                //    var next = i + 1;
                //    var arg = s[i];
                //    var param = next < argLength && !s[next].StartsWith("-") ? s[next] : "";

                //    if (arg.StartsWith("-"))
                //    {
                //        map.Add(arg.ToLowerInvariant(), param);
                //    }
                //}

                Console.WriteLine("Starting... please wait");
                var start = DateTime.Now;

                var projectExists = ModelHelperExtensions.ProjectFileExists();

                if (!projectExists)
                {
                    Console.Write(".model-helper project file does not exists. Use > mh new to create one");
                    return;
                }                

                var projectReader = new DefaultProjectReader();
                var project = projectReader.Read(Path.Combine(Directory.GetCurrentDirectory(), ".model-helper"));

                log.Add("read project");
                if (project != null)
                {
                    var outputCodeList = new List<GeneratedTemplate>();
                    var export =
                        ExportByKey ||
                        Export; 

                    var totalResult = new StringBuilder();

                    //var repo = new ModelHelper.Data.SqlServerRepository(project.DataSource.Connection, project);
                    var repo = WithConnection ? project.CreateRepository(ConnectionName) : project.CreateRepository();

                    var templateFiles = GetTemplateFiles();

                    var templateReader = new JsonTemplateReader();

                    var selectedTemplates = GetTemplateSelection(templateFiles);                    

                    var templateCount = selectedTemplates.Count();

                    AddStatistics(templateCountLabel, templateCount);

                    
                    var tables = GetTableSelection(project);
                    
                    var tablesCount = tables.Count();
                    AddStatistics(tableCountLabel, tablesCount);
                    AddStatistics(totalItemCountLabel, 0);
                    AddStatistics(totalExportCountLabel, 0);

                    var totalItemsToGenerate = tablesCount * templateCount;
                    var totalExportCount = 0;
                    var progressIndicator = 0;

                    foreach (var t in selectedTemplates)
                    {
                        var selectedTemplate = templateFiles.FirstOrDefault(tpl =>
                            String.Equals(tpl.Name, t.Trim(), StringComparison.CurrentCultureIgnoreCase));

                        if (selectedTemplate != null)
                        {
                            log.Add("generate for template: " + selectedTemplate.Name);
                            var template = templateReader.Read(selectedTemplate.FileInfo.FullName, selectedTemplate.Name);
                            log.Add("template loaded");
                            if (template != null)
                            {
                                var model = new TemplateModel();

                                model.DatatypeConverters = _dataTypeConverters.Select(c => c.Value).ToList();
                                model.SqlScriptGenerators = _sqlGenerators.Select(g => g.Value).ToList();

                                model.IncludeChildren =
                                    map.ContainsKey("--include-children") || map.ContainsKey("-ic") ||
                                    IncludeRelations;

                                model.IncludeParents =
                                    map.ContainsKey("--include-parents") || map.ContainsKey("-ip") ||
                                    IncludeRelations;

                                model.IncludeRelations = IncludeRelations;
                                 
                                foreach (var table in tables)
                                {
                                    var codeOutput = new GeneratedTemplate();
                                    codeOutput.Index = progressIndicator++;
                                    codeOutput.TemplateUsed = selectedTemplate;


                                    ConsoleExtensions.ShowPercentProgress("Generating Code... ", progressIndicator,
                                        totalItemsToGenerate);

                                    log.Add($"load table {table} ");
                                    model.Table = repo.GetEntity(table.Trim(), true).Result;

                                    if (model.Table == null)
                                    {
                                        log.Add($"{table} does not exist");
                                        throw new Exception($"{table} does not exist");
                                    }

                                    model.Project = project;
                                    log.Add($"table loaded ");
                                    codeOutput.TableUsed = model.Table;
                                    codeOutput.State = TemplateGeneratedState.InProgress;

                                    log.Add($"use table {model.Table.Name} for template");
                                    var result = template.Render(model);

                                    codeOutput.SourceFile.Content = result;
                                    codeOutput.State = TemplateGeneratedState.CodeGeneratedSuccess;

                                    AddStatistics(totalItemCountLabel, progressIndicator);

                                    totalResult.Append("\n" + result);

                                    // var exportByType = s.Contains("--export-bytype") || s.Contains("--export-bykey") || s.Contains("-ek");

                                    if (export && template.CanExport)
                                    {
                                        var outputPath = string.Empty;
                                        var tmpPath = string.Empty;

                                        if (ExportByKey && !string.IsNullOrEmpty(template.ExportType))
                                        {
                                            var codeLocation =
                                                project.Code.Locations.FirstOrDefault(c =>
                                                    c.Key == template.ExportType);

                                            if (codeLocation != null)
                                            {
                                                tmpPath = codeLocation.Path;
                                            }
                                        }
                                        else
                                        {
                                            tmpPath = map.ContainsKey("--export")
                                                ? map["--export"].Replace("\"", "")
                                                : "";
                                        }


                                        outputPath = tmpPath;

                                        if (string.IsNullOrEmpty(tmpPath))
                                        {
                                            outputPath = Path.Combine(Directory.GetCurrentDirectory(),
                                                "temp-output");
                                        }
                                        else if (tmpPath.StartsWith(".\\"))
                                        {
                                            var partPath = tmpPath.Substring(2).Render(model);
                                            outputPath = Path.Combine(Directory.GetCurrentDirectory(), partPath);
                                        }

                                        var directory = new DirectoryInfo(outputPath);

                                        if (!directory.Exists)
                                        {
                                            directory.Create();
                                        }

                                        //if (directory.Exists)

                                        var fileName = string.IsNullOrEmpty(template.ExportFileName)
                                            ? $"tmp-{DateTime.Now.ToString("yyyyMMdd-HHmmss")}.txt"
                                            : template.ExportFileName;

                                        fileName = fileName.Render(model);


                                        var filePath = Path.Combine(outputPath, fileName);

                                        codeOutput.SourceFile.Filename = filePath;
                                        codeOutput.DestinationFile.Exists = File.Exists(filePath);
                                        codeOutput.DestinationFile.Path = filePath;

                                        // TODO - need to remove this
                                        var writeFile =
                                            !File.Exists(filePath) || OverwriteFiles;
                                        if (writeFile)
                                        {
                                            System.IO.File.WriteAllText(filePath, result);
                                            AddStatistics(totalExportCountLabel, ++totalExportCount);
                                        }                                        
                                    }

                                    outputCodeList.Add(codeOutput);

                                }
                                
                            }
                        }
                    }
                    
                    var end = DateTime.Now;                    

                    if (!SkipClipboard && totalResult.Length > 0)
                    {
                        ModelHelperExtensions.ToClipboard(totalResult.ToString());

                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        //Console.WriteLine($"{model.Table.Name} ble generert til {filePath}...");
                        //

                        Console.WriteLine(
                            "\n\nNB! The generated content has also copied to the clip board, use Ctrl + v to paste if needed");
                        Console.ResetColor();
                    }


                    if (ShowResult && totalResult.Length > 0)
                    {
                        "\nResult".WriteConsoleSubTitle();
                        Console.Write(totalResult.ToString());
                    }

                    var span = new TimeSpan(end.Ticks - start.Ticks);

                    WriteSummary(span, totalItemsToGenerate);

                    if (DryRun && export)
                    {
                        outputCodeList
                            .Select(l => new
                            {
                                TemplateName = l.TemplateUsed.Name,
                                TableName = l.TableUsed.Name,
                                l.State,
                                l.SourceFile.CanExport
                            }).ToList()
                            .ToConsoleTable()
                            .WriteToConsole();


                        Console.Write($"\nExport files and snippets? Y/n: ");
                        var exportResponse = Console.ReadLine();

                        if (string.IsNullOrEmpty(exportResponse) ||
                            exportResponse.Equals("y", StringComparison.CurrentCultureIgnoreCase))
                        {
                            foreach (var template in outputCodeList)
                            {
                                Console.WriteLine("Write to file " + template.TemplateUsed.Name);
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {

                
                var exceptionMessage = $"\nException\n{exception.Message}\n\nStack: {exception.StackTrace}\n\n";
                Console.WriteLine(exceptionMessage);

                if (exception.InnerException != null)
                {
                    var innerExceptionMessage = $@"\nInner Exception\n{exception.InnerException.Message}\n\nStack: {exception.InnerException.StackTrace}";
                    Console.WriteLine(innerExceptionMessage);
                }

                Console.WriteLine("Log");
                foreach (var logItem in log)
                {
                    Console.WriteLine(logItem);
                }
            }
        }

        private static List<string> GetTemplatesFromGroups(string[] groups, List<TemplateFile> templateFiles)
        {
            var items = new List<string>();
            var templateReader = new JsonTemplateReader();

            foreach (var templateFile in templateFiles)
            {
                //var fileInfo = new FileInfo(customFile);
                try
                {
                    var template = templateReader.Read(templateFile.FileInfo.FullName, templateFile.Name);
                    //if (template != null && template.Groups != null && template.Groups.Any() && !template.Groups.Except(groups).Any())
                    if (template != null && template.Groups != null && template.Groups.Any())
                    {
                        var isInGroup = !groups.Except(template.Groups).Any();

                        if (isInGroup)
                        {
                            var fileName = templateFile.Name;

                            if (!items.Contains(fileName))
                            {
                                items.Add(fileName);
                            }
                        }

                    }

                }
                catch (Exception exception)
                {

                }


            }
            return items;


        }

        private static List<string> GetTablesFromGroups(string[] groups, IDatabaseRepository repository)
        {
            var items = new List<string>();


            foreach (var group in groups)
            {
                //var fileInfo = new FileInfo(customFile);
                var entities = repository.SuggestEntityGroupName(group).Result;

                if (entities != null && entities.Any())
                {
                    foreach (var table in entities)
                    {
                        var tableName = $"{table.Schema}.{table.Name}";
                        if (!items.Contains(tableName))
                        {
                            items.Add(tableName);
                        }
                    }
                }

            }
            return items;


        }


        private HelpItem GetHelp()
        {
            var help = new HelpItem();

            help.ShortDescription = "Dette er en test på hjelp..";
            return help;
        }
    }
}