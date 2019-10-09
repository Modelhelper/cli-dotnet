namespace ModelHelper.Core.Project
{
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

    public class UserContext
    {
        
        public string InterfaceName { get; set; }
        public string Namespace { get; set; }
        public string UserProperty { get; set; }
        public string UserType { get; set; }
        public string VariableName { get; set; }
    }
}