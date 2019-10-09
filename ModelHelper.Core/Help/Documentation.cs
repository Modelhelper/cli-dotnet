using ModelHelper.Core.CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ModelHelper.Core.Help
{
    public static class Documentation
    {
        public static HelpItem FromAttributes(Type t)
        {
            var helpItem = new HelpItem();

            var metaAttribute = (CommandMetadataAttribute)Attribute.GetCustomAttribute(t, typeof(CommandMetadataAttribute));
            var shortDescAttribute = (ShortDescriptionAttribute)Attribute.GetCustomAttribute(t, typeof(ShortDescriptionAttribute));
            var longDescAttribute = (LongDescriptionAttribute)Attribute.GetCustomAttribute(t, typeof(LongDescriptionAttribute));
            var sampleAttributes = t.GetCustomAttributes(typeof(CommandSampleAttribute), false).ToList();

            helpItem.Key = metaAttribute != null ? metaAttribute.Key : string.Empty;
            helpItem.Alias = metaAttribute != null ? metaAttribute.Alias : string.Empty;

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

                try
                {
                    if (optionAttribute != null)
                    {
                        var optionItem = new HelpOption
                        {
                            Aliases = optionAttribute.Aliases != null && optionAttribute.Aliases.Any() ? optionAttribute.Aliases.ToList() : new List<string>(),
                            IsOptional = !optionAttribute.IsRequired,
                            ShortDescription = shortPropDescAttribute != null ? shortPropDescAttribute.Text : string.Empty,
                            Key = optionAttribute.Key,

                        };

                        helpItem.Options.Add(optionItem);
                    }
                }
                catch (Exception e)
                {

                    throw;
                }
                


            }

            return helpItem;
        }
    }
}