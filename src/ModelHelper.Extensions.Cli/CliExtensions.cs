using ModelHelper.Extensions.Presentation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ModelHelper.Extensions.Cli
{
    public static class CliExtensions
    {
        
        public static string GetSlogan()
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
                "colourful",
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
            var sloganString = asciiArt[ascIndex];

            return sloganString;
        }
    
        public static void WriteSlogan()
        {
            var slogan = GetSlogan();
            $"\n\n{slogan}".WriteConsoleGray();
        }
   

    public static void WriteToConsole(this Table table, bool includeHeader = true)
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

                System.Console.WriteLine(builder.ToString());
                
            }

            var maxWidth = pads.Sum(p => p.Value);
            if (table.UseHeader)
            {
                System.Console.WriteLine($"{"".PadRight(maxWidth, '-')}");
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

                System.Console.WriteLine(builder.ToString());
            }
            
            
            
        }
    
    
            public static Table ToConsoleTable<T>(this IEnumerable<T> source, params string[] properties) where T : class
        {
            var table = new Table();
            var list = source.ToList();
            if (list.Any())
            {
               // var t = source.First().GetType();

                var rows = new List<Row>();
                var header = list.First().ToTableHeader(properties);
                //var propertyList = properties.Length > 0 ? props.Where(p => properties.Contains(p.Name)).ToList() : props.ToList();

                foreach (var s in list)
                {
                    var row = s.ToTableRow(header);
                    rows.Add(row);
                }

                table.Header = header;
                table.Rows = rows;
            }
            
            return table;
        }
        internal static Row ToTableHeader<T>(this T source, params string[] properties)
        {
            var row = new Row();

            var props = source.GetType().GetProperties();
            var propertyList = properties.Length > 0 ? props.Where(p => properties.Contains(p.Name)).ToList() : props.ToList();

            row.Values = propertyList.Select(p =>
                new RowValue
                {
                    Value = p.Name,
                    Alignment = p.PropertyType == typeof(int) ? RowValueAlignment.Right : RowValueAlignment.Left
                }).ToList();
            
            return row;
        }

        internal static Row ToTableRow<T>(this T source, Row header)
        {
            var row = new Row();

            var props = source.GetType().GetProperties();
            var propertyList = header != null && header.Values.Any()
                ? props.Where(p => header.Values.Select(h => h.Value).Contains(p.Name)).ToList() //.Select( h => new { h.Value, h.Alignment }
                : props.ToList();

            
            row.Values = propertyList.Select(p => new RowValue
            {
                Value = p.GetValue(source).ToString(),
                Alignment = p.PropertyType == typeof(int) ? RowValueAlignment.Right : RowValueAlignment.Left
            }).ToList();
            
            return row;
        }
        public static string WriteConsoleSuccess(this string input)
        {
            System.Console.ForegroundColor = ConsoleColor.Green;
            System.Console.WriteLine(input);
            System.Console.ResetColor();

            return input;
        }

        public static string WriteConsoleWarning(this string input)
        {
            System.Console.ForegroundColor = ConsoleColor.Yellow;
            System.Console.WriteLine(input);
            System.Console.ResetColor();

            return input;
        }

        public static string WriteConsoleError(this string input)
        {
            System.Console.ForegroundColor = ConsoleColor.Red;
            System.Console.WriteLine(input);
            System.Console.ResetColor();

            return input;
        }

        public static string WriteConsoleGray(this string input)
        {
            System.Console.ForegroundColor = ConsoleColor.DarkGray;
            System.Console.WriteLine(input);
            System.Console.ResetColor();

            return input;
        }

        public static string WriteConsoleCommand(this string command)
        {
            System.Console.ForegroundColor = ConsoleColor.White;
            System.Console.WriteLine($"\n> {command}");
            System.Console.ResetColor();

            return command;
        }

        
        public static void WriteConsoleLogo()
        {
            System.Console.ForegroundColor = ConsoleColor.Blue;
            System.Console.WriteLine(GetAsciiText());
            System.Console.ResetColor();
            
        }

        public static void ShowPercentProgress(string message, int currElementIndex, int totalElementCount, string operation = "")
        {
            //if (currElementIndex < 0 || currElementIndex > totalElementCount)
            //{
            //    throw new InvalidOperationException("currElement out of range");
            //}
            int percent = (100 * (currElementIndex)) / totalElementCount;
            if (percent > 100) percent = 100;

            var operationString = string.IsNullOrEmpty(operation) ? "" : "=> " + operation;
            System.Console.Write("\r{0}: {1}% {2}", message, percent.ToString().PadLeft(4), operationString);
            if (currElementIndex == totalElementCount - 1)
            {
                System.Console.WriteLine(Environment.NewLine);
            }
            
        }

        public static void WriteConsoleTitle(this string title)
        {
            var screenLen = 100;
            var totalLen = title.Length > screenLen ? title.Length : screenLen;
            var len = title.Length;
            var padLeft = ((totalLen - len) / 2) + len;
            //Console.ForegroundColor = ConsoleColor.DarkBlue;
            System.Console.WriteLine("\n");
            System.Console.WriteLine("".PadRight(screenLen, '-'));
            System.Console.WriteLine($"{title.ToUpper().PadLeft(padLeft)}");
            System.Console.WriteLine("".PadRight(screenLen, '-'));
            System.Console.ResetColor();
        }

        public static void WriteConsoleSubTitle(this string subTitle, string small = "")
        {
            System.Console.WriteLine($"\n  {subTitle.ToUpper()} {small}\n");
            //Console.WriteLine("".PadRight(subTitle.Length, '-'));            
        }

        public static void WriteIfContent(this string input)
        {
            if (!string.IsNullOrEmpty(input))
            {
                System.Console.WriteLine(input);
            }            
        }

        public static string UserTemplateDirectory()
        {
            var appData = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            var modelHelperData =
                Path.Combine(appData, "ModelHelper", "templates");

            return modelHelperData;
        }

        public static string GetAsciiText()
        {
            var ascii = @"

        :::   :::    ::::::::  :::::::::  :::::::::: :::  
      :+:+: :+:+:  :+:    :+: :+:    :+: :+:        :+:   
    +:+ +:+:+ +:+ +:+    +:+ +:+    +:+ +:+        +:+    
   +#+  +:+  +#+ +#+    +:+ +#+    +:+ +#++:++#   +#+     
  +#+       +#+ +#+    +#+ +#+    +#+ +#+        +#+      
 #+#       #+# #+#    #+# #+#    #+# #+#        #+#       
###       ###  ########  #########  ########## ########## 
      :::    ::: :::::::::: :::        :::::::::  :::::::::: ::::::::: 
     :+:    :+: :+:        :+:        :+:    :+: :+:        :+:    :+: 
    +:+    +:+ +:+        +:+        +:+    +:+ +:+        +:+    +:+  
   +#++:++#++ +#++:++#   +#+        +#++:++#+  +#++:++#   +#++:++#:    
  +#+    +#+ +#+        +#+        +#+        +#+        +#+    +#+    
 #+#    #+# #+#        #+#        #+#        #+#        #+#    #+#     
###    ### ########## ########## ###        ########## ###    ###      

";


            var ascii2 = @"
888b     d888               888          888 888    888          888                           
8888b   d8888               888          888 888    888          888                           
88888b.d88888               888          888 888    888          888                           
888Y88888P888  .d88b.   .d88888  .d88b.  888 8888888888  .d88b.  888 88888b.   .d88b.  888d888 
888 Y888P 888 d88""""88b d88"" 888 d8P  Y8b 888 888    888 d8P  Y8b 888 888 ""88b d8P  Y8b 888P""   
888  Y8P  888 888  888 888  888 88888888 888 888    888 88888888 888 888  888 88888888 888     
888   ""   888 Y88..88P Y88b 888 Y8b.     888 888    888 Y8b.     888 888 d88P Y8b.     888     
888       888  ""Y88P""   ""Y88888  ""Y8888  888 888    888  ""Y8888  888 88888P""   ""Y8888  888     
                                                                     888                       
                                                                     888                       
                                                                     888                       
";
            return ascii2;
        }


        public static string ToStringTable<T>(
    this IEnumerable<T> values,
    string[] columnHeaders,
    params Func<T, object>[] valueSelectors)
        {
            return ToStringTable(values.ToArray(), columnHeaders, valueSelectors);
        }

        

        public static string ToStringTable<T>(
          this T[] values,
          string[] columnHeaders,
          params Func<T, object>[] valueSelectors)
        {
            

            var arrValues = new string[values.Length + 1, valueSelectors.Length];

            // Fill headers
            for (int colIndex = 0; colIndex < arrValues.GetLength(1); colIndex++)
            {
                arrValues[0, colIndex] = columnHeaders[colIndex];
            }

            // Fill table rows
            for (int rowIndex = 1; rowIndex < arrValues.GetLength(0); rowIndex++)
            {
                for (int colIndex = 0; colIndex < arrValues.GetLength(1); colIndex++)
                {
                    arrValues[rowIndex, colIndex] = valueSelectors[colIndex]
                      .Invoke(values[rowIndex - 1]).ToString();
                }
            }

            return ToStringTable(arrValues);
        }

        public static string ToStringTable(this string[,] arrValues)
        {
            int[] maxColumnsWidth = GetMaxColumnsWidth(arrValues);
            var headerSpliter = new string('-', maxColumnsWidth.Sum(i => i + 3) - 1);

            var sb = new StringBuilder();
            for (int rowIndex = 0; rowIndex < arrValues.GetLength(0); rowIndex++)
            {
                for (int colIndex = 0; colIndex < arrValues.GetLength(1); colIndex++)
                {
                    // Print cell
                    string cell = arrValues[rowIndex, colIndex];
                    cell = cell.PadRight(maxColumnsWidth[colIndex]);
                    sb.Append(" | ");
                    sb.Append(cell);
                }

                // Print end of line
                sb.Append(" | ");
                sb.AppendLine();

                // Print splitter
                if (rowIndex == 0)
                {
                    sb.AppendFormat(" |{0}| ", headerSpliter);
                    sb.AppendLine();
                }
            }

            return sb.ToString();
        }

        private static int[] GetMaxColumnsWidth(string[,] arrValues)
        {
            var maxColumnsWidth = new int[arrValues.GetLength(1)];
            for (int colIndex = 0; colIndex < arrValues.GetLength(1); colIndex++)
            {
                for (int rowIndex = 0; rowIndex < arrValues.GetLength(0); rowIndex++)
                {
                    int newLength = arrValues[rowIndex, colIndex].Length;
                    int oldLength = maxColumnsWidth[colIndex];

                    if (newLength > oldLength)
                    {
                        maxColumnsWidth[colIndex] = newLength;
                    }
                }
            }

            return maxColumnsWidth;
        }
    }
}
