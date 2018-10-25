using System.Collections.Generic;
using System.Linq;

namespace ModelHelper.Extensions
{
    public static class CommandArgumentExtensions
    {
        public static Dictionary<string, string> AsArgumentDictionary(this string[] input)
        {
            return input.ToList().AsArgumentDictionary();
        }

        public static Dictionary<string, string> AsArgumentDictionary(this List<string> input)
        {
            var map = new Dictionary<string, string>();

            if (input != null && input.Count > 0)
            {
                var argLength = input.Count;

                for (int i = 0; i < argLength; i++)
                {
                    var next = i + 1;
                    var arg = input[i];
                    var param = next < argLength && !input[next].StartsWith("-") ? input[next] : "";

                    if (arg.StartsWith("-"))
                    {
                        map.Add(arg.ToLowerInvariant(), param);
                    }
                }
            }

            

            return map;
        }
    }
}