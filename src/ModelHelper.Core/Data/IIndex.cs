using System.Collections.Generic;

namespace ModelHelper.Data
{
    public interface IIndex
    {
        string Id { get;  }
        string Name { get; }

        double Size { get;  }
        double AvgFragmentationPercent { get;  }

        bool IsClustered { get;  }
        bool IsPrimaryKey { get;  }

        bool IsUnique { get; }

        double AvgPageSpacePercent { get; }
        double AvgRecordSize { get; }
        int Rows { get; } 

    IEnumerable<IIndexColumn> Columns { get; }

 
    }
}