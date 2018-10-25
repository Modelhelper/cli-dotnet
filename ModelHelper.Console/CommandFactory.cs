using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Media.Media3D;
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
using ModelHelper.Extensibility;

namespace ModelHelper
{
    [Obsolete]
    public static class CommandFactory
    {
        public static List<Command> Create()
        {
            var command = new List<ICommand>();

            command.Add(new HelpCommand());
            command.Add(new AboutCommand());
            command.Add(new GenerateCommand());
            command.Add(new ProjectCommand());
            command.Add(new EntityCommand());
            command.Add(new TemplateCommand());
            command.Add(new NewCommand());

            var commands_old = new List<Command>();

            commands_old.Add(New());
            commands_old.Add(Generate());
            commands_old.Add(Template());
            commands_old.Add(Version());
            commands_old.Add(Help());
            commands_old.Add(Tables());
            commands_old.Add(About());
            commands_old.Add(Script());
            commands_old.Add(Project());

            return commands_old;
        }

        [Obsolete]
        public static Command Init()
        {
            var help = HelpFactory.Init();
            var command = new Command
            {
                Key = "init",
                ShortDescription = "",
                
            };

            command.Execute = s =>
            {
                var modelHelperLocal = ModelHelperExtensions.RootFolder;

                ConsoleExtensions.WriteConsoleTitle(help.GetMessage(Constants.HELP_INIT_TITLE));               
                Console.WriteLine(help.GetMessage(Constants.HELP_INIT_WELCOME));

                Console.Write(help.GetMessage("runSetup"));
                var result = Console.ReadLine();

                if (string.IsNullOrEmpty(result) || result.ToLower() == "y")
                {
                    ModelHelperExtensions.CreateRootDirectory();
                    
                    ModelHelperConfig.ReadConfig();
                    var templateCount = ModelHelperExtensions.FetchRemoteTemplates(ModelHelperConfig.RemoteTemplateLocation, 
                        Path.Combine(modelHelperLocal, "templates"), true,
                        (index, total) => ConsoleExtensions.ShowPercentProgress("Laster ned file", index, total));
                    
                    var afterInstall = help.GetMessage("afterInstall", modelHelperLocal, templateCount.ToString()); 
                   
                    Console.WriteLine(afterInstall);
                    Console.Write(help.GetMessage("listTemplates"));
                    var hemplateResult = Console.ReadLine();

                    if (string.IsNullOrEmpty(hemplateResult) || hemplateResult.ToLower() == "y")
                    {
                        var templateCommand = Template();
                        templateCommand.Execute(new List<string>().ToArray());
                    }
                }

                Console.Write(help.GetMessage("showValidCommands"));
                var helpResult = Console.ReadLine();

                if (string.IsNullOrEmpty(helpResult) || helpResult.ToLower() == "y")
                {
                    var helpCommand = Help();
                    helpCommand.Execute(null);
                }
            };

            return command;
        }

        public static Command Project()
        {
            var command = new Command
            {
                Key = "project",
                ShortDescription = "Jobber med prosjektrelaterte ting (som i .model-helper)",
                Options = new List<Option>
                {
                    new Option{Key = "--new <project-name>"},
                    new Option{Key = "--from <project-name>", HelpMessage = "Brukes sammen med --new for å kopiere innhold fra den valgte prosjektfilen"},
                    new Option{Key = "--show", HelpMessage = "Viser innholdet i prosjektfilen"},
                    new Option{Key = "--recent", HelpMessage = "Viser en liste over de siste prosjektene du har jobbet med"},
                    //new Option{Key = "--skip-tests", HelpMessage = "Skips testing the connection to the database, based on the connection string"},
                    new Option{Key = "--overwrite", HelpMessage = "skriver over eksisterende .model-helper fil uten å gi advarsel"},
                    //new Option{Key = "--open", HelpMessage = "Opens the .model-helper file in VSCode"},

                }
            };

            command.Execute = (args) =>
            {
                if (args.Contains("--new"))
                {
                    var newCommand = CommandFactory.New();
                    newCommand.Execute(args);
                }
                else if (args.Contains("--recent"))
                {
                    
                }
                else
                {                    
                    var projectExists = ModelHelperExtensions.ProjectFileExists();

                    if (projectExists)
                    {
                        var projectReader = new ProjectJsonReader();
                        var project = projectReader.Read(Path.Combine(Directory.GetCurrentDirectory(), ".model-helper"));

                        if (project != null)
                        {
                            ConsoleExtensions.WriteConsoleTitle(project.Name);
                            Console.WriteLine($"Customer: {project.Customer}");
                        }
                    }
                }
            };
            return command;
        }

