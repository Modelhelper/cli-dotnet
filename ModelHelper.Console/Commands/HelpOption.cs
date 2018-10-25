using System.Collections.Generic;

namespace ModelHelper.Commands
{
    public class HelpOption
    {
        public string Key { get; set; }
        public List<string> Aliases { get; set; }

        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }

        public bool IsOptional { get; set; }
    }
}