using System;

namespace ModelHelper.Core.Help
{
    public class HelpSample
    {
        [Obsolete]
        public string Title { get; set; }
        [Obsolete]
        public string Introduction { get; set; }
        public string CommandText { get; set; }
        public string Description { get; set; }
        public string Important { get; set; }
    }
}