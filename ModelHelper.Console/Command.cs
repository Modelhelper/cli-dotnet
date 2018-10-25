using System;
using System.Collections.Generic;
using System.IO;

namespace ModelHelper
{
    [Obsolete]
    public class Command
    {
        public string Key { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public List<Option> Options { get; set; }

        public Action<string[]> Execute { get; set; }

    }

    
}