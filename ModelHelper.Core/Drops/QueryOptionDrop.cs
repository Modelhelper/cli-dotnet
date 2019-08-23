using DotLiquid;
using ModelHelper.Core.Project;
using ModelHelper.Core.Project.V1;

namespace ModelHelper.Core.Drops
{
    public class QueryOptionDrop : Drop
    {
        private readonly QueryOption _queryOption;

        public QueryOptionDrop(QueryOption queryOption)
        {
            _queryOption = queryOption;
        }

        public bool UseQueryOptions => _queryOption.UseQueryOptions;
        public string ClassName => _queryOption.ClassName;
        public string Namespace => _queryOption.Namespace;
        public string UserIdProperty => _queryOption.UserIdProperty;
        public string UserIdType => _queryOption.UserIdType;
        public bool UseClaimsPrincipalExtension => _queryOption.UseClaimsPrincipalExtension;
        public string ClaimsPrincipalExtensionMethod => _queryOption.ClaimsPrincipalExtensionMethod;
        public string ClaimsPrincipalExtensionNamespace => _queryOption.ClaimsPrincipalExtensionNamespace;



    }
    
}