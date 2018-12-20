using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using ModelHelper.Core.Project;
using ModelHelper.Core.Rules;
using ModelHelper.Core.Templates;
using ModelHelper.Data;
using ModelHelper.Extensions;
using Newtonsoft.Json;
using ModelHelper.Core.CommandLine;
using ModelHelper.Extensibility;
using System.Text;

namespace ModelHelper.Commands
{
    [Export(typeof(ICommand))]
    [ExportMetadata("Key", "entity")]
    public class EntityCommand : BaseCommand
    {
        public EntityCommand()
        {
            Key = "entity";
            Alias = "e";
            Help = HelpFactory.Entity();
        }

        [Option(Key ="--evaluate", Aliases = new[] { "-e" })]
        public bool Evaluate { get; set; }

        [Option(Key = "--model", Aliases = new[] { "-m" })]
        public bool ShowModelContent { get; set; }

        [Option(Key = "--model-entity", Aliases = new[] { "-mt", "--model-table" })]
        public bool ShowModelTableContent { get; set; }

        [Option(Key = "--view-only", IsRequired = false, Aliases = new []{"-vo"})]
        public bool ViewsOnly { get; set; } = false;

        [Option(Key = "--table-only", IsRequired = false, Aliases = new []{"-to"})]
        public bool TablesOnly { get; set; } = false;

        [Option(Key = "--description-only", IsRequired = false, Aliases = new[] { "-do" })]
        public bool DescriptionOnly { get; set; } = false;

        [Option(Key = "--with-description", IsRequired = false, Aliases = new[] { "-d" })]
        public bool WithDescription { get; set; } = false;
        //[Option(Key = "--describe", Aliases = new[] { "-d" })]
        //public bool DescribeTableLayout { get; set; }

        //[Option(Key = "--groups", Aliases = new[] { "-g" })]
        //public bool ShowGroup { get; set; }

        [Option(Key = "--except", Aliases = new []{"-e", "--except-table"})]
        public List<string> ExceptEntities { get; set; } = new List<string>();

        [Option(Key = "--dump", IsRequired = false, ParameterIsRequired = true, ParameterProperty = "DumpPath", Aliases = new []{"-d"})]
        public bool Dump { get; set; } = false;
        public string DumpPath { get; set; }

        [Option(Key = "--column", IsRequired = false, ParameterIsRequired = true, ParameterProperty = "ColumnName", Aliases = new[] { "-c" })]
        public bool WithColumn { get; set; } = false;
        public string ColumnName { get; set; }

        [Option(Key = "--import", IsRequired = false, ParameterIsRequired = true, ParameterProperty = "ImportPath", Aliases = new[] { "-i" })]
        public bool Import { get; set; } = false;
        public string ImportPath { get; set; }

        [Option(Key = "--no-pk", IsRequired = false)]
        public bool WithNoPrimaryKey { get; set; } = false;

        [Option(Key = "--no-clustered", IsRequired = false)]
        public bool WithNoClusteredPrimaryKey { get; set; } = false;

        [Option(Key = "--no-fk", IsRequired = false)]
        public bool WithNoForeignKey { get; set; } = false;
        
        [Option(Key = "--traverse-relations", IsRequired = false, Aliases = new[] { "-tr", "--traverse" })]
        public bool TraverseRelations { get; set; } = false;

        //[Option(Key = "--depth", IsRequired = false, ParameterIsRequired = true, ParameterProperty = "Depth")]
        [Option(Key = "--depth", IsRequired = false)]
        public int Depth { get; set; } = 1;

        [Option(Key = "--max-level", IsRequired = false)]
        public int MaxLevel { get; set; } = -1;

        //public int Depth {get;set;};
        public override bool EvaluateArguments(IRuleEvaluator<Dictionary<string, string>> evaluator)
        {
            throw new System.NotImplementedException();
        }

