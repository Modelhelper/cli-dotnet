using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Builder;

namespace ModelHelper.Cli.Commands
{
    public class CommonOptions
    {
        public Option Env
        {
            get
            {
                var option = new Option("--env", "Sets the environment that the url should point to. Default value: Prod");
                option.AddAlias("--environment");
                option.Argument = new Argument<Environment>(defaultValue: () => Environment.Prod).WithSuggestions("apple", "banana", "cherry");

                return option;
            }
        }
        public Option TemplateGroup
        {
            get
            {
                var option = new Option("--template-group", "A list of template groups to use for code generation");
                option.AddAlias("-tg");
                option.AddAlias("-grp");
                option.Argument = new Argument<List<string>>();

                return option;

            }
        }


        public Option Entities
        {
            get
            {
                var option = new Option("--entity", "One or more tables to be used in the given template");
                option.AddAlias("-e");
                option.Argument = new Argument<List<string>>();

                return option;

            }
        }

        public Option ExceptEntities
        {
            get
            {
                var option = new Option("--except", "Excludes the named table(s) for code generation");
                option.AddAlias("--except-table");
                option.Argument = new Argument<List<string>>();

                return option;

            }
        }

        public Option EntityGroups
        {
            get
            {
                var option = new Option("--entity-group", "One or more template groups to use in code generation");
                option.AddAlias("-eg");
                option.Argument = new Argument<List<string>>();

                return option;

            }
        }

        public Option Templates
        {
            get
            {
                var option = new Option("--template", "The group that the list belongs to");
                option.AddAlias("-t");
                option.Argument = new Argument<List<string>>();

                return option;

            }
        }

        public Option ExportByKey
        {
            get
            {
                var option = new Option("--export-bykey", "The group that the list belongs to");
                option.AddAlias("-ek");
                option.Argument = new Argument<bool>(defaultValue: () => false);

                return option;

            }
        }

        public Option OverwriteFiles
        {
            get
            {
                var option = new Option("--overwrite", "The group that the list belongs to");
                option.AddAlias("-o");
                option.Argument = new Argument<bool>(defaultValue: () => false);

                return option;

            }
        }

        public Option ExportToPath
        {
            get
            {
                var option = new Option("--export", "The group that the list belongs to");
                option.Argument = new Argument<string>();

                return option;

            }
        }

        public Option WithConnection
        {
            get
            {
                var option = new Option("--connection", "The group that the list belongs to");
                option.AddAlias("-c");
                option.Argument = new Argument<string>();

                return option;

            }
        }

        public Option IncludeRelations
        {
            get
            {
                var option = new Option("--include-relations", "The group that the list belongs to");
                option.AddAlias("-ir");
                option.AddAlias("-r");
                option.Argument = new Argument<bool>(defaultValue: () => false);

                return option;

            }
        }
        public Option ShowResult
        {
            get
            {
                var option = new Option("--show", "The group that the list belongs to");
                option.AddAlias("-s");
                option.Argument = new Argument<bool>(defaultValue: () => false);

                return option;

            }
        }

        public Option SkipClipboard
        {
            get
            {
                var option = new Option("--skip-clipboard", "The group that the list belongs to");
                option.AddAlias("-sc");
                option.Argument = new Argument<bool>(defaultValue: () => false);

                return option;

            }
        }

        public Option Verbose
        {
            get
            {
                var option = new Option("--verbose", "The group that the list belongs to");
                option.AddAlias("-v");
                option.Argument = new Argument<bool>(defaultValue: () => false);

                return option;

            }
        }



        public Option ViewsOnly
        {
            get
            {
                var option = new Option("--view-only", "The group that the list belongs to");
                option.AddAlias("-vo");
                option.Argument = new Argument<bool>(defaultValue: () => false);

                return option;

            }
        }
        public Option TablesOnly
        {
            get
            {
                var option = new Option("--table-only", "The group that the list belongs to");
                option.AddAlias("-to");
                option.Argument = new Argument<bool>(defaultValue: () => false);

                return option;

            }
        }
    }
}