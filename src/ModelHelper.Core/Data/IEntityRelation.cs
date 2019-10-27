namespace ModelHelper.Data
{
    public interface IEntityRelation : IEntity
    {
        int GroupIndex { get; set; }
        string ConstraintName { get; set; }
        string ParentColumnName { get; set; }
        string ParentColumnType { get; set; }
        string ChildColumnName { get; set; }
        string ChildColumnType { get; set; }

        bool ParentColumnNullable { get; set; }
        bool IsSelfJoin { get; set; }
        bool ChildColumnNullable { get; set; }
    }
}