        public override void Execute(List<string> arguments)
        {
            var map = ArgumentParser.Parse(this, arguments);
            var args = arguments;

            var entityName = args.Count > 0 && !args[0].StartsWith("-") ? args[0] : "";
            var projectReader = new ProjectJsonReader();
            var project = projectReader.Read(Path.Combine(Directory.GetCurrentDirectory(), ".model-helper"));
            var entityFilter = !string.IsNullOrEmpty(entityName) && entityName.Contains("%") ? entityName : "";

            if (project != null)
            {
                var repo = new ModelHelper.Data.SqlServerRepository(project.DataSource.Connection, project);

                
                if (!string.IsNullOrEmpty(entityName) && string.IsNullOrEmpty(entityFilter))
                {
                    if (Dump)
                    {


                        if (string.IsNullOrEmpty(DumpPath))
                        {
                            throw new Exception("A valid path following the --dump must be provided");
                        }

                        Console.WriteLine("Dumping the database to a json file to this location: " + DumpPath);

                        var dumpResult = repo.DumpContentAsJson(entityName, DumpPath).Result;
                    // call to external methodSystem.IO.File.WriteAllText(path, json);

                    return;
                    }

                    if (Import)
                    {
                        if (string.IsNullOrEmpty(ImportPath))
                        {
                            throw new Exception("A valid path must be provided");

                        }

                        
                        var e = repo.GetEntity(entityName).Result;

                        
                        this.ImportJsonData(repo, e, ImportPath);

                        return;
                    }
                    if (ShowModelContent || ShowModelTableContent)
                    {
                        var model = new TemplateModel();

                        if (ShowModelContent)
                        {
                            
                            model.IncludeChildren = true;
                            //args.Contains("--include-children") || args.Contains("--include-relations");
                            model.IncludeParents = true;
                            // args.Contains("--include-parents") || args.Contains("--include-relations");
                            model.IncludeRelations = true; // args.Contains("--include-relations");

                            model.Table = repo.GetEntity(entityName.Trim(), true).Result;
                            model.Project = project;

                            var jsonModel = JsonConvert.SerializeObject(model.Table, Formatting.Indented);
                            Console.WriteLine(jsonModel);
                        }
                        else if (ShowModelTableContent)
                        {
                            
                            model.IncludeChildren = false;
                            //args.Contains("--include-children") || args.Contains("--include-relations");
                            model.IncludeParents = false;
                            // args.Contains("--include-parents") || args.Contains("--include-relations");
                            model.IncludeRelations = false; // args.Contains("--include-relations");

                            model.Table = repo.GetEntity(entityName.Trim(), false).Result;                            

                            var jsonModel = JsonConvert.SerializeObject(model.Table, Formatting.Indented);
                            Console.WriteLine(jsonModel);
                        }

                    }
                    
                    else
                    {
                        //if (DescribeTableLayout)
                        //{
                        //    DescribeTable(tableName, repo);
                        //}

                        ListEntityContent(entityName, repo);

                        if (Evaluate)
                        {
                            var table = repo.GetEntity(entityName, true).Result;

                            if (table.Type.Equals("table", StringComparison.InvariantCultureIgnoreCase))
                            {


                                var evaluator = new TableEvaluator();
                                var evaluatorResult = evaluator.Evaluate(table);
                                ConsoleExtensions.WriteConsoleSubTitle("Entity evaluation");
                                if (evaluatorResult.Result != EvaluationResultOption.Passes)
                                {
                                    Console.WriteLine(evaluatorResult.Message);
                                }
                                else
                                {
                                    Console.WriteLine("This entity looks perfect :-)");
                                }
                            }
                        }
                    }
                    //if (args.Contains("--suggest-groups"))
                    //{
                    //    var groups = repo.SuggestEntityGroups().Result;

                    //    foreach (var group in groups)
                    //    {
                    //        Console.WriteLine("Group: " + group.Name);
                    //        foreach (var entity in group.Entities)
                    //        {
                    //            Console.WriteLine($"{entity.Schema}.{entity.Name}");
                    //        }
                    //    }
                    //}
                    //else if (args.Contains("--analyze") || args.Contains("--describe") || args.Contains("--model") ||
                    //         args.Contains("--data"))
                    //{
                    //    if (args.Contains("--analyze"))
                    //    {
                    //        ListAnalyzeResult(tableName, repo);
                    //    }

                    //    if (args.Contains("--describe"))
                    //    {
                    //        DescribeTable(tableName, repo);
                    //    }



                    //    if (args.Contains("--data"))
                    //    {
                    //        Console.WriteLine("data -- coming soon");
                    //    }
                    //}


                }
                else
                {
                    ListEntities(repo, Evaluate, entityFilter);
                }

            }

        }

        private void DescribeEntity(string entityName, SqlServerRepository repo)
        {
            var tableDef = entityName.Split('.');
            var schema = tableDef.Length > 1 ? tableDef[0] : "dbo";
            var name = tableDef.Length == 1 ? tableDef[0] :
                tableDef.Length == 2 ? tableDef[1] : entityName;
            var columns = repo.GetColumns(schema, name).Result.ToList();

            var maxLenName = columns.Max(c => c.Name.Length);
            var maxLenType = columns.Max(c => c.DataType.Length);

            ConsoleExtensions.WriteConsoleSubTitle($"Description of {entityName}");


            Console.WriteLine(
                $"{"Name".PadRight(maxLenName + 5)}{"DataType".PadRight(20)}{"IsIdentity".ToString().PadRight(15)}{"IsPrimaryKey".ToString().PadRight(15)}");
            Console.WriteLine($"{"".PadRight(maxLenName + 55, '-')}");
            foreach (var column in columns)
            {
                Console.WriteLine(
                    $"{column.Name.PadRight(maxLenName + 5)}{column.DataType.PadRight(20)}{column.IsIdentity.ToString().PadRight(15)}{column.IsPrimaryKey.ToString().PadRight(15)}");
            }
        }