        [Obsolete]
        public static Command New()
        {
            var command = new Command
            {
                Key = "new",
                ShortDescription = "oppretter ny .model-helper der mh.exe kjøres fra",
                Options = new List<Option>
                {
                    new Option{Key = "<project-name>", HelpMessage = "Setter prosjektnavnet"},
                    new Option{Key = "--from <project-name>", HelpMessage = "Brukes for å kopiere innhold fra den valgte prosjektfilen"},
                    //new Option{Key = "--skip-tests", HelpMessage = "Skips testing the connection to the database, based on the connection string"},
                    new Option{Key = "--overwrite", HelpMessage = "skriver over eksisterende .model-helper fil uten å gi advarsel"},
                    //new Option{Key = "--open", HelpMessage = "Opens the .model-helper file in VSCode"},

                }
            };

            command.Execute = s =>
            {                

                var skipTests = s.Contains("--skip-tests");
                var projectName = s.Length > 0 && !s[0].StartsWith("-") ? s[0] : "";
                var openForEdit = s.Contains("--open");

                var projectExists = ModelHelperExtensions.ProjectFileExists();
                var createProjectFile = true;

                if (projectExists)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("Det eksisterer en prosjektfil allerde, vil du overskrive denne (y/N)? ");
                    Console.ResetColor();
                    var result = Console.ReadLine();

                    createProjectFile = !string.IsNullOrEmpty(result) && result.ToLowerInvariant() == "y";

                }

                if (createProjectFile)
                {
                    
                
                    var project = new ModelHelper.Core.Project.Project();
                    var projectWriter = new ModelHelper.Core.Project.ProjectJsonWriter();


                    Console.WriteLine("\nDu blir nå presentert noen få spørsmål\n");

                    if (string.IsNullOrEmpty(projectName))
                    {
                        Console.Write("Prosektnavn: ");
                        projectName = Console.ReadLine();
                    }

                    project.Name = projectName;

                    Console.Write("Kundenavn: ");
                    project.Customer = Console.ReadLine();

                    Console.Write("Legg inn 'ConnectionString' nå? (Y/n): ");
                    var createConnection = Console.ReadLine();
                    if (string.IsNullOrEmpty(createConnection) ||
                        (!string.IsNullOrEmpty(createConnection) &&
                         createConnection.Equals("y", StringComparison.InvariantCultureIgnoreCase)))
                    {
                        var connectionstring = new SqlConnectionStringBuilder();

                        Console.Write("MS Sql Server name: ");
                        var sqlServer = Console.ReadLine();
                        if (!string.IsNullOrEmpty(sqlServer))
                        {
                            connectionstring.DataSource = sqlServer;

                            Console.Write("Database Name: ");
                            connectionstring.InitialCatalog = Console.ReadLine();

                            Console.Write("Use integrated security (Y/n): ");
                            var integratedAnswer = Console.ReadLine();
                            var useIntegrated = string.IsNullOrEmpty(integratedAnswer) ||
                                                (!string.IsNullOrEmpty(integratedAnswer) && integratedAnswer.Equals("y", StringComparison.InvariantCultureIgnoreCase));

                            if (useIntegrated)
                            {
                                connectionstring.IntegratedSecurity = true;
                            }
                            else
                            {
                                Console.Write("Username: ");
                                connectionstring.UserID = Console.ReadLine();

                                Console.Write("Password: ");
                                var pass = "";
                                ConsoleKeyInfo key;
                                do
                                {
                                    key = Console.ReadKey(true);
                                    if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                                    {
                                        pass += key.KeyChar;
                                        Console.Write("*");
                                    }
                                    else
                                    {
                                        if (key.Key == ConsoleKey.Backspace && connectionstring.Password.Length > 0)
                                        {
                                            pass = pass.Substring(0, (pass.Length - 1));
                                            Console.Write("\b \b");
                                        }
                                    }
                                } while (key.Key != ConsoleKey.Enter);
                                connectionstring.Password = pass;

                            }

                            connectionstring.ConnectTimeout = 10;
                            connectionstring.ApplicationName = "ModelHelper";

                            project.DataSource.Connection = connectionstring.ConnectionString;

                            
                        }
                    }
                    project.Code.Connection.Method = "";
                    //project.Api = new ProjectApiModel
                    //{
                    //    UseLogger = false,
                    //    UseTelemetry = false,
                    //    UseUserRepository = false
                    //};

                    project.Code.Locations = new List<ProjectCodeStructure>
                    {
                        new ProjectCodeStructure{Key = "api.interfaces", Namespace = $"{project.Customer }.Infrastructure.Repositories" , Path = ".\\interfaces"},
                        new ProjectCodeStructure{Key = "api.models", Namespace = $"{project.Customer }.Infrastructure.Model" , Path = ".\\models"},
                        new ProjectCodeStructure{Key = "api.controllers", Namespace = $"{project.Customer }.Api.Controllers" , Path = ".\\controllers"},
                        new ProjectCodeStructure{Key = "api.repositories", Namespace = $"{project.Customer }.Data.Repositories" , Path = ".\\repositories"},
                    };
                   
                    
                    //project.CustomTemplateDirectory = ".\\templates";

                    var projectPath = Path.Combine(Directory.GetCurrentDirectory(), ".model-helper");
                    projectWriter.Write(projectPath, project);
                }
                

            };

            return command;
        }

