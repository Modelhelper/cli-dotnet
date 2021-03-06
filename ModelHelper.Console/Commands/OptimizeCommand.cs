﻿using System;
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
using ModelHelper.Core.Project.V1;
using ModelHelper.Extensibility;

namespace ModelHelper.Commands
{
    [Export(typeof(ICommand))]
    public class OptimizeCommand : BaseCommand
    {
        public OptimizeCommand()
        {
            Key = "optimize";
            Alias = "o";
            Help = HelpFactory.Optimize();
        }

        [Option(Key = "--index", Aliases = new[] { "-i" })]
        public bool OptimizeIndexes { get; set; }

        [Option(Key = "--reorganize")]
        public bool Reorganize { get; set; }

        [Option(Key = "--rebuild")]
        public bool Rebuild { get; set; }

        [Option(Key = "--update-statistics")]
        public bool UpdateStatistics { get; set; }

        [Option(Key = "--connection", IsRequired = false, ParameterIsRequired = true, ParameterProperty = "ConnectionName", Aliases = new[] { "-c" })]
        public bool WithConnection { get; set; } = false;
        public string ConnectionName { get; set; }

        public override bool EvaluateArguments(IRuleEvaluator<Dictionary<string, string>> evaluator)
        {
            return true;
        }

        public override void Execute(Core.ApplicationContext context)
        {
            var map = ArgumentParser.Parse(this, context.Options);

            var entityName = context.Options.Count > 0 && !context.Options[0].StartsWith("-") ? context.Options[0] : "";
            var projectReader = new DefaultProjectReader();
            var project = projectReader.Read(Path.Combine(Directory.GetCurrentDirectory(), ".model-helper"));
            var entityFilter = !string.IsNullOrEmpty(entityName) && entityName.Contains("%") ? entityName : "";

            if (project != null)
            {
                var repo = WithConnection ? project.CreateRepository(ConnectionName) : project.CreateRepository();

                if (!string.IsNullOrEmpty(entityName))
                {
                    var entity = repo.GetEntity(entityName, true).Result;

                    foreach (var index in entity.Indexes)
                    {
                        if (index.AvgFragmentationPercent >= 10 && index.AvgFragmentationPercent < 35)
                        {
                            var reorganizeResult = repo.ReorganizeIndex(index.Name, entityName).Result;
                        }
                        if (index.AvgFragmentationPercent > 35)
                        {
                            var rebuildResult = repo.RebuildIndex(index.Name, entityName).Result;
                        }
                    }

                    Console.WriteLine("Optimizing entity - " + entityName);
                }
                else
                {
                    Console.WriteLine("Optimizing database");
                    var dbOptResult = repo.OptimizeDatabase().Result;
                }
                
            }

            
        }

        internal void RebuildIndexes(IEnumerable<IIndex> indexes)
        {
            foreach(var index in indexes)
            {

            }
        }
    }
}