        private void ListAnalyzeResult(string entityName, SqlServerRepository repo)
        {
            var tableDef = entityName.Split('.');
            var schema = tableDef.Length > 1 ? tableDef[0] : "dbo";
            var name = tableDef.Length == 1 ? tableDef[0] :
                tableDef.Length == 2 ? tableDef[1] : entityName;
            var columns = repo.GetColumns(schema, name).Result.ToList();

            var maxLenName = columns.Max(c => c.Name.Length);
            var maxLenType = columns.Max(c => c.DataType.Length);

            ConsoleExtensions.WriteConsoleSubTitle($"Analyseresulat for {entityName}");

            Console.WriteLine(
                $"{"Name".PadRight(maxLenName + 5)}{"DataType".PadRight(20)}{"IsIdentity".ToString().PadRight(15)}{"IsPrimaryKey".ToString().PadRight(15)}");
            Console.WriteLine($"{"".PadRight(maxLenName + 55, '-')}");
            foreach (var column in columns)
            {
                Console.WriteLine(
                    $"{column.Name.PadRight(maxLenName + 5)}{column.DataType.PadRight(20)}{column.IsIdentity.ToString().PadRight(15)}{column.IsPrimaryKey.ToString().PadRight(15)}");
            }
        }

        internal string GetColumnDescription(IColumn column)
        {
            if (column.IsIdentity | column.IsForeignKey | column.IsPrimaryKey)
            {
                var id = column.IsIdentity ? "ID" : "";
            }

            return "";
        }
        private void ListEntityContent(string entityName, SqlServerRepository repo)
        {
            var tableDef = entityName.Split('.');
            var schema = tableDef.Length > 1 ? tableDef[0] : "dbo";
            var name = tableDef.Length == 1 ? tableDef[0] :
                tableDef.Length == 2 ? tableDef[1] : entityName;

            var table = repo.GetEntity(entityName, true).Result;
            //var columns = repo.GetColumns(schema, name).Result.ToList();

            var maxLenName = table.Columns.Max(c => c.Name.Length);
            var maxLenType = table.Columns.Max(c => c.DataType.Length);


            ConsoleExtensions.WriteConsoleTitle($"Entity content for {entityName}");

            if (!string.IsNullOrEmpty(table.Description))
            {
                ConsoleExtensions.WriteConsoleGray("Description:");
                Console.WriteLine(table.Description);
            }
            
            ConsoleExtensions.WriteConsoleSubTitle("Columns");

            table.Columns.Select(c => new
            {
                Name = c.Name,
                DataType = c.DataType,
                Nullable = c.IsNullable.ToString(),
                IsIdentity = c.IsIdentity ? "ID" : "",
                PK = c.IsPrimaryKey ? "PK" : "",
                FK = c.IsForeignKey ? "FK" : "",
                // Collation = !string.IsNullOrEmpty(c.Collation) ? c.Collation : "",
                Description = !string.IsNullOrEmpty(c.Description) && c.Description.Length > 80 
                    ? c.Description.Substring(0, 80) + "..." 
                    : c.Description
            }).ToList()
            .ToConsoleTable()
            .WriteToConsole();


            if (table.Indexes.Any())
            {
                ConsoleExtensions.WriteConsoleSubTitle("Indexes");
                table.Indexes.Select(i => new
                {
                    i.Name,
                    i.IsClustered,
                    i.IsPrimaryKey,
                    i.IsUnique,
                    Fragmentation = $"{Math.Round(i.AvgFragmentationPercent)}%"
                }).ToList()
                .ToConsoleTable()
                .WriteToConsole();
            }                        

            if (table.ChildRelations.Any() || table.ParentRelations.Any())
            {

                if (table.ChildRelations.Any())
                {
                    ConsoleExtensions.WriteConsoleSubTitle("One to Many", "(model.Table.ChildRelations)");
                    
                    var relations = table.ChildRelations.Select(c => new
                    {
                        Schema = c.Schema,
                        Name = c.Name,
                        ChildCol = $"{c.ChildColumnName} ({c.ChildColumnType})",
                        ParentCol = $"{c.ParentColumnName} ({c.ParentColumnType})",
                        Constraint = c.ConstraintName
                    });

                    relations.ToList().ToConsoleTable().WriteToConsole();
                    
                }



                if (table.ParentRelations.Any())
                {
                    Console.WriteLine("");
                    ConsoleExtensions.WriteConsoleSubTitle("mange til en", "(model.Table.ParentRelations)");

                    table.ParentRelations.Select(c => new
                    {
                        Schema = c.Schema,
                        Name = c.Name,
                        ChildCol = $"{c.ChildColumnName} ({c.ChildColumnType})",
                        ParentCol = $"{c.ParentColumnName} ({c.ParentColumnType})",
                        Constraint = c.ConstraintName
                    }).ToConsoleTable().WriteToConsole();
                    
                }


            }


            if (TraverseRelations && repo.CanTraverseRelations)
            {

                "Relation Tree".WriteConsoleSubTitle();
                var relations = repo.TraverseRelations(entityName, Depth, MaxLevel).Result;

                if (relations != null)
                {

                    relations.Select(t => new {                        
                        t.FullPath
                    }).ToList().ToConsoleTable().WriteToConsole();
                }
                else
                {
                    $"Cannot traverse {entityName}".WriteConsoleError();
                }
                
            }
        }

