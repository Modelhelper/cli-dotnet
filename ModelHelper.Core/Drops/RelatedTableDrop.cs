using System;
using System.Collections.Generic;
using System.Linq;
using ModelHelper.Core.Database;
using DotLiquid;
using ModelHelper.Core.Extensions;
using ModelHelper.Extensibility;

namespace ModelHelper.Core.Drops
{
    public class RelatedTableDrop : Drop, ITableDrop
    {
        private readonly ITableRelation _relation;

        public RelatedTableDrop(ITableRelation relation, IEntity parentTable)
        {
            _relation = relation;
            Columns = new List<DataColumnDrop>();
            PrimaryKeys = new List<DataColumnDrop>();
            AllColumns = new List<DataColumnDrop>();
            IgnoredColumns = new List<DataColumnDrop>();
            ViewModelColumns = new List<DataColumnDrop>();
            IncludeChildRelations = false;
            IncludeParentRelations = false;
            var hasViewModelColumn = relation.Columns.Any(c => c.UseInViewModel);
            var tableInstances = parentTable.ParentRelations.Count(r => String.Equals(r.Name, relation.Name, StringComparison.InvariantCultureIgnoreCase)) > 1;

            foreach (var column in _relation.Columns)
            {
                var canBeUsedAsViewModelColumn = column.IsPrimaryKey == false && column.IsForeignKey == false &&
                                                 column.IsIgnored == false && column.IsIdentity == false &&
                                                 !column.IsCreatedByUser && !column.IsCreatedDate &&
                                                 !column.IsDeletedMarker && !column.IsModifiedByUser &&
                                                 !column.IsModifiedDate;

                var parentTableName = relation.Name.AsUpperCamelCase().SingularizeWord();
                var viewModelName = $"{parentTableName}{column.PropertyName}";

                var drop = new DataColumnDrop(column);
                AllColumns.Add(drop);

                if (column.IsIgnored)
                {
                    IgnoredColumns.Add(drop);
                }
                else
                {
                    Columns.Add(drop);
                }

                if (column.IsPrimaryKey)
                {
                    PrimaryKeys.Add(drop);
                }

                if (canBeUsedAsViewModelColumn)
                {
                    if (hasViewModelColumn)
                    {
                        if (column.UseInViewModel)
                        {
                            if (tableInstances)
                            {
                                var name = relation.ChildColumnName.ToLower().EndsWith("id")
                                    ? relation.ChildColumnName.Substring(0, relation.ChildColumnName.Length - 2)
                                    : relation.ChildColumnName;

                                viewModelName = $"{name.AsUpperCamelCase().SingularizeWord()}{column.PropertyName.AsUpperCamelCase().SingularizeWord()}";
                            }

                            column.PropertyName = viewModelName;
                            ViewModelColumns.Add(new DataColumnDrop(column));
                        }
                        


                    }
                    else 
                    {

                        column.PropertyName = $"{parentTableName}{column.PropertyName}";
                        if (tableInstances)
                        {
                            var name = relation.ChildColumnName.ToLower().EndsWith("id")
                                ? relation.ChildColumnName.Substring(0, relation.ChildColumnName.Length - 2)
                                : relation.ChildColumnName;

                            column.PropertyName = $"{name.AsUpperCamelCase().SingularizeWord()}{column.PropertyName.AsUpperCamelCase().SingularizeWord()}";
                        }

                        ViewModelColumns.Add(new DataColumnDrop(column));
                    }
                }
                
                
            }

            //foreach (var column in _relation.Columns.Where(c => c.IsPrimaryKey))
            //{
               
            //}
        }

        public bool IsSelfJoin => _relation.IsSelfJoin;
        public string ParentColumnName => _relation.ParentColumnName;
        public string ParentColumnType => _relation.ParentColumnType;
        public bool ParentColumnNullable => _relation.ParentColumnNullable;
        
        public string ChildColumnName => _relation.ChildColumnName;
        public string ChildColumnType => _relation.ChildColumnType;
        public bool ChildColumnNullable => _relation.ChildColumnNullable;
        public string ModelName => _relation.ModelName;

        public string Name => _relation.Name;
        public string Schema => _relation.Schema;
        public string Alias => _relation.Alias;
       

        public bool UsesIdentityColumn => _relation.UsesIdentityColumn;
        public bool UsesGuidAsPrimaryKey => _relation.UsesGuidAsPrimaryKey;

        public bool UsesDeletedColumn => _relation.UsesDeletedColumn;
        public string DeletedColumnName => _relation.DeletedColumnName;

        public int GroupIndex => _relation.GroupIndex;

        public List<DataColumnDrop> Columns { get; }
        public List<DataColumnDrop> AllColumns { get; }
        public List<DataColumnDrop> IgnoredColumns { get; }
        public List<DataColumnDrop> PrimaryKeys { get; }
        public List<DataColumnDrop> ViewModelColumns { get; }
        public bool IncludeParentRelations { get; }
        public bool IncludeChildRelations { get; }
    }
}