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

    public class UserContextDrop : Drop
    {
        private readonly UserContext _userContext;

        public UserContextDrop(UserContext userContext)
        {
            _userContext = userContext;
        }

        public string InterfaceName => _userContext.InterfaceName;
        public string VariableName => _userContext.VariableName;
        public string Namespace => _userContext.Namespace;
        public string UserProperty => _userContext.UserProperty;
        public string UserType => _userContext.UserType;
        

    }
}