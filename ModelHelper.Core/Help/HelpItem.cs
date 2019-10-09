using ModelHelper.Core.CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ModelHelper.Core.Help
{
    public class HelpItem
    {
        public static HelpItem FromAttributes(Type t)
        {
            var helpItem = new HelpItem();


            var shortDescAttribute = (ShortDescriptionAttribute)Attribute.GetCustomAttribute(t, typeof(ShortDescriptionAttribute));
            var longDescAttribute = (LongDescriptionAttribute)Attribute.GetCustomAttribute(t, typeof(LongDescriptionAttribute));
            var sampleAttributes = t.GetCustomAttributes(typeof(CommandSampleAttribute), false).ToList();

            helpItem.ShortDescription = shortDescAttribute != null ? shortDescAttribute.Text : string.Empty;
            helpItem.LongDescription = longDescAttribute != null ? longDescAttribute.Text : string.Empty;
            helpItem.Samples = new List<HelpSample>();

            if (sampleAttributes != null && sampleAttributes.Any())
            {
                foreach (var sample in sampleAttributes)
                {
                    var item = (CommandSampleAttribute)sample;

                    helpItem.Samples.Add(new HelpSample { Description = item.Description, CommandText = item.CommandText, Important = item.Important });
                }

            }

            helpItem.Options = new List<HelpOption>();

            var props = t.GetProperties();
            foreach (var p in props)
            {

                var shortPropDescAttribute = (ShortDescriptionAttribute)Attribute.GetCustomAttribute(p, typeof(ShortDescriptionAttribute));
                var optionAttribute = (OptionAttribute)Attribute.GetCustomAttribute(p, typeof(OptionAttribute));

                if (optionAttribute != null)
                {
                    var optionItem = new HelpOption
                    {
                        Aliases = optionAttribute.Aliases.ToList(),
                        IsOptional = !optionAttribute.IsRequired,
                        ShortDescription = shortPropDescAttribute != null ? shortPropDescAttribute.Text : string.Empty,
                        Key = optionAttribute.Key,

                    };

                }


            }

            return helpItem;
        }

        public HelpItem()
        {
            Options = new List<HelpOption>();
            ConsoleMessages = new Dictionary<string, string>();
            Samples = new List<HelpSample>();
        }
        public string Key { get; set; }
        public string Alias { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public List<HelpOption> Options { get; set; }

        public List<HelpSample> Samples { get; set; }
        public string GetMessage(string key, params string[] args)
        {
            if (ConsoleMessages.ContainsKey(key))
            {
                return ConsoleMessages[string.Format(key, args)];
            }
            else
            {
                return string.Empty;
            }
        }
        public Dictionary<string, string> ConsoleMessages { get; set; }
        public string Title { get; set; }
        public string Signature { get; set; }
    }
}