using System;
using System.Collections.Generic;
using System.Net;
using ModelHelper.Core.Help;

namespace ModelHelper
{
    public class HelpItems
    {
        public HelpItems()
        {
            Commands = new List<HelpItem>();
        }
        public List<HelpItem> Commands { get;set; }
    }


    public static class HelpFactory
    {

        public static List<HelpItem> Create()
        {
            var help = new HelpItems();

            help.Commands.Add(Init());
            help.Commands.Add(New());
            help.Commands.Add(Project());
            help.Commands.Add(Entity());
            help.Commands.Add(ColumnMapping());
            help.Commands.Add(CodeLocation());

            return help.Commands;
        }

        internal static HelpItem Optimize()
        {
            var help = new HelpItem { Key = "optimize", Alias = "o" };

            return help;
        }

        public static HelpItem Help()
        {
            var help = new HelpItem();

            help.ConsoleMessages.Add("title", "Help");
            help.ConsoleMessages.Add("validCommands", "Gyldige kommandoer");
            help.ConsoleMessages.Add("commandHeader", "Valg");
            help.ConsoleMessages.Add("options", "valg");

            return help;
        }
        public static HelpItem New()
        {
            var item = new HelpItem();

            item.Key = "new";
            item.ShortDescription = "Creates a new .model-helper file where the mh.exe is executed from";
            item.LongDescription = $@"{item.ShortDescription}

Normally you would create this at the root of yor code project
e.g where your *.sln file if Visual Studio project or where your VS Code is started from.
";
            item.Options = new List<HelpOption>
                {
                    new HelpOption{Key = "<project-name>"},
                    // new HelpOption{Key = "--from <path to other .model-helper>", ShortDescription = "Brukes sammen med --new for å kopiere innhold fra den valgte prosjektfilen"},
                    new HelpOption{Key = "--overwrite | -o", ShortDescription = "if there is another .model-helper file it will overwrite this without warning"},                    

                };

            item.Alias = "n";
            //item.ConsoleMessages.Add("projectFileExists", "Det eksisterer en prosjektfil allerde, vil du overskrive denne (y/N)?");
            item.Samples.Add(new HelpSample
            {
                CommandText = "mh new my-project",
                Description = @"Creates a new project with the name 'my-project'
You will then be asked a set of questions:

    - Customer name:
    - Create database connection (Y/n):
    - ...
"
            });
            return item;
        }
        public static HelpItem Project()
        {
            var item = new HelpItem();

            item.Key = "project";
            item.ShortDescription = "";
            item.LongDescription = "";
            item.Options = new List<HelpOption>
                {
                    new HelpOption{Key = "--json", ShortDescription = "Show the content of the current project file as json"},                                        
                    new HelpOption{Key = "--show-options", ShortDescription = "Show the options in the current project file"},                                        
                    new HelpOption{Key = "--show-locations", ShortDescription = "Show the code locations for the current project file"},                                        
                    new HelpOption{Key = "--location-add", ShortDescription = "Add a new code location. You will be asked for a key, namespace and physical path"},                                        
                    new HelpOption{Key = "--location-set <key>", ShortDescription = ""},                                        
                    new HelpOption{Key = "--location-remove <key>", ShortDescription = "Removes a code location"},
                    new HelpOption{Key = "--column-add", ShortDescription = ""},                                        
                    new HelpOption{Key = "--column-set columnName", ShortDescription = ""},                                        
                    new HelpOption{Key = "--column-remove columnName", ShortDescription = "removes the column name"},                                        
                    new HelpOption{Key = "--option-add", ShortDescription = "Add an option"},                                        
                    new HelpOption{Key = "--option-set <key>", ShortDescription = "Edits the content of the option"},                                        
                    new HelpOption{Key = "--option-remove <key>", ShortDescription = "Removed an option"},                                        
                    //new Option{Key = "--open", HelpMessage = "Opens the .model-helper file in VSCode"},
                              
                    // --set-query-option
                    // --set-default-schema
                    // --set-db-connection
                    // --set-code-connection
                    // --enable-query-options
                    // --disable-query-options                    
                };

            item.Alias = "p";
            item.Samples = new List<HelpSample>
            {
                new HelpSample
                {
                    CommandText = "mh project --column-add",
                    Description = @"This will add a new column to the columnMapping list.
You will be asked about:
    ColumnName:
    PropertyName:     
    TranslatedName:    
    Should this column be ignored? (y/N):
    Indicates row changed date (y/N):
    Indicates row changed by (y/N):
    Indicates row creation date (y/N):
    Indicates row created by user (y/N):
    Marks that the row is deleted (y/N):

Read more about the usage of the column mapping typing
> mh help ColumnMapping
"
                },
                new HelpSample
                {
                    CommandText = "mh project --location-add",
                    Description = @"This will add a new column to the columnMapping list.
You will be asked about:
    Key (must be unique, e.g: api.interfaces):
    Namespace (e.g: Customer.Project.Infrastructure.Interfaces): 
    Path (e.g: .\where\to\put\the\file): 

A location is used by the template to add a correct namespace and to export the file when
the template shares the same key.

Read more about the usage of Code Location typing
> mh help CodeLocation
"
                }
            };
            
            return item;
        }
        public static HelpItem Init()
        {
            var item = new HelpItem();

            item.ConsoleMessages.Add("title", "Welcome to Model Helper CLI");
            item.ConsoleMessages.Add("welcomeMessage", @"
Welcome to Model Helper CLI. With this tool you can connect to a database and generate
code based on different templates.

This is the first time you start this tool and then you need to configure and retrieve some templates.

Some general templates have been created that will be downloaded from a central register. It is also
possible to create custom-specific or general templates.

In order to connect to the database, the program needs to know a little about the project you are working on.
For each project, a .model-helper file is created. 

run the command 'mh help' for further assitance ");

            item.ConsoleMessages.Add("runSetup", "\nRun setup (Y/n)? ");
            item.ConsoleMessages.Add("afterInstall", @"
There is now a config.json file here '{0}'.
this is where you find all the 'main templates'.

{1} template(s) was downloaded and is now ready to be used'.
Run the command 'mh template' to list all available templates.
");
            item.ConsoleMessages.Add("showValidCommands", "\nShow valid commands (Y/n)? ");
            item.ConsoleMessages.Add("listTemplates", "List templates (Y/n)? ");            
            item.ConsoleMessages.Add("downloadTemplate", "Downloading templates");            

            return item;
        }

        public static HelpItem Entity()
        {
            var item = new HelpItem();

            item.Key = "entity";
            item.ShortDescription = "Lists all entities (table and views) or the content of a named table or view";
            item.LongDescription = "";
            //item.Signature = "table [tablename] --evaluate";
            item.Options = new List<HelpOption>
            {                
                new HelpOption{Key = "--evaluate", ShortDescription = "Evaluates the structure of an entity"},
                new HelpOption{Key = "--model", ShortDescription = "Shows the complete model that is sent to the template engine"},                
                new HelpOption{Key = "--model-entity", ShortDescription = "Shows the entity part of the model that is sent the template engine"},
                // new HelpOption{Key = "--dump \"<path>\"", ShortDescription = "Dumps the selected entity or set of entities to a json file."},                
                new HelpOption{Key = "--view-only", ShortDescription = "Lists only views"},
                new HelpOption{Key = "--table-only", ShortDescription = "Lists only tables"},
                // new HelpOption{Key = "--analyze", ShortDescription = "Lists only tables"},
            };
                        
            item.Samples = new List<HelpSample>
            {
                new HelpSample
                {
                    CommandText = "mh entity\nor\n> mh e",
                    Description = "List all tables and views"
                },
                new HelpSample
                {
                    CommandText = "mh entity Name%",
                    Description = "Show a list of tables and views starting with Name"
                },
                new HelpSample
                {
                    CommandText = "mh entity %Name%",
                    Description = "Show a list of tables and views containing Name"
                },
                new HelpSample
                {
                    CommandText = "mh entity YourEntityName",                    
                    Description = @"Lists the column structure and relations for a named entity

If you omit the schema from tablename it will asume the default schema",                    

                },
                new HelpSample
                {
                    CommandText = "mh entity YourEntityName --evaluate",                    
                    Description = @"Using the --evaluate options will fire a set of validation rules for a entity:

    * Does the table include a Primary Key?
    * Checks if the table has an identity column is also a Primary Key
    * Checks if columns that ends with ID also has a constraint
    * Checks if text columns uses the same collation",
                },
                new HelpSample
                {
                    CommandText = "mh entity YourEntityName --model",
                    Description = "Use this command to see how the template engine uses this entity",                    
                }
            };
            return item;
        }


        public static HelpItem ColumnMapping()
        {
            var item = new HelpItem();
            item.Key = "ColumnMapping";
            item.Title = "Extended help for the ColumnMapping";
            item.LongDescription = @"
It's important to understand how the columnMapping works
You will find the columnMapping in the project.database.columnMapping section of the .model-helper.

";

            item.Samples = new List<HelpSample>
            {
                new HelpSample
                {
                    CommandText = "mh project --column-mapping",
                    Description = "This command will show you the column mapping for the current project"
                }
            };
            return item;
        }

        public static HelpItem CodeLocation()
        {
            var item = new HelpItem();
            var sampleString = "{{model.Namespaces[\"api.interfaces\"]}}";
            item.Key = "CodeLocation";
            item.Title = "Extended help for Code location";
            item.LongDescription = $@"
In order for the template engine to produce code and files that is unique to your project 
it's important to understand how a code location in the project file relates to the 
templates 'exportType' property.

A template can only define one code location key in the 'exportType' property. 
When the template engine is exporting the result using the '--export-bytype' option it will use
the value found in the templates 'exportType' property and match this against the 
first occurance in the code.locations list.

If a match is found the 'path' property will be used together with the template 'exportFileName' property

A template can also use a code location inside the template to find the the proper namespace for a class

Example: using {sampleString};

You will find the code location in the project.code.locations section in the .model-helper file.

";

            item.Samples = new List<HelpSample>
            {
                new HelpSample
                {
                    CommandText = "mh project --location-list",
                    Description = "This command will show you the code locations for the current project"
                },
                new HelpSample
                {
                    CommandText = "mh project --location-add",
                    Description = @"Adds a new location to the location list. You will be asked a few questions about 
the location key, path and namespace"
                }
            };
            return item;
        }

        public static HelpItem Config()
        {
            var help = new HelpItem();

            return help;
        }

        public static HelpItem Generate()
        {
            var help = new HelpItem();
            help.Title = "Help for the generate command";
            help.Key = "generate";
            help.Alias = "g";
            help.ShortDescription = "Generates code based on the selected entity and template";
            help.LongDescription = $@"
{help.ShortDescription}

Use various options to decide how and what to generate.
For --entity and --template it's possible to select more entities by separating each with , (comma).

Example: mh generate --entity {"\""}entity1, entity2{"\""} --template {"\""}tpl1, tpl2{"\""}

The example above will generate a total of 4 code files (entity count * template count)

If --export option is provided without a path specification the generated code will be created
in a temp folder.

To export to a file the template the 'canExport' property must be set to true.
";
            help.Options = new List<HelpOption>
            {
                new HelpOption{Key = "--entity <entity-name> | -e <entity-name>", Aliases = new List<string>{"e"}, IsOptional = false, ShortDescription = "Specifies the entity or set of entitys to use as base for the model."},
                new HelpOption{Key = "--entity-group <group-name> |  -eg <group-name>", IsOptional = false, ShortDescription = "Angir hvilke tabeller som logisk hører sammen, som skal brukes mot valgte mal."},
                new HelpOption{Key = "--except <list-of-entities>", IsOptional = false, ShortDescription = "Hvis man har bestemt om man skal bruke alle (*) eller en tabell gruppe, bortsett fra..."},
                new HelpOption{Key = "--view-only | -vo", IsOptional = false, ShortDescription = "Only list or filter views"},
                new HelpOption{Key = "--table-only | -to", IsOptional = false, ShortDescription = "Only list or filter tables"},
                new HelpOption{Key = "--template <template-name> | -t <template-name>", Aliases = new List<string>{"t"}, IsOptional = false, ShortDescription = "Angir malen for å brukes mot den valgte tabellen"},
                new HelpOption{Key = "--template-group <group-name> |-tg <group-name>", IsOptional = false, ShortDescription = "Angir hvilke grupperte maler som skal brukes mot den valgte tabellen"},
                new HelpOption{Key = "--export \"[<directory>]\"", IsOptional = true, ShortDescription = "Bestemmer hvor den genererte koden skal skrives til."},
                new HelpOption{Key = "--export-bykey | -ek", IsOptional = true, ShortDescription = "Bestemmer hvor den genererte koden skal skrives til."},
                new HelpOption{Key = "--show-result | --show | -s", IsOptional = true, ShortDescription = "Viser resultatet av koden i console- vinduet"},
                new HelpOption{Key = "--overwrite | -o", IsOptional = true, ShortDescription = "Hvis --export er valgt og filen allerede eksisterer så vil denne overskrive uten advarsel"},
                new HelpOption{Key = "--skip-clipboard | -sc", IsOptional = true, ShortDescription = "Legger ikke resultate i Clipboard,"},
                new HelpOption{Key = "--include-children | -ic", IsOptional = true, ShortDescription = "Sets a flag to indicate if the template model should use child relataions"},
                new HelpOption{Key = "--include-parents | -ip", IsOptional = true, ShortDescription = "Sets a flag to indicate if the template model should use parent relataions"},
                new HelpOption{Key = "--include-relations | -ir", IsOptional = true, ShortDescription = "Sets a flag to indicate if the template model should use either parent or child relataions"},
                new HelpOption{Key = "--help | -h", IsOptional = true, ShortDescription = "Shows help for this command"},
            };

            help.Samples = new List<HelpSample>
            {
                new HelpSample
                {
                    CommandText = "mh generate --entity * --template sql-select-all --show\nor...\n> mh g -e * -t sql-select-all -s",
                    Description = "This will generate code for all entities in the database"
                },
                new HelpSample
                {
                    CommandText = "mh generate --entity * --table-only --template sql-select-all --show\nor...\n> mh g -e * -to -t sql-select-all -s",
                    Description = "This will generate code for all tables found in the database"
                },
                new HelpSample
                {
                    CommandText = "mh generate --entity * --view-only --template sql-select-all --show\nor...\n> mh g -e * -vo -t sql-select-all -s",
                    Description = "This will generate code for all views found in the database"
                },
                new HelpSample
                {
                    CommandText = "mh generate --entity * --except \"Order, OrderItem\" --template sql-select-all --show",
                    Description = "This will generate code for all entities in the database except for the Order and OrderItem table"
                },
                new HelpSample
                {
                    CommandText = "mh generate --entity \"Order, OrderItem\" --template sql-select-all --show",
                    Description = "This will generate code for both Order and OrderItem entity "
                },                
                new HelpSample
                {
                    CommandText = "mh g --entity Order% --template sql-select-all --show",
                    Description = @"This will generate code for all entities that starts with Order 
By including either --table-only or --view-only you can further limit the collection of the returned entities"
                },                
                new HelpSample
                {
                    CommandText = "mh g --entity myTable --template-group CoreWebApi --show",
                    Description = @"This will generate code for the myTable entity using a template group called CoreWebApi. 
The number of code files generated depends on how many templates that is specified in the group."
                },
                new HelpSample
                {
                    CommandText = "mh generate --entity * --template-group CoreWebApi -ir -sc --export-bykey",
                    Description = @"This will generate code for all tables and views in the database using the templates found in
the CoreWebApi template group. If the database contains 20 tables and views and the CoreWebApi consist of 5 templates 
(Controller, Entity model, Repository, Interface and View model) a total of 20 * 5 = 100 code files will be created based on 
the code.location section of the .model-helper project file.
Read more about setting up the project file with path and nameapsace for the --export-bykey:
> mh help CodeLocation
"

                },
            };
            return help;
        }
    }
}