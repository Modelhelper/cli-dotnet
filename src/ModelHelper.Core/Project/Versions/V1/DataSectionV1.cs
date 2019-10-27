using ModelHelper.Project.V2;
using System;
using System.Collections.Generic;

namespace ModelHelper.Core.Project.V1
{

    [Obsolete("Moved to version 1 to 2 converter")]
    public class DataSectionV1
    {
        public DataSectionV1()
        {
            QueryOption = new QueryOption();
            ColumnExtras = new List<ColumnExtraV1>();
            IgnoredColumns = new List<string>();
            Translations = new List<KeyValuePair<string, string>>();
            NameMap = new List<KeyValuePair<string, string>>();
        }

        public string DataSourceType { get; set; }

        public string Connection { get; set; }
        public string ConnectionInterface { get; set; }
        public string ConnectionVariable { get; set; }
        public string ConnectionMethod { get; set; }

        [Obsolete]
        public bool UseQueryOptions { get; set; }

        [Obsolete]
        public string QueryOptionsClassName { get; set; }

        public QueryOption QueryOption { get; set; }

        public List<string> IgnoredColumns { get; set; }

        public List<KeyValuePair<string, string>> Translations { get; set; }
        public List<KeyValuePair<string, string>> NameMap { get; set; }

        public List<ColumnExtraV1> ColumnExtras { get; set; }
    }
}