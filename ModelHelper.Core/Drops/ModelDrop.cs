using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using ModelHelper.Core.Templates;
using DotLiquid;
using ModelHelper.Extensibility;

namespace ModelHelper.Core.Drops
{
    public class ModelDrop : Drop
    {
        public ITemplateModel TemplateModel { get; }

        public ModelDrop(ITemplateModel templateModel, bool includeChildTables = false) //:base(templateModel.Table)
        {
            TemplateModel = templateModel;
            SqlScriptGenerators = templateModel.SqlScriptGenerators;
            DatatypeConverters = templateModel.DatatypeConverters;

            Project = new ProjectDrop(templateModel.Project);
            Table = new TableDrop(templateModel.Table, templateModel.IncludeChildren, templateModel.IncludeParents);
            Namespace = new NamespaceDrop(templateModel.Project);
           // Api = new ApiDrop(templateModel.Project.Api);
            Database = new DatabaseDrop(templateModel.Project);
            IncludeChildren = templateModel.IncludeChildren;
            IncludeParents = templateModel.IncludeParents;
            IncludeRelations = templateModel.IncludeRelations;

            Namespaces = new Dictionary<string, string>();
            //Options = new HashSet<KeyValuePair<string, object>>();
            Options = new Dictionary<string, object>();
            OptionValues = new List<object>();
            OptionKeys = new List<string> ();

            QueryOptions = new QueryOptionDrop(templateModel.Project.Code.QueryOptions);
            UserContext = new UserContextDrop(templateModel.Project.Code.UserContext);

            ConnectionInterface = templateModel?.Project?.Code?.Connection != null ? templateModel.Project.Code.Connection.Interface : string.Empty;
            ConnectionVariable = templateModel?.Project?.Code?.Connection != null ? templateModel.Project.Code.Connection.Variable : string.Empty;
            ConnectionMethod = templateModel?.Project?.Code?.Connection != null ? templateModel.Project.Code.Connection.Method : string.Empty;

            UseQueryOptions = templateModel?.Project?.Code?.UseQueryOptions ?? false;
            InjectUserContext = templateModel?.Project?.Code?.InjectUserContext ?? false;

            if (templateModel?.Project?.Code?.Locations != null)
            {
                foreach (var location in templateModel.Project.Code.Locations)
                {
                    Namespaces.Add(location.Key, location.Namespace);
                }
            }

            if (templateModel?.Project?.Options != null)
            {
                foreach (var pair in templateModel.Project.Options.Select(p => new KeyValuePair<string, object>(p.Key, p.Value)))
                {
                    if (!Options.ContainsKey(pair.Key))
                    {
                        Options.Add(pair.Key, pair.Value);
                        OptionValues.Add(pair.Value);
                        OptionKeys.Add(pair.Key);
                    }
                    
                }
            }
            

            if (templateModel.Options != null)
            {
                foreach (var option in templateModel.Options)
                {
                    if (!Options.ContainsKey(option.Key))
                        //Options.Add(option);
                    {
                        Options.Add(option.Key, option.Value);
                        OptionValues.Add(option.Value);
                        OptionKeys.Add(option.Key);
                    }
                    //OptionsList.Add(option);
                }
                //Options =  templateModel.Options;
            }

            
        }

        public bool UseQueryOptions { get; }
        public bool InjectUserContext { get; }
        public string ConnectionInterface { get; }
        public string ConnectionVariable { get; }
        public string ConnectionMethod { get; }

        public List<ISqlGenerator> SqlScriptGenerators { get; }
        public List<IDatatypeConverter> DatatypeConverters { get; }

        public Dictionary<string, string> Dictionary => TemplateModel.Dictionary ?? new Dictionary<string, string>();

        public QueryOptionDrop QueryOptions { get; set; }
        public UserContextDrop UserContext { get; set; }

        public DatabaseDrop Database { get; set; }
        public ProjectDrop Project { get;  }        
        public Dictionary<string, string> Namespaces { get;  }
        public NamespaceDrop Namespace { get;  }
        //public ApiDrop Api { get; }


        public TableDrop Table { get; }

        public Dictionary<string, object> Options { get;  }
        public List<object> OptionValues { get;  }
        public List<string> OptionKeys { get;  }
        //public HashSet<KeyValuePair<string, object>> Options { get;  }

        public bool IncludeChildren { get; }
        public bool IncludeParents { get; }
        public bool IncludeRelations { get; }

        //public dynamic Options { get; }        

    }
    
}