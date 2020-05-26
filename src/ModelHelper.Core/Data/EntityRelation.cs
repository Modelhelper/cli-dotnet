namespace ModelHelper.Data
{
    public sealed class EntityRelation 
    {
        public int GroupIndex { get; set; }
        public string ConstraintName { get; set; }
        public string ParentColumnName { get; set; }
        public string ParentColumnType { get; set; }
        public string ChildColumnName { get; set; }
        public string ChildColumnType { get; set; }

        public bool ParentColumnNullable { get; set; }
        public bool IsSelfJoin { get; set; }
        public bool ChildColumnNullable { get; set; }

        public Entity Entity { get; set; }
    }
}