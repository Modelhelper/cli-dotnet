using System.Collections.Generic;

namespace ModelHelper.Data
{
    public sealed class Index
    {
        public string Id { get;  set;}
        public string Name { get; set;}

        public double Size { get;  set;}
        public double AvgFragmentationPercent { get;  set;}

        public bool IsClustered { get;  set;}
        public bool IsPrimaryKey { get;  set;}

        public bool IsUnique { get; set;}

        public double AvgPageSpacePercent { get; set;}
        public double AvgRecordSize { get; set;}
        public int Rows { get; set; }

    public IEnumerable<IndexColumn> Columns { get; set;}

 
    }
}