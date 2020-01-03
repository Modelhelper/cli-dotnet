namespace ModelHelper.Core.Project
{
    public static class ProjectFactory
    {
        public static Project3 Create(ProjectCreateType type)
        {
            switch (type)
            {
                case ProjectCreateType.Empty: return EmptyProject();
                case ProjectCreateType.Demo: return DemoProject();
                default: return DefaultProject();

            }
        }

        public static Project3 EmptyProject()
        {
            return new Project3();
        }

        public static Project3 DefaultProject()
        {
            var project = new Project3();
            project.Customer = "Default";
            return project;
        }
        
        public static Project3 DemoProject()
        {
            var project = DefaultProject();
            project.Customer = "Demo";
            
            project.Source = new ProjectSource
            {
                DefaultSource = "demo-sql"
            };

            project.Source.Connections = new System.Collections.Generic.List<ProjectSource.ProjectSourceConnection>();
            
            var demoConnection = new ProjectSource.ProjectSourceConnection{
                ConnectionString = "DEMO-SQL",
                DefaultSchema = "dbo",
                Name = "demo-sql",
                Type = "mssql",
                CreateConnectionMethod = "CreateForDemo()",
                Groups = new System.Collections.Generic.List<ProjectSource.ProjectEntityGroup>{
                    new ProjectSource.ProjectEntityGroup {
                        Name = "person_group",
                        Schema = "dbo",
                        Entities = new System.Collections.Generic.List<string> {
                            "Person",
                            "Contact",
                            "Phone",
                            "Email",
                            "Address"

                        },
                        Options = new System.Collections.Generic.Dictionary<string, string>{
                            {"folder", "Person"}
                        }
                    },
                    new ProjectSource.ProjectEntityGroup {
                        Name = "order_group",
                        Schema = "dbo",
                        Entities = new System.Collections.Generic.List<string> {
                            "OrderHead",
                            "OrderItem"                            
                        },
                        Options = new System.Collections.Generic.Dictionary<string, string>{
                            {"folder", "Order"}
                        }
                    }
                },
                Mapping = new System.Collections.Generic.List<ProjectSource.ProjectSourceColumnMapping>
                {
                    new ProjectSource.ProjectSourceColumnMapping
                    {
                        Name = "CreatedOn",
                        IsIgnored = true,
                        UsedAs = "CreatedDate"
                    }
                }
            };

            project.Code = new ProjectCodeSection
            {
                Locations = new System.Collections.Generic.List<ProjectCodeSection.CodeLocation>
                {
                    new ProjectCodeSection.CodeLocation
                    {
                        Key = "data",
                        NamePostfix = "Repository",
                        Namespace = "{root}.Data.{folder | Upper}",
                        Path = ".\\Data\\{folder | Upper}"
                    }
                },
                RemoveColumnPrefixes = true 
            };

            project.Source.Connections.Add(demoConnection);
            return project;
        }



    }
}