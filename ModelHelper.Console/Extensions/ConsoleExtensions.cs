using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using ModelHelper.Core.Extensions;
using ModelHelper.Core.Database;

namespace ModelHelper.Extensions
{
    public static class ConsoleExtensions
    {
        public static ConsoleTable ToConsoleTable<T>(this IEnumerable<T> source, params string[] properties) where T : class
        {
            var table = new ConsoleTable();
            var list = source.ToList();
            if (list.Any())
            {
               // var t = source.First().GetType();

                var rows = new List<ConsoleTableRow>();
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
        internal static ConsoleTableRow ToTableHeader<T>(this T source, params string[] properties)
        {
            var row = new ConsoleTableRow();

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

        internal static ConsoleTableRow ToTableRow<T>(this T source, ConsoleTableRow header)
        {
            var row = new ConsoleTableRow();

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
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(input);
            Console.ResetColor();

            return input;
        }

        public static string WriteConsoleWarning(this string input)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(input);
            Console.ResetColor();

            return input;
        }

        public static string WriteConsoleError(this string input)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(input);
            Console.ResetColor();

            return input;
        }

        public static string WriteConsoleGray(this string input)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(input);
            Console.ResetColor();

            return input;
        }

        public static string WriteConsoleCommand(this string command)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"\n> {command}"); 
            Console.ResetColor();

            return command;
        }

        
        public static void WriteConsoleLogo()
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(GetAsciiText());
            Console.ResetColor();
            
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
            Console.Write("\r{0}: {1}% {2}", message, percent.ToString().PadLeft(4), operationString);
            if (currElementIndex == totalElementCount - 1)
            {
                Console.WriteLine(Environment.NewLine);
            }
            
        }

        public static void WriteConsoleTitle(this string title)
        {
            var screenLen = 100;
            var totalLen = title.Length > screenLen ? title.Length : screenLen;
            var len = title.Length;
            var padLeft = ((totalLen - len) / 2) + len;
            //Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine("\n");
            Console.WriteLine("".PadRight(screenLen, '-'));
            Console.WriteLine($"{title.ToUpper().PadLeft(padLeft)}");
            Console.WriteLine("".PadRight(screenLen, '-'));
            Console.ResetColor();
        }

        public static void WriteConsoleSubTitle(this string subTitle, string small = "")
        {            
            Console.WriteLine($"\n  {subTitle.ToUpper()} {small}\n");
            //Console.WriteLine("".PadRight(subTitle.Length, '-'));            
        }

        public static void WriteIfContent(this string input)
        {
            if (!string.IsNullOrEmpty(input))
            {
                Console.WriteLine(input);
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
            Debug.Assert(columnHeaders.Length == valueSelectors.Length);

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