        private void ImportJsonData(SqlServerRepository repo, IEntity entity, string dataPath)
        {
            if (File.Exists(dataPath) && entity != null)
            {
                Console.WriteLine("Begin import...");
                var json = File.ReadAllText(ImportPath);

                Console.WriteLine("Import into " + entity.Name + " from " + ImportPath);
                //Console.WriteLine(json);

                var result = repo.ImportJsonData(entity, json).Result;
                Console.WriteLine($"{result} records was inserted");
            }
        }

        private void ListEntities(SqlServerRepository repo, bool evaluate = false, string filter = "")
        {
            var columnName = WithColumn && !string.IsNullOrEmpty(ColumnName) ? ColumnName: "";
            var entities = repo.GetEntities(TablesOnly, ViewsOnly, filter, columnName).Result.ToList();

            if (entities.Count > 0)
            {


                var maxLenName = entities.Max(t => t.Name.Length) + 10;
                var maxLenType = 20;

                $"Entities".WriteConsoleTitle();
                Console.WriteLine();
                if (!evaluate)
                {

                   
                    if (WithDescription || DescriptionOnly)
                    {
                        if (DescriptionOnly)
                        {
                            entities.Select(t => new {
                                t.Name,                                
                                t.Description
                            }).ToConsoleTable().WriteToConsole();
                        }
                        else
                        {
                            entities.Select(t => new {
                                T = t.Type.Substring(0, 1),
                                Name = $"{t.Name} (rows: {t.RowCount.ToString("N0")})",                                
                                t.Description
                            }).ToConsoleTable().WriteToConsole();
                        }
                    }
                    else
                    {
                        entities.Select(t => new {
                            t.Name,
                            t.Alias,                            
                            t.Type,
                            t.RowCount,

                        }).ToConsoleTable().WriteToConsole();
                    }
                    
                    
                    
                }
                else
                {
                    var evaluator = new TableEvaluator();
                    var count = entities.Count;

                    var evals = new Dictionary<string, string>();
                    var anyFail = false;

                    for (int i = 0; i < count; i++)
                    {
                        var entity = entities[i];
                        var entityName = $"{entity.Schema}.{entity.Name}";

                        var dbEntity = repo.GetEntity($"{entity.Schema}.{entity.Name}", true).Result;
                        var result = evaluator.Evaluate(dbEntity);



                        if (result.Failed || result.HasWarnings)
                        {
                            anyFail = true;
                            $"Evaluating {entityName}..".WriteConsoleGray();
                            foreach (var evaluationResult in result.Evaluations)
                            {
                                if (evaluationResult.Result == EvaluationResultOption.Warning)
                                {
                                    evaluationResult.Message.WriteConsoleWarning();
                                    Console.WriteLine();
                                }
                                else if (evaluationResult.Result == EvaluationResultOption.Failed)
                                {
                                    evaluationResult.Message.WriteConsoleError();
                                    Console.WriteLine();
                                }
                            }

                        }



                    }

                    if (!anyFail)
                    {
                        "All tables are good to go :-)\n".WriteConsoleSuccess();
                    }
                    else
                    {
                        "The rest of the tables are good to go :-)\n".WriteConsoleSuccess();
                    }
                }

            }
            else
            {
                Console.WriteLine("No entities to list");
            }
        }

    }
}