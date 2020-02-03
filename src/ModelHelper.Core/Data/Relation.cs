namespace ModelHelper.Data
{
    public sealed class Relation
    {
        public int SortIndex { get; set; }
        public int Level { get; set; }

        public string FullPath { get; set; }
        public int Depth { get; set; }

        public string Family { get; set; }

        public string ReferenceName { get; set; }

        public string ParentTableName { get; set; }
        public string ParentColumnName { get; set; }
        public string ParentColumnType { get; set; }

        public string ReferencedTableName { get; set; }
        public string ReferencedColumnName { get; set; }
        public string ReferencedColumnType { get; set; }

        public bool IsSelfJoin { get; set; }
    }
}