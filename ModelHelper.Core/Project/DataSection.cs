using System;
using System.Collections.Generic;

namespace ModelHelper.Core.Project
{
    public class DataSection
    {
        public DataSection()
        {
            QueryOption = new QueryOption();
            ColumnExtras = new List<ColumnExtra>();
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

        public List<ColumnExtra> ColumnExtras { get; set; }
    }

    public class QueryOption
    {
        public bool UseQueryOptions { get; set; }
        public string ClassName { get; set; }
        public string Namespace { get; set; }
        public string UserIdProperty { get; set; }
        public string UserIdType { get; set; }
        public bool UseClaimsPrincipalExtension { get; set; }
        public string ClaimsPrincipalExtensionMethod { get; set; }
        public string ClaimsPrincipalExtensionNamespace { get; set; }



    }
    public class ColumnExtra
    {
        public string Name { get; set; }
        public string PropertyName { get; set; }
        public string Translation { get; set; }

        public bool IsIgnored { get; set; }
        public bool IsCreatedByUser { get; set; }
        public bool IsCreatedDate { get; set; }
        public bool IsModifiedByUser { get; set; }
        public bool IsModifiedDate { get; set; }
        public bool IsDeletedMarker { get; set; }

        public bool IncludeInViewModel { get; set; }
    }
}