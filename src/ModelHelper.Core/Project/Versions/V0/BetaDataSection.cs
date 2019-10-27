using System.Collections.Generic;

namespace ModelHelper.Core.Project.V0
{
    public class BetaDataSection
    {
        public BetaDataSection()
        {
            QueryOption = new BetaQueryOption();
            ColumnExtras = new List<BetaColumnExtra>();
            IgnoredColumns = new List<string>();
            Translations = new List<KeyValuePair<string, string>>();
            NameMap = new List<KeyValuePair<string, string>>();
        }

        public string DataSourceType { get; set; }

        public string Connection { get; set; }
        public string ConnectionInterface { get; set; }
        public string ConnectionVariable { get; set; }
        public string ConnectionMethod { get; set; }

        
        public bool UseQueryOptions { get; set; }
        
        public string QueryOptionsClassName { get; set; }

        public BetaQueryOption QueryOption { get; set; }

        public List<string> IgnoredColumns { get; set; }

        public List<KeyValuePair<string, string>> Translations { get; set; }
        public List<KeyValuePair<string, string>> NameMap { get; set; }

        public List<BetaColumnExtra> ColumnExtras { get; set; }

    }
}