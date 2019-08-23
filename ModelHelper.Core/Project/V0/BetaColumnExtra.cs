namespace ModelHelper.Core.Project.V0
{
    public class BetaColumnExtra
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