        [Obsolete]
        public static Command Generate()
        {
            var command = new Command
            {
                Key = "generate",
                ShortDescription = "Genererer kode for den spesifiserte tabellen og mal",
                LongDescription = $@"
Genererer kode for den spesifiserte tabellen og mal.

Bruk de forskjellige opsjonene for å bestemme hva som skal utføres.
For --table og --template er det mulig å angi flere enheter ved å skille med , (komma).

Eks: mh generate --table {"\""}tabell1, tabell2, ..n{"\""} --template {"\""}tpl1, tpl2, ..n{"\""}

Dette vil generere totalt = (antall tabeller x antall maler)

Dersom --output er angitt uten å spesifisere hvor så vil oppsettet i .model-helper filen brukes, 
så sant det eksisterer. Hvis ikke så vil det opprettes en ModelHelperTemp mappe der resultatet av
all koden vil bli generert.

For at det skal være mulig å eksportere koden til fil så må malen og egenskapen 'canExport' være 'true',
samt 'exportFileName' ha et innhold.

",
                Options = new List<Option>
                {
                    new Option{Key = "--table <table-name>", IsOptional = false, HelpMessage = "Angir hvilken tabell som skal brukes mot valgte mal."},
                    new Option{Key = "--table-group <group-name>", IsOptional = false, HelpMessage = "Angir hvilke tabeller som logisk hører sammen, som skal brukes mot valgte mal."},
                    new Option{Key = "--except-table <list-of-tables>", IsOptional = false, HelpMessage = "Hvis man har bestemt om man skal bruke alle (*) eller en tabell gruppe, bortsett fra..."},
                    new Option{Key = "--template <template-name>", IsOptional = false, HelpMessage = "Angir malen for å brukes mot den valgte tabellen"},
                    new Option{Key = "--template-group <group-name>", IsOptional = false, HelpMessage = "Angir hvilke grupperte maler som skal brukes mot den valgte tabellen"},
                    new Option{Key = "--export \"[<directory>]\" | -e", IsOptional = true, HelpMessage = "Bestemmer hvor den genererte koden skal skrives til."},
                    new Option{Key = "--export-bykey | -ek", IsOptional = true, HelpMessage = "Bestemmer hvor den genererte koden skal skrives til."},
                    new Option{Key = "--show-result | --show | -s", IsOptional = true, HelpMessage = "Viser resultatet av koden i console- vinduet"},
                    new Option{Key = "--overwrite | -o", IsOptional = true, HelpMessage = "Hvis --output er valgt og filen allerede eksisterer så vil denne overskrive uten advarsel"},
                    new Option{Key = "--skip-clipboard | -sc", IsOptional = true, HelpMessage = "Legger ikke resultate i Clipboard,"},
                    new Option{Key = "--include-children | -ic", IsOptional = true, HelpMessage = "Sets a flag to indicate if the template model should use child relataions"},
                    new Option{Key = "--include-parents | -ip", IsOptional = true, HelpMessage = "Sets a flag to indicate if the template model should use parent relataions"},
                    new Option{Key = "--include-relations | -ir", IsOptional = true, HelpMessage = "Sets a flag to indicate if the template model should use either parent or child relataions"},
                    new Option{Key = "--help | -h", IsOptional = true, HelpMessage = "Viser hjelp for denne kommando"},
                    //new Option{Key = "--project", IsOptional = true, HelpMessage = "Overstyrer --template og --table ved å bruke model og innholdet i denne som grunnlag for en JSON- fil"},
                }
            };

            command.Execute = s =>
            {
                var statistics = new Dictionary<string, int>();
                var log = new List<string>();

                var tableCountLabel = "Antall tabeller";
                var templateCountLabel = "Antall maler";
                var totalItemCountLabel = "Antall maler generert";
                var totalExportCountLabel = "Antall filer eksportert";

                try
                {
                    var map = new Dictionary<string, string>();

                    var argLength = s.Length;

                    for (int i = 0; i < argLength; i++)
                    {
                        var next = i + 1;
                        var arg = s[i];
                        var param = next < argLength && !s[next].StartsWith("-") ? s[next] : "";

                        if (arg.StartsWith("-"))
                        {
                            map.Add(arg.ToLowerInvariant(), param);
                        }
                    }

                    Console.WriteLine("Starter... vennligst vent" );
                    var start = DateTime.Now;

                    var projectExists = ModelHelperExtensions.ProjectFileExists();

                    if (map.ContainsKey("-h") || map.ContainsKey("--help"))
                    {
                        CommandFactory.Help().Execute(new []{ "generate"});
                        return;
                        
                    }
                    if (!projectExists)
                    {

                        Console.Write(".model-helper prosjektfil eksisterer ikke her. Opprett en nå (y/N)? ");
                        var result = Console.ReadLine();

                        var create = !string.IsNullOrEmpty(result) && result.ToLowerInvariant() == "y";

                        if (create)
                        {
                            var newCommand = CommandFactory.New();
                            var args = new List<string>();
                            newCommand.Execute(args.ToArray());
                        }
                        else
                        {
                            Console.WriteLine("Det er også mulig å opprette nytt prosjekt med kommandoen 'mh new'");
                        }

                    }
                    else
                    {
                        //var map = new Dictionary<string, string>();

                        //var argLength = s.Length;

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

                        var projectReader = new ProjectJsonReader();
                        var project = projectReader.Read(Path.Combine(Directory.GetCurrentDirectory(), ".model-helper"));

                        log.Add("read project");
                        if (project != null)
                        {
                            var exceptTables = new List<string>();
                            var tableName = map.ContainsKey("--table") ? map["--table"] : "";
                            var templateName = map.ContainsKey("--template") ? map["--template"] : "";
                            var templateGroups =
                                map.ContainsKey("--template-group") ? map["--template-group"] : String.Empty;

                            var tableGroupName = map.ContainsKey("--table-group") ? map["--table-group"] : "";

                            var totalResult = new StringBuilder();

                            var repo = new ModelHelper.Data.SqlServerRepository(project.DataSource.Connection, project);

                            var templateFiles = new List<TemplateFile>();
                            var customTemplatePath = Path.Combine(Directory.GetCurrentDirectory(), "templates");
                            templateFiles.AddRange(customTemplatePath.GetTemplateFiles("custom"));
                            templateFiles.AddRange(ConsoleExtensions.UserTemplateDirectory().GetTemplateFiles(""));

                            var templateReader = new JsonTemplateReader();

                            var selectedTemplates = new List<string>();
                            //var selectedTemplate = Path.Combine(CommandExtensions.UserTemplateDirectory(),
                            //    templateName + ".json");
                            if (!map.ContainsKey("--template-group"))
                            {
                                //selectedTemplates = templateName.Split(',');
                                selectedTemplates = new List<string>(templateName.Split(','));
                            }
                            else
                            {                                
                                selectedTemplates = new List<string>(GetTemplatesFromGroups(templateGroups.Split(','), templateFiles));
                            }

                            var templateCount = selectedTemplates.Count();

                            if (!statistics.ContainsKey(templateCountLabel))
                            {
                                statistics.Add(templateCountLabel, templateCount);
                            }

                            var tables = new List<string>();

                            if (map.ContainsKey("--except-table"))
                            {
                                var exceptTableNames = map["--except-table"].ToLowerInvariant();
                                exceptTables.AddRange(exceptTableNames.Split(',').Select(tbl => tbl.Trim()));
                            }

                            if (!map.ContainsKey("--table-group"))
                            {
                                tables = new List<string>(tableName.Split(','));
                            }
                            else
                            {
                                tables = new List<string>(GetTablesFromGroups(tableGroupName.Split(','), repo));
                            }

                            
                            if (tables.Any() && tables.Count() == 1 && tables[0] == "*")
                            {
                                var allTables = repo.GetTables().Result;
                                tables = allTables.Select(t => $"{t.Schema}.{t.Name}").ToList();
                            }

                            tables = tables.Where(tbl => !exceptTables.Contains(tbl.ToLowerInvariant())).ToList();

                            var tablesCount = tables.Count();
                            if (!statistics.ContainsKey(tableCountLabel))
                                {
                                statistics.Add(tableCountLabel, tablesCount);
                            }

                            if (!statistics.ContainsKey(totalItemCountLabel))
                            {
                                statistics.Add(totalItemCountLabel, 0);
                            }

                            if (!statistics.ContainsKey(totalExportCountLabel))
                            {
                                statistics.Add(totalExportCountLabel, 0);
                            }

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
                                    var template = templateReader.Read(selectedTemplate.FileInfo.FullName);

                                    if (template != null)
                                    {
                                        var model = new TemplateModel();

                                        model.IncludeChildren = map.ContainsKey("--include-children") || map.ContainsKey("-ic") || map.ContainsKey("--include-relations") || map.ContainsKey("-ir");
                                        model.IncludeParents = map.ContainsKey("--include-parents") || map.ContainsKey("-ip") || map.ContainsKey("--include-relations") || map.ContainsKey("-ir");
                                        model.IncludeRelations = map.ContainsKey("--include-relations") || map.ContainsKey("-ir");

                                        var tableListToUse = tables.Where(tbl =>
                                            !exceptTables.Contains(tbl.ToLowerInvariant()));

                                        foreach (var table in tables)
                                        {
                                            
                                            model.Table = repo.GetEntity(table.Trim(), true).Result;
                                            model.Project = project;

                                            log.Add($"use table {model.Table.Name} for template");
                                            var result = template.Render(model);

                                            progressIndicator++;
                                            statistics[totalItemCountLabel] = progressIndicator;

                                            totalResult.Append("\n" + result);
                                            var export = s.Contains("--export") || s.Contains("--export-bykey") || s.Contains("--export-bytype") || s.Contains("-ek") || s.Contains("-e");
                                            var exportByType = s.Contains("--export-bytype") || s.Contains("--export-bykey") || s.Contains("-ek");

                                            if (export)
                                            {
                                                var outputPath = string.Empty;
                                                var tmpPath = string.Empty;

                                                if (exportByType && !string.IsNullOrEmpty(template.ExportType))
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
                                                    tmpPath = map.ContainsKey("--export") ? map["--export"].Replace("\"", "") : "";
                                                }


                                                outputPath = tmpPath;

                                                if (string.IsNullOrEmpty(tmpPath))
                                                {
                                                    outputPath = Path.Combine(Directory.GetCurrentDirectory(), "temp-output");
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
                                                var writeFile =
                                                    !File.Exists(filePath) || map.ContainsKey("--overwrite") ||
                                                    map.ContainsKey("-o");
                                                if (writeFile)
                                                {
                                                    System.IO.File.WriteAllText(filePath, result);
                                                    statistics[totalExportCountLabel] = ++totalExportCount;
                                                }
                                                
                                                
                                                //Console.ForegroundColor = ConsoleColor.DarkGreen;
                                                //Console.WriteLine($"{model.Table.Name} ble generert til {filePath}...");
                                                //Console.ResetColor();
                                            }
                                            
                                            var message = $"{template.Name}, {model.Table.Name}";

                                            ConsoleExtensions.ShowPercentProgress("Oppretter Innhold... ", progressIndicator, totalItemsToGenerate);
                                        }

                                        //Write progress here
                                        
                                    }
                                }
                            }

                            //CommandExtensions.ShowPercentProgress("Oppretter Innhold... ", 100, 100);

                            
                            var end = DateTime.Now;
                            var skipClipboard = s.Contains("--skip-clipboard") || s.Contains("-sc");

                            if (!skipClipboard && totalResult.Length > 0)
                            {
                                ModelHelperExtensions.ToClipboard(totalResult.ToString());

                                Console.ForegroundColor = ConsoleColor.DarkGreen;
                                //Console.WriteLine($"{model.Table.Name} ble generert til {filePath}...");
                                //

                                Console.WriteLine("\n\nNB! Innholdet er også kopiert til utklippstavlen, bruk Ctrl + v for å lime inn hvis du trenger det");
                                Console.ResetColor();
                            }


                            if ((s.Contains("--show-result") || s.Contains("--show-output") || s.Contains("--show")) && totalResult.Length > 0)
                            {
                                ConsoleExtensions.WriteConsoleSubTitle("\nResultat av rendring");
                                Console.Write(totalResult.ToString());
                            }

                            var span = new TimeSpan(end.Ticks - start.Ticks);
                            

                            Console.WriteLine("\n\nStatistikk:");
                            foreach (var item in statistics)
                            {
                                Console.WriteLine($"{item.Key.PadRight(25)}: {item.Value.ToString().PadLeft(5)}");
                            }

                            var timeSaved = Math.Sqrt(totalItemsToGenerate * span.TotalSeconds);
                            Console.WriteLine($"\nTid: {span.TotalSeconds} sekunder, eller {span.TotalMilliseconds} millisekunder, eller {Math.Round(timeSaved)} sparte arbeidstimer (husk å fakturere...) ");
                            Console.WriteLine("\n'Model Helper' den gode hjelper :-)");
                        }


                    }

                }
                catch (Exception exception)
                {

                    Console.WriteLine("\n\nKunne ikke generere innhold, sjekk innstillingene");
                    Console.WriteLine(exception.Message);

                    if (exception.InnerException != null)
                    {
                        Console.WriteLine(exception.InnerException.Message);
                    }

                    Console.WriteLine("Log");
                    foreach (var logItem in log)
                    {
                        Console.WriteLine(logItem);
                    }
                }

            };

            return command;
        }

        [Obsolete]
        private static List<string> GetTemplatesFromGroups(string[] groups, List<TemplateFile> templateFiles)
        {
            var items = new List<string>();
            var templateReader = new JsonTemplateReader();

            foreach (var templateFile in templateFiles)
            {
                //var fileInfo = new FileInfo(customFile);
                try
                {
                    var template = templateReader.Read(templateFile.FileInfo.FullName);
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
                catch(Exception exception)
                {

                }
                
                
            }
            return items;

            
        }

        [Obsolete]
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
        
        public static Command Script()
        {
            var command = new Command
            {
                Key = "script",
                ShortDescription = "Med script kan du teste ulike snippets og script før du bruker de.",
                //Options = new List<Option>
                //{
                //    new Option{Key = "--test", HelpMessage = "Tester en skriptlinje og returnerer til Console- vinduet"},
                    
                //}

            };

            command.Execute = (args) =>
            {
                //CommandExtensions.WriteTitle("Script");

                var model = new TemplateModel
                {
                    Table = new Table
                    {
                        Alias = "t",
                        Name = "Test_Table",
                        Schema = "dbo",
                        RowCount = 0,
                        Columns = new List<IColumn>
                        {
                            new Column{DataType = "int", IsIdentity = true, IsPrimaryKey = true, IsIgnored = false, IsNullable = false, Name = "Id", PropertyName = "Id"},
                            new Column{DataType = "varchar", IsIdentity = false, IsPrimaryKey = false, IsIgnored = false, IsNullable = true, Name = "F_Name", PropertyName = "FirstName"},
                        },
                        
                    },
                    Project = new Project
                    {
                        //Api = new ProjectApiModel { UseLogger = false, UseTelemetry = false, UseUserRepository = false},
                        DataSource = new ProjectSourceSection { Connection = "blank"},
                        Customer = "ModelHelper",
                        Name = "Test"
                    }
                };

                var script = args.Length > 0 && !args[0].StartsWith("-") ? args[0] : "";

                if (!string.IsNullOrEmpty(script))
                {
                    var result = script.Render(model);
                    Console.WriteLine(result);
                }                

            };

            return command;
        }

        [Obsolete]
        public static Command Template()
        {
            var command = new Command
            {
                Key = "template",
                ShortDescription = "Jobb med maler",
                Options = new List<Option>
                {
                    new Option{Key = "--new", HelpMessage = "Oppretter et nytt prosjekt"},  
                    new Option{Key = "--copy-from <template>", HelpMessage = "Oppretter et nytt prosjekt"},
                    new Option{Key = "--code-from <code-file>", HelpMessage = "Brukes sammen med --new for å kopiere kode fra den sp prosjektfilen"},
                    new Option{Key = "--fetch-remote", HelpMessage = "Oppretter et nytt prosjekt"},
                    new Option{Key = "--simple", HelpMessage = "Oppretter et nytt prosjekt"}
                }
                
            };

            command.Execute = args =>
            {
                
                if (!ModelHelperExtensions.ProjectFileExists())
                {
                    Console.WriteLine("Prosjektfil eksisterer ikke");
                    return;
                    
                }

                Console.WriteLine(command.ShortDescription);

                var showList = true; //s.Contains("--list") || s.Contains("-l");
                var showContent = args.Contains("--show");
                var templateName = args.Length > 0 && !args[0].StartsWith("-") ? args[0] : "";
                var fetchRemote = args.Contains("--fetch-remote") || args.Contains("-fr");
                var overwrite = args.Contains("--overwrite");
                var verbose = args.Contains("--verbose");
                var simple = args.Contains("--simple");

                var modelHelperData = ConsoleExtensions.UserTemplateDirectory();                    

                if (!Directory.Exists(modelHelperData))
                {
                    Directory.CreateDirectory(modelHelperData);
                }

                if (args.Contains("--new"))
                {
                    
                }

                if (fetchRemote)
                {
                    var remoteLocation = ModelHelperConfig.RemoteTemplateLocation;

                    ModelHelperExtensions.FetchRemoteTemplates(remoteLocation, modelHelperData, overwrite,
                        (index, total) => ConsoleExtensions.ShowPercentProgress("Laster ned maler... ", index, total));
                }

                var templateReader = new JsonTemplateReader();
                var customTemplatePath = Path.Combine(Directory.GetCurrentDirectory(), "templates");

                if (string.IsNullOrEmpty(templateName))
                {
                                       
                    var templateFiles = new List<TemplateFile>();

                    templateFiles.AddRange(customTemplatePath.GetTemplateFiles("custom"));
                    templateFiles.AddRange(modelHelperData.GetTemplateFiles(""));
                                        
                    var maxLenName = (templateFiles.Count > 0 ? templateFiles.Max(f => f.Name.Length) : 10) + 5;
                    
                    if (!simple)
                    {
                        //{"ExportFileName".ToString().PadRight(30)}
                        Console.WriteLine($"{"\nName".PadRight(maxLenName)}{"Language".PadRight(15)}{"Type".PadRight(15)}{"ExportKey".PadRight(20)}{"CanExport".ToString().PadRight(15)}");
                        Console.WriteLine($"{"".PadRight(maxLenName + 75 + 20 - 30, '-')}");
                    }
                        
                    foreach (var templateFile in templateFiles)
                    {
                        //var fileInfo = new FileInfo(customFile);

                        if (!simple)
                        {
                            try
                            {
                                var template = templateReader.Read(templateFile.FileInfo.FullName);

                                
                                if (template != null)
                                {
                                    Console.WriteLine(
                                    $"{templateFile.Name.PadRight(maxLenName)}{template.Language.PadRight(15)}{templateFile.Location.PadRight(15)}{template.ExportType.PadRight(20)}{template.CanExport.ToString().PadRight(15)}");
                                    
                                }

                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e);
                                
                            }
                        }
                        else
                        {
                            var fileName = templateFile.Name.Substring(0, templateFile.Name.LastIndexOf("."));
                            Console.WriteLine($"{fileName}");
                        }

                    }
                    
                }
                else
                {
                    var selectedTemplate = Path.Combine(modelHelperData, templateName + ".json");
                    //Console.WriteLine("Selected template: " + selectedTemplate);

                    ConsoleExtensions.WriteConsoleTitle("Valgt mal " + templateName);
                    var template = templateReader.Read(selectedTemplate);

                    if (template != null)
                    {
                        Console.WriteLine(template.Name);
                        Console.WriteLine(template.Body);
                    }
                }

            };

            return command;
        }

        [Obsolete]
        public static Command Tables()
        {
            var command = new Command
            {
                Key = "table",
                ShortDescription = "Viser en oversikt over alle tabeller eller en spesifikk tabell",
                Options = new List<Option>
                {                    
                    new Option{Key = "--model", IsOptional = true, HelpMessage = "Viser innholdet i model- data slik den tas inn i en template"},
                    new Option{Key = "--describe", IsOptional = true, HelpMessage = "Viser innholdet i model- data slik den tas inn i en template"},
                    new Option{Key = "--analyze", IsOptional = true, HelpMessage = "Gjør en analyse av data i tabellen, viser resultat som en tabell"},
                    new Option{Key = "--groups", IsOptional = true, HelpMessage = "Viser tabeller gruppert etter hvilke som hører sammen"},
                    new Option{Key = "--suggest-groups", IsOptional = true, HelpMessage = "Foreslår tabellgrupper etter hvilke som logisk hører sammen"},
                    new Option{Key = "--suggest-keys", IsOptional = true, HelpMessage = "Foreslår fremmednøkler dersom det ikke finnes på den valgte tabellen og gir en mal for å opprette"},
                    new Option{Key = "--data", IsOptional = true, HelpMessage = "Viser første 10 rader i tabellen"},
                    //new Option{Key = "--project", IsOptional = true, HelpMessage = "Overstyrer --template og --table ved å bruke model og innholdet i denne som grunnlag for en JSON- fil"},
                }
            };

            command.Execute = args =>
            {
                var tableName = args.Length > 0 && !args[0].StartsWith("-") ? args[0] : "";
                var projectReader = new ProjectJsonReader();
                var project = projectReader.Read(Path.Combine(Directory.GetCurrentDirectory(), ".model-helper"));

                if (project != null)
                {
                    var repo = new ModelHelper.Data.SqlServerRepository(project.DataSource.Connection, project);
                    

                    if (!string.IsNullOrEmpty(tableName))
                    {
                        if (args.Contains("--suggest-groups"))
                        {
                            var groups = repo.SuggestEntityGroups().Result;

                            foreach (var group in groups)
                            {
                                Console.WriteLine("Group: " + group.Name);
                                foreach (var entity in group.Entities)
                                {
                                    Console.WriteLine($"{entity.Schema}.{entity.Name}");
                                }
                            }
                        }
                        else if (args.Contains("--analyze") || args.Contains("--describe") || args.Contains("--model") || args.Contains("--data"))
                        {
                            if (args.Contains("--analyze"))
                            {                                
                                ListAnalyzeResult(tableName, repo);
                            }
                            
                            if (args.Contains("--describe"))
                            {                               
                                DescribeTable(tableName, repo);
                            }

                            if (args.Contains("--model"))
                            {
                                var model = new TemplateModel();
                                model.IncludeChildren = args.Contains("--include-children") || args.Contains("--include-relations");
                                model.IncludeParents = args.Contains("--include-parents") || args.Contains("--include-relations");
                                model.IncludeRelations = args.Contains("--include-relations");

                                model.Table = repo.GetEntity(tableName.Trim(), true).Result;
                                model.Project = project;

                                var jsonModel = JsonConvert.SerializeObject(model, Formatting.Indented);
                                Console.WriteLine(jsonModel);

                            }

                            if (args.Contains("--data"))
                            {
                                Console.WriteLine("data -- coming soon");
                            }
                        }
                        
                        else
                        {
                            ListTableContent(tableName, repo);

                            if (args.Contains("--evaluate"))
                            {
                                var table = repo.GetEntity(tableName, true).Result;
                                var evaluator = new TableEvaluator();
                                var evaluatorResult = evaluator.Evaluate(table);
                                ConsoleExtensions.WriteConsoleSubTitle("Table evaluation");
                                if (evaluatorResult.Result != EvaluationResultOption.Passes)
                                {
                                    Console.WriteLine(evaluatorResult.Message);                                    
                                }
                                else
                                {
                                    Console.WriteLine("Tabellen ser perfekt ut :-)");
                                }
                            }
                        }                        
                        
                    }
                    else
                    {
                        ListTables(repo, args.Contains("--evaluate"));
                    }
                    
                }
                

            };
            return command;
        }

        private static void PrintProjectInfo()
        {
            var projectReader = new ProjectJsonReader();
            var project = projectReader.Read(Path.Combine(Directory.GetCurrentDirectory(), ".model-helper"));

            ConsoleExtensions.WriteConsoleSubTitle($"Prosjekt");

            if (project != null)
            {
                var connectionString = new SqlConnectionStringBuilder(project.DataSource.Connection);
                var list = new Dictionary<string, string> {
                    {"Navn", project.Name },
                    {"Kunde", project.Customer },
                    {"Server", connectionString.DataSource },
                    {"Database", connectionString.InitialCatalog },

                };
                var padding = list.Max(i=> i.Key.Length) + 1;

                foreach(var item in list)
                {
                    Console.WriteLine($"{ item.Key.PadRight(padding)}{":".PadRight(3)} {item.Value}");
                }
                                
            }
            else
            {
                Console.WriteLine($"Finner ingen gyldig .model-helper fil på {Path.Combine(Directory.GetCurrentDirectory())}");
            }
        }
        private static void DescribeTable(string tableName, SqlServerRepository repo)
        {
            var tableDef = tableName.Split('.');
            var schema = tableDef.Length > 1 ? tableDef[0] : "dbo";
            var name = tableDef.Length == 1 ? tableDef[0] : tableDef.Length == 2 ? tableDef[1] : tableName;
            var columns = repo.GetColumns(schema, name).Result.ToList();

            var maxLenName = columns.Max(c => c.Name.Length);
            var maxLenType = columns.Max(c => c.DataType.Length);

            ConsoleExtensions.WriteConsoleSubTitle($"Beskrivelse av {tableName}");
            

            Console.WriteLine(
                $"{"Name".PadRight(maxLenName + 5)}{"DataType".PadRight(20)}{"IsIdentity".ToString().PadRight(15)}{"IsPrimaryKey".ToString().PadRight(15)}");
            Console.WriteLine($"{"".PadRight(maxLenName + 55, '-')}");
            foreach (var column in columns)
            {
                Console.WriteLine(
                    $"{column.Name.PadRight(maxLenName + 5)}{column.DataType.PadRight(20)}{column.IsIdentity.ToString().PadRight(15)}{column.IsPrimaryKey.ToString().PadRight(15)}");
            }
        }

        private static void ListAnalyzeResult(string tableName, SqlServerRepository repo)
        {
            var tableDef = tableName.Split('.');
            var schema = tableDef.Length > 1 ? tableDef[0] : "dbo";
            var name = tableDef.Length == 1 ? tableDef[0] : tableDef.Length == 2 ? tableDef[1] : tableName;
            var columns = repo.GetColumns(schema, name).Result.ToList();

            var maxLenName = columns.Max(c => c.Name.Length);
            var maxLenType = columns.Max(c => c.DataType.Length);

            ConsoleExtensions.WriteConsoleSubTitle($"Analyseresulat for {tableName}");            

            Console.WriteLine(
                $"{"Name".PadRight(maxLenName + 5)}{"DataType".PadRight(20)}{"IsIdentity".ToString().PadRight(15)}{"IsPrimaryKey".ToString().PadRight(15)}");
            Console.WriteLine($"{"".PadRight(maxLenName + 55, '-')}");
            foreach (var column in columns)
            {
                Console.WriteLine(
                    $"{column.Name.PadRight(maxLenName + 5)}{column.DataType.PadRight(20)}{column.IsIdentity.ToString().PadRight(15)}{column.IsPrimaryKey.ToString().PadRight(15)}");
            }
        }
        private static void ListTableContent(string tableName, SqlServerRepository repo)
        {
            var tableDef = tableName.Split('.');
            var schema = tableDef.Length > 1 ? tableDef[0] : "dbo";
            var name = tableDef.Length == 1 ? tableDef[0] : tableDef.Length == 2 ? tableDef[1] : tableName;

            var table = repo.GetEntity(tableName, true).Result;
            //var columns = repo.GetColumns(schema, name).Result.ToList();

            var maxLenName = table.Columns.Max(c => c.Name.Length);
            var maxLenType = table.Columns.Max(c => c.DataType.Length);            

            ConsoleExtensions.WriteConsoleTitle($"Tabellinnhold for {tableName}");
            ConsoleExtensions.WriteConsoleSubTitle("Kolonner");            

            Console.WriteLine(
                $"{"Name".PadRight(maxLenName + 5)}{"DataType".PadRight(20)}{"Nullable".PadRight(15)}{"IsIdentity".ToString().PadRight(15)}{"PK".ToString().PadRight(15)}{"FK".ToString().PadRight(15)}{"Collation".ToString().PadRight(30)}");
            Console.WriteLine($"{"".PadRight(maxLenName + 55 + 60, '-')}");
            foreach (var column in table.Columns)
            {
                var collation = !string.IsNullOrEmpty(column.Collation) ? column.Collation : "";
                Console.WriteLine(
                    $"{column.Name.PadRight(maxLenName + 5)}{column.DataType.PadRight(20)}{column.IsNullable.ToString().PadRight(15)}{column.IsIdentity.ToString().PadRight(15)}{column.IsPrimaryKey.ToString().PadRight(15)}{column.IsForeignKey.ToString().PadRight(15)}{collation.PadRight(30)}");
            }

            if (table.ChildRelations.Any() || table.ParentRelations.Any())
            {
                
                if (table.ChildRelations.Any())
                {
                    ConsoleExtensions.WriteConsoleSubTitle("en til mange", "(model.Table.ChildRelations)");
                    var relLenName = table.ChildRelations.Max(c => c.Name.Length) + 5;
                    var relLenParentCol = table.ChildRelations.Max(c => c.ParentColumnName.Length) + 5;
                    var relLenChildCol = table.ChildRelations.Max(c => c.ChildColumnName.Length) + 5;
                    var totalLength = relLenName + relLenChildCol + relLenParentCol + 10;

                    Console.WriteLine(
                        $"{"Schema".PadRight(10)}{"Table".PadRight(relLenName)}{"Child Col".PadRight(relLenChildCol)}{"Parent Col".PadRight(relLenParentCol)}");

                    Console.WriteLine($"{"".PadRight(totalLength, '-')}");

                    foreach (var relation in table.ChildRelations)
                    {
                        Console.WriteLine($"{relation.Schema.PadRight(10)}{relation.Name.PadRight(relLenName)}{relation.ChildColumnName.PadRight(relLenChildCol)}{relation.ParentColumnName.PadRight(relLenParentCol)}");
                    }
                }
                

                
                if (table.ParentRelations.Any())
                {
                    Console.WriteLine("");
                    ConsoleExtensions.WriteConsoleSubTitle("mange til en", "(model.Table.ParentRelations)");

                    var relLenName = table.ParentRelations.Max(c => c.Name.Length) + 5;
                    var relLenParentCol = table.ParentRelations.Max(c => c.ParentColumnName.Length) + 5;
                    var relLenChildCol = table.ParentRelations.Max(c => c.ChildColumnName.Length) + 5;
                    var totalLength = relLenName + relLenChildCol + relLenParentCol + 10;

                    Console.WriteLine(
                        $"{"Schema".PadRight(10)}{"Table".PadRight(relLenName)}{"Child Col".PadRight(relLenChildCol)}{"Parent Col".PadRight(relLenParentCol)}");

                    Console.WriteLine($"{"".PadRight(totalLength, '-')}");

                    foreach (var relation in table.ParentRelations)
                    {
                        Console.WriteLine($"{relation.Schema.PadRight(10)}{relation.Name.PadRight(relLenName)}{relation.ChildColumnName.PadRight(relLenChildCol)}{relation.ParentColumnName.PadRight(relLenParentCol)}");
                    }
                }
               
                
            }
            
        }

        private static void ListTables(SqlServerRepository repo, bool evaluate = false)
        {
            var tables = repo.GetTables().Result.ToList();

            var maxLenName = tables.Max(t=> t.Name.Length) + 10;
            var maxLenType = 20;

            ConsoleExtensions.WriteConsoleTitle($"Tabeller");
            
            Console.WriteLine(
                $"{"\n\nName".PadRight(maxLenName )}{"Type".PadRight(maxLenType)}{"Alias".PadRight(7)}{"Rows".PadLeft(15)}");
            Console.WriteLine($"{"".PadRight(maxLenName + maxLenType + 17 + 5, '-')}");

            var evaluator = new TableEvaluator();
            var message = string.Empty;

            foreach (var table in tables)
            {
                var tableName = $"{table.Schema}.{table.Name}";
                var failed = false;

                if (evaluate)
                {
                    var tableObject = repo.GetEntity(tableName, true).Result;
                    
                    var evaluation = evaluator.Evaluate(tableObject);
                    if (evaluation.Result == EvaluationResultOption.Failed)
                    {
                        message = evaluation.Message;
                        failed = true;
                    }
                }
                var number = $"{table.RowCount:n0}";
                Console.WriteLine(
                    $"{tableName.PadRight(maxLenName)}{table.Type.PadRight(maxLenType)}{table.Alias.PadRight(7)}{number.PadLeft(15)}");

                if (failed)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine($"\t{message}");
                    Console.ResetColor();
                }
            }
            
        }

        [Obsolete]
        public static Command Version()
        {
            var command = new Command
            {
                Key = "version",
                ShortDescription = "Displays the version"
            };

            command.Execute = s =>
            {
                Assembly execAssembly = Assembly.GetCallingAssembly();

                AssemblyName name = execAssembly.GetName();
                
                Console.WriteLine($"Version: {name.Version.Major.ToString()}.{name.Version.Minor.ToString()} for .Net ({execAssembly.ImageRuntimeVersion})");
                              

            };
            return command;
        }

        [Obsolete]
        public static Command About()
        {
            var command = new Command
            {
                Key = "about",
                ShortDescription = "Viser informasjon om denne applikasjonen"
            };

            command.Execute = s =>
            {
                Assembly execAssembly = Assembly.GetCallingAssembly();

                AssemblyName name = execAssembly.GetName();


                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine(ConsoleExtensions.GetAsciiText());
                Console.ResetColor();

                Version().Execute(null);                
                Console.WriteLine($"App name: {name.Name}.exe");
                Console.WriteLine($"Location: {Assembly.GetExecutingAssembly().Location}");

                Console.WriteLine("");
                PrintProjectInfo();
                Console.WriteLine("");
                // commands
                Help().Execute(null);

                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("\nKlarer du å finne 'easter'- egget????\n");
                Console.ResetColor();
            };
            return command;
        }

        public static Command Help()
        {
            var command = new Command
            {
                Key = "help",
                ShortDescription = "Viser hjelpetekst for kommandoene",
                Options = new List<Option>
                {
                    new Option{Key = "<command>", HelpMessage = "Viser mer hjelp for den spesifiserte kommandoen"}
                }
            };

            command.Execute = s =>
            {
                var commands = Create();
                int cmdLen = 15;
                int descLen = 80;

                if (s == null || s.Length == 0)
                {
                    ConsoleExtensions.WriteConsoleTitle($"Help");
                    ConsoleExtensions.WriteConsoleSubTitle("Gyldige kommandoer");

                    Console.WriteLine($"{"Command".PadRight(cmdLen)}{"Description".PadRight(descLen)}");
                    Console.WriteLine($"{"".PadRight(descLen + cmdLen, '-')}");

                    foreach (var cmd in commands.OrderBy(c => c.Key))
                    {
                        Console.WriteLine($"{cmd.Key.PadRight(cmdLen)}{cmd.ShortDescription.PadRight(descLen)}");
                    }
                }
                else
                {
                    var cmd = s[0];
                    // 
                    if (commands.Any(c => c.Key == cmd))
                    {
                        var helpCommand = commands.FirstOrDefault(c => c.Key == cmd);

                        if (helpCommand != null)
                        {
                            var description = !string.IsNullOrEmpty(helpCommand.LongDescription)
                                ? helpCommand.LongDescription
                                : helpCommand.ShortDescription;

                            Console.WriteLine($"\n{description}\n");
                            Console.WriteLine($"Options:\n");

                            if (helpCommand.Options != null && helpCommand.Options.Any())
                            {
                                int optionalLen = 12;
                                int optionKeyLen = helpCommand.Options.Max(o => o.Key.Length) + 5;
                                int optionDescLen = helpCommand.Options.Max(o => o.HelpMessage.Length);

                                Console.WriteLine($"{"Option".PadRight(optionKeyLen)}{"IsOptional".PadRight(optionalLen)}{"Description".PadRight(optionDescLen)}");
                                Console.WriteLine($"{"".PadRight(optionKeyLen + optionDescLen + optionalLen, '-')}");

                                foreach (var option in helpCommand.Options)
                                {
                                    var optionKey = (!string.IsNullOrEmpty(option.Key) ? option.Key : "").PadRight(optionKeyLen);
                                    var optionDescription =
                                        (!string.IsNullOrEmpty(option.HelpMessage) ? option.HelpMessage : "")
                                        .PadRight(optionDescLen);

                                    var isOptional = option.IsOptional.ToString().PadRight(optionalLen);
                                    Console.WriteLine($"{optionKey}{isOptional}{optionDescription}");
                                }
                            }
                            
                        }
                    }
                }

                Console.WriteLine("\n");
            };

            return command;
        }
    }
}
