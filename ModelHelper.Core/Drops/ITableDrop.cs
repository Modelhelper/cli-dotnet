﻿using System.Collections.Generic;
using ModelHelper.Extensibility;

namespace ModelHelper.Core.Drops
{
    public interface ITableDrop 
    {
        string Name { get; }

        string ModelName { get; }

        string Schema { get; }
        string Alias { get; }

        //string ApiResourceAddress { get; }
        bool UsesIdentityColumn { get; }
        bool UsesGuidAsPrimaryKey { get; }

        bool UsesDeletedColumn { get; }
        string DeletedColumnName { get; }        

        List<DataColumnDrop> Columns { get; }
        List<DataColumnDrop> AllColumns { get; }
        List<DataColumnDrop> IgnoredColumns { get; }
        List<DataColumnDrop> PrimaryKeys { get; }

        bool IncludeParentRelations { get; }
        bool IncludeChildRelations { get; }
    }
}