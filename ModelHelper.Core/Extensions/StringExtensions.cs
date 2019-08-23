using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Humanizer;
using ModelHelper.Core.Project;
using ModelHelper.Core.Project.V1;
using Newtonsoft.Json;

namespace ModelHelper.Core.Extensions
{
    public static class StringExtensions
    {
        public static List<string> WordExceptions = new List<string>
        {
            "process", "status"
        };

        public static string AsWords(this string input)
        {
            var words = input.Humanize(LetterCasing.Title);

            return words;
        }
        
        public static string PluralizeWord(this string word)
        {
            //if (WordExceptions.Any(s => s.EndsWith(word.ToLower())))
            //{
            //    return word;
            //}
            var lastPos = word.LastUpperCasePosition();
            var toPlural = word.Substring(lastPos);

            var pluralizied = toPlural.Pluralize();
            if (string.IsNullOrEmpty(pluralizied))
            {
                return word;
            }
            if (lastPos > 0)
            {
                var rest = word.Substring(0, lastPos);
                return rest + pluralizied;
            }
            else
            {
                return pluralizied;
            }
           
        }

        public static string SingularizeWord(this string word)
        {
            if (WordExceptions.Any(s =>word.ToLowerInvariant().EndsWith(s)))
            {
                return word;
            }

            var lastPos = word.LastUpperCasePosition();
            var toSingular = word.Substring(lastPos);

            var singular = toSingular.Singularize();

            if (string.IsNullOrEmpty(singular))
            {
                return word;
            }

            if (lastPos > 0)
            {
                var rest = word.Substring(0, lastPos);
                return rest + singular;
            }
            else
            {
                return singular;
            }
            
        }

        private static int LastUpperCasePosition(this string input)
        {
            int lastPos = 0;
            for (int x = 0; x < input.Length; x++)
            {
                if (Char.IsUpper(input[x]))
                {
                    lastPos = x;
                }
            }

            return lastPos;
        }

        public static bool IsMultiWord(this string input)
        {
            if (!string.IsNullOrEmpty(input))
            {
                var multiWord = input.Contains("_") || input.Contains("-") || input.Contains(" ");
                return multiWord;
            }
            else
            {
                return false;
            }

        }

        public static string GetAbrevation(this string input)
        {
            var separators = new string[]
            {
                " ",
                "-",
                ",",
                "_"
            };

            var words = input.Split(separators, StringSplitOptions.RemoveEmptyEntries);

            var sb = new StringBuilder();
            foreach (var word in words)
            {
                sb.Append(word[0].ToString().ToLowerInvariant());
            }

            return sb.ToString();

        }
        public static string AsKebabCase(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            var test = input.AsUpperCamelCase();
            return Regex.Replace(
                    test,
                    "(?<!^)([A-Z][a-z]|(?<=[a-z])[A-Z])",
                    "-$1",
                    RegexOptions.Compiled)
                .Trim()
                .ToLower();
        }

        public static string AsSnakeCase(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            var test = input.AsUpperCamelCase();
            return Regex.Replace(
                    test,
                    "(?<!^)([A-Z][a-z]|(?<=[a-z])[A-Z])",
                    "_$1",
                    RegexOptions.Compiled)
                .Trim()
                .ToLower();
        }


        public static string NameMapValue(this string input, List<KeyValuePair<string, string>> nameMap)
        {
            var result = input;

            if (nameMap != null && nameMap.Any(n => String.Equals(n.Key, input, StringComparison.CurrentCultureIgnoreCase)))
            {
                var item = nameMap.FirstOrDefault(k => String.Equals(k.Key, input, StringComparison.CurrentCultureIgnoreCase));
                result = !string.IsNullOrEmpty(item.Value) ? item.Value : input;
            }
            return result;
        }

        public static string NameMapValue(this string input, List<ProjectDataColumnMapping> nameMap)
        {
            var result = input;

            if (nameMap != null && nameMap.Any(n => String.Equals(n.Name, input, StringComparison.CurrentCultureIgnoreCase)))
            {
                var item = nameMap.FirstOrDefault(k => String.Equals(k.Name, input, StringComparison.CurrentCultureIgnoreCase));
                result = !string.IsNullOrEmpty(item.PropertyName) ? item.PropertyName : input;
            }
            return result;
        }

        public static bool IsAllUppercase(this string input)
        {
            var countUpper = input.Count(char.IsUpper);
            var letterCount = input.Length;
            var allUpper = countUpper == letterCount;
            //var allUpper = input.Any(char.IsUpper);
            return allUpper;
        }

        public static string AsUpperCamelCase(this string input)
        {
            var isMultiWord = input.IsMultiWord();

            input = input.CleanInput();
            //!isMultiWord ? input :
            //var testcasing = input
            //    .Replace('_', ' ')
            //    .Replace('-', ' ');

            //if (testcasing.EndsWith("ID"))
            //{
            //    testcasing = input.Replace("ID", "Id");
            //}

            TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo; // new TextInfo(CultureInfo.CurrentCulture);
            var result = isMultiWord
                ? textInfo.ToTitleCase(input.ToLowerInvariant()).Trim().Replace(" ", "")
                : input.IsAllUppercase()
                    ? input[0].ToString().ToUpperInvariant() + input.Substring(1).ToLowerInvariant()
                    : input[0].ToString().ToUpperInvariant() + input.Substring(1);

            return result;
        }

        public static string CleanInput(this string input)
        {
            if (!string.IsNullOrEmpty(input))
            {
                var testcasing = input
                    .Replace('_', ' ')
                    .Replace('-', ' ');

                if (testcasing.EndsWith("ID"))
                {
                    testcasing = testcasing.Replace("ID", "Id");
                }

                return testcasing;
            }
            else
            {
                return "";
            }
        }

        public static string AsLowerCamelCase(this string input)
        {
            input = input.AsUpperCamelCase();

            var result = input[0].ToString().ToLowerInvariant() + input.Substring(1);
            return result;
        }

        public static T GetJson<T>(this IEnumerable<string> input, bool isArray = true)
        {
            var sb = new StringBuilder();

            foreach (var i in input)
            {
                sb.Append(i);
            }

            if (sb != null)
            {
                if (isArray)
                {
                    var jsonObj = JsonConvert.DeserializeObject<List<T>>(sb.ToString());
                    if (jsonObj != null)
                    {
                        return jsonObj.FirstOrDefault();
                    }
                }
                else
                {
                    var jsonObj = JsonConvert.DeserializeObject<T>(sb.ToString());
                    if (jsonObj != null)
                    {
                        return jsonObj;
                    }
                }
            }

            return default(T);
        }
    }
}