namespace ModelHelper.Core.Project
{
    public class ProjectCodeStructure
    {
        public string Key { get; set; }
        public string Namespace { get; set; }
        public string Path { get; set; }

        //public string NameTemplate { get; set; } // {{model.Table.Name | UpperCamel | Singular}}Repository
        public string NamePrefix { get; set; } // Repository
    }
}