namespace ModelHelper.Data
{
    public sealed class IndexColumn
    {
        public string Name { get; }
        public bool IsDescending { get; }
        public bool IsNullable { get; }
        public bool IsIdentity { get; }
        public double PartitionOriginal { get; }

        
    }
}