namespace ModelHelper.Data
{
    public interface IIndexColumn
    {
        string Name { get; }
        bool IsDescending { get; }
        bool IsNullable { get; }
        bool IsIdentity { get; }
        double PartitionOrginal { get; }

        
    }
}