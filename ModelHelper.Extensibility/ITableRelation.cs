namespace ModelHelper.Extensibility
{
    public interface ITableRelation : IEntity
    {
        int GroupIndex { get; set; }
        string ConstraintName { get; set; }
        string ParentColumnName { get; set; }
        string ParentColumnType { get; set; }
        string ChildColumnName { get; set; }
        string ChildColumnType { get; set; }

        bool IsSelfJoin { get; set; }
    }
}