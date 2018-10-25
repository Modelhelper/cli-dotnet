using System;

namespace ModelHelper
{
    [Obsolete]
    public class CommandOption
    {
        public string Key { get; set; }
        public string Argument { get; set; }

        public int Index { get; set; }

        public bool IsRequired { get; set; }
    }
}