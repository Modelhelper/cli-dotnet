using System;

namespace ModelHelper
{
    public class Option
    {
        public string Key { get; set; }
        public string HelpMessage { get; set; }

        public bool IsOptional { get; set; }
    }
}