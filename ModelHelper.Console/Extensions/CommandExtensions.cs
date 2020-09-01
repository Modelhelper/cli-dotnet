using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using ModelHelper.Core.Help;
using ModelHelper.Core.Project;
using ModelHelper.Core.Project.V1;

namespace ModelHelper.Extensions
{
    public static class CommandExtensions
    {

        public static void WriteSlogan()
        {
            
            var preps = new List<string>
            {
                "cool",
                "friendly",
                "fantastic",
                "helpful",
                "humble",
                "faithful",
                "powerful",
                "adventurous",
                "adaptable",
                "ambitious",
                "generous",
                "passionate",
                "practical",
                "rational",
                "resourceful",
                "sensible",
                "sincere",
                "exuberant",
                "diligent",
                "accurate",
                "coherent",
                "colorful",
                "concise",
                "confident",
                "courageous",
                "credible",
                "creative",
                "cuddly",
                "cultivated",
                "cushy",
                "darling",
                "dashing",
                "decent",
                "decorous",
                "dedicated",
                "deliberate",
                "delightful",
                "demonstrative",
                "dependable",
                "determined",
                "diplomatic",
                "disarming",
                "distinguished",
                "dynamic",
                "eager",
                "efficient",
                "effortless",
                "electric",
                "engaging",
                "enduring",
                "enormous",
                "enriching",
                "enthusiastic",
                "fancy",
                "fascinating",
                "far-sighted",
                "far-sighted",
                "faultless",
                "fast",
                "favorable",
                "favorite",
                "flamboyant",
                "forgiving",
                "friendly",
                "fruitful",
                "fulfilling",
                "futuristic",
                "generous",
                "gentle",
                "gleeful",
                "goodhearted",
                "good-humored",
                "good-looking",
                "graceful",
                "greathearted",
                "hard-working",
                "harmonious",
                "heroic",
                "high-powered",
                "hopeful",
                "idealistic",
                "immediate",
                "impeccable",
                "independent",
                "innocent",
                "innovative",
                "jiggish",
                "jazzy",
                "kind",
                "knowable",
                "knowledgeable",
                "lionhearted",
                "luminous",
                "magnificentv",
                "marvelous",
                "motivational",
                //"",
                //"",
                //"",
                //"",
                //"",
                //"",
                //"",
                //"",
                //"",
                //"",
            };
            var rnd = new Random();
            var index = rnd.Next(preps.Count);
            var slogan = $"'Model-Helper' the {preps[index]} helper...";

            var asciiArt = new List<string>
            {
                $@"(>'-')> <('_'<) ^('_')\- \m/(-_-)\m/ <( '-')> \_( .')> < ( ._.) -`
 {slogan}
",
                $",.-~*´¨¯¨`*·~-.¸-( {slogan} )-,.-~*´¨¯¨`*·~-.¸",
                $"¸¸♬·¯·♩¸¸♪·¯·♫¸¸ {slogan} ¸¸♬·¯·♩¸¸♪·¯·♫¸¸",
                $"-=iii=<()  ♪·¯·♫¸  {slogan}",
                $"(¯`·._.·(¯`·._.·(¯`·._.·  {slogan}  ·._.·´¯)·._.·´¯)·._.·´¯)"
        };
            
            var asciiRnd = new Random();
            var ascIndex = asciiRnd.Next(asciiArt.Count);
            var print = asciiArt[ascIndex];
            $"\n\n{print}".WriteConsoleGray();
        }

        public static void WriteToConsole(this HelpItem help, bool verbose = true)
        {
            if (help == null)
            {
                Console.WriteLine("Sorry.... No help is provided for this command :-(");
                return;
            }

            if (!string.IsNullOrEmpty(help.Title))
            {
                help.Title.WriteConsoleTitle();
                
            }

            Console.Write(help.LongDescription);

            if (help.Options.Any())
            {
                "\n\nOptions:".WriteConsoleTitle();               
                foreach (var option in help.Options)
                {
                    if (verbose)
                    {
                        var aliasBuilder = new StringBuilder();


                        if (option.Aliases.Any())
                        {
                            aliasBuilder.Append("(aliases: ");
                            for (int i = 0; i < option.Aliases.Count; i++)
                            {
                                var separator = i == 0 ? "" : " | ";
                                aliasBuilder.Append($"{separator}{option.Aliases[i]}");
                            }

                            aliasBuilder.Append(")");



                        }

                        Console.WriteLine($"{option.Key} {aliasBuilder}");

                        var descripton = !string.IsNullOrEmpty(option.LongDescription)
                            ? option.LongDescription
                            : option.ShortDescription;

                        descripton.WriteConsoleGray();
                        //Console.WriteLine(descripton);
                        Console.WriteLine();
                    }
                    else
                    {

                    }
                }
            }

            if (help.Samples.Any())
            {
                "Samples".WriteConsoleTitle();
                

                foreach (var sample in help.Samples.Where( s => !string.IsNullOrEmpty(s.CommandText)))
                {

                    sample.CommandText.WriteConsoleCommand();

                    if (!string.IsNullOrEmpty(sample.Description))
                    {
                        sample.Description.WriteConsoleGray();                        
                    }

                    if (!string.IsNullOrEmpty(sample.Important))
                    {
                        $"\n{sample.Important}".WriteConsoleWarning();
                    }
                }
            }
            /*
             *
             * if ( commands.Any(c => c.Key == cmd.Key))
                {
                    var helpCommand = commands.FirstOrDefault(c => c.Key == cmd.Key);

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
             */
           
        }

