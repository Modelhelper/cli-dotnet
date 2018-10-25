using System.Collections.Generic;

namespace ModelHelper.Core.Help
{
    public class HelpOption
    {
        public HelpOption()
        {
            Aliases = new List<string>();
        }
        public string Key { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public List<string> Aliases { get; set; }

        public bool IsOptional { get; set; }
    }
}