        public static void WriteToConsole(this ConsoleTable table, bool includeHeader = true)
        {
            table.UseHeader = includeHeader;
            var pads = new Dictionary<int, int>();
            var margin = 5;
            for (int i = 0; i < table.Rows.Count; i++)
            {
                
                    for (int j = 0; j < table.Rows[i].Values.Count; j++)
                    {
                        var len = table.Rows[i].Values[j].Value.Length + margin;
                        if (!pads.ContainsKey(j))
                        {
                            pads.Add(j, len);
                        }
                        else
                        {
                            if (len > pads[j])
                            {
                                pads[j] = len;
                            }
                        }
                }
                    
                    
            }

            

            if (table.UseHeader)
            {

                var builder = new StringBuilder();
                for (int i = 0; i < table.Header.Values.Count; i++)
                {
                    var header = table.Header.Values[i];
                    var len = string.IsNullOrEmpty(header.Value) ? margin : header.Value.Length + margin;
                    if (pads.ContainsKey(i))
                    {
                        if (len > pads[i])
                        {
                            pads[i] = len;
                        }
                    }
                    else
                    {
                        pads.Add(i, len);
                    }

                    if (header.Alignment == RowValueAlignment.Left)
                    {
                        builder.Append($"{header.Value.PadRight(pads[i])}");
                    }
                    else
                    {
                        builder.Append($"{header.Value.PadLeft(pads[i])}");
                    }
                }

                Console.WriteLine(builder.ToString());
                
            }

            var maxWidth = pads.Sum(p => p.Value);
            if (table.UseHeader)
            {
                Console.WriteLine($"{"".PadRight(maxWidth, '-')}");
            }

            foreach (var row in table.Rows)
            {
                var builder = new StringBuilder();
                for (int i = 0; i < row.Values.Count; i++)
                {
                    var val = row.Values[i];
                    var pad = pads[i];
                    
                    if (val.Alignment == RowValueAlignment.Left)
                    {
                        
                        builder.Append($"{val.Value.PadRight(pads[i])}");
                    }
                    else
                    {
                        builder.Append($"{val.Value.PadLeft(pads[i])}");
                    }
                    //builder.Append($"{val.Value.PadRight(pad)}");
                }

                Console.WriteLine(builder.ToString());
            }
            
            
            
        }

        public static void WriteToConsole(this IProject project )
        {
            

            $"Current ModelHelper Project".WriteConsoleSubTitle();

            if (project != null)
            {


                var list = new Dictionary<string, string>
                {
                    {"Name", project.Name},
                    {"Customer", project.Customer},
                    {
                        "Version", project.Version
                        
                    }
                };

                var padding = list.Max(i => i.Key.Length) + 1;

                var connections = project.Data.Connections.Select(i => new {
                        Name = i.Name, i.DbType, DefaultSchema = string.IsNullOrEmpty(i.DefaultSchema) ? "": i.DefaultSchema,
                        Server = new SqlConnectionStringBuilder(i.ConnectionString).DataSource,
                        Database = new SqlConnectionStringBuilder(i.ConnectionString).InitialCatalog

                    });

                
                foreach (var item in list)
                {
                    Console.WriteLine($"{ item.Key.PadRight(padding)}{":".PadRight(3)} {item.Value}");
                }


                $"Connections".WriteConsoleSubTitle();
                connections.ToConsoleTable().WriteToConsole();

                "\n\nFor more about the current project use the 'mh project --verbose' command".WriteConsoleWarning();
            }
            else
            {
                
                $"\nFinner ingen gyldig .model-helper fil på {Path.Combine(Directory.GetCurrentDirectory())}"
                    .WriteConsoleError();
            }
        }

        public static void PrintProjectInfo()
        {
            var projectReader = new DefaultProjectReader();
            var project = projectReader.Read(Path.Combine(Directory.GetCurrentDirectory(), ".model-helper"));

            WriteToConsole(project);

            
        }
    }
}