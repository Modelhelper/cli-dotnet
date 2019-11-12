using System;
using System.Linq;
using Xunit;

namespace ModelHelper.UnitTests
{
    public class Project_Version3_tests
    {
        [Fact]
        public void Should_Load_Project()
        {
            // arrange
            
            // act
            var project = ModelHelper.Extensions.ProjectExtensions.LoadContent(ValidJson);
            
            // assert

            Assert.NotNull(project);
            
        }

        [Fact]
        public void RootNamespace_Should_be_ModelHelper()
        {
            // arrange
            var expected = "Root";
            // act
            var project = ModelHelper.Extensions.ProjectExtensions.LoadContent(ValidJson);
            var actual = project.RootNamespace;
            // assert
            Assert.Equal(expected, actual);
            Assert.NotNull(project.Source);
            Assert.NotNull(project.Code);
            Assert.NotNull(project.Options);
        }

        [Fact]
        public void ProjectData_Should_not_be_null()
        {
            // arrange

            // act
            var project = ModelHelper.Extensions.ProjectExtensions.LoadContent(ValidJson);

            // assert            
            Assert.NotNull(project.Source);
            Assert.NotNull(project.Code);
            Assert.NotNull(project.Options);
        }

        [Fact]
        public void Project_Should_have_2_connections()
        {
            // arrange
            var expected = 2;
            // act
            var project = ModelHelper.Extensions.ProjectExtensions.LoadContent(ValidJson);
            var actual = project.Source.Connections.Count();
            // assert            
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Project_main_connection_Should_have_2_groups()
        {
            // arrange
            var expected = 2;
            // act
            var project = ModelHelper.Extensions.ProjectExtensions.LoadContent(ValidJson);
            var actual = project.Source.Connections.FirstOrDefault(t => t.Name == "main").Groups.Count();
            // assert            
            Assert.Equal(expected, actual);
        }



        public static string ValidJson = @"{
    'version': '3.0.0',
    'rootNamespace': 'Root',
    'customer': 'TestCustomer',
    'name': 'Api',
    'description': 'description',
    'rootPath': 'path',
    'source': {
        'defaultSource': 'main',
        'connections': [            
            {
                'name': 'sub',
                'connectionString': 'sub connection',
                'defaultSchema': 'dbo',
                'type': 'mssql',
                'options': {},
                'mapping': [],
                'createConnectionMethod': 'CreateForSub()'
            },
            {
                'name': 'main',
                'connectionString': 'main connection',
                'defaultSchema': 'dbo',
                'type': 'mssql',      
                'createConnectionMethod': 'Create()',                          
                'options': {
                    'currentFolder': 'connection level'
                },
                'mapping': [],
                'groups': [
                    { 
                        'name': 'group_name', 
                        'schema': 'entities',
                        'options': {
                            'CurrentFolder': 'Entity group'
                        },
                        'entities': [
                            'Entity1', 
                            'Entity2'
                        ]
                    },
                    {
                        'name': 'SharedGroup',
                        'options': {
                            'CurrentFolder': 'Shared'
                        },
                        'entities': [
                            'Entity1', 
                            'Entity2'
                        ]
                    }
                ]
            }
            
        ],
        'mapping': [
            {
                'name': 'IsActive',
                'isIgnored': false,
                'usedAs': 'DeletedMarker'
            },            
            {
                'name': 'CreatedDate',
                'isIgnored': 'True',
                'usedAs': 'CreatedOn'
            },
            {
                'name': 'CreatedBy',
                'isIgnored': 'True',
                'usedAs': 'CreatedBy'
            },
            {
                'name': 'ChangedOn',
                'isIgnored': 'True',
                'usedAs': 'ModifiedOn'
            },
            {
                'name': 'ChangedDate',
                'isIgnored': 'True',
                'usedAs': 'ModifiedOn'
            }            
        ]
    },
    
    'options': {
        'currentFolder': 'Last level'
    },
    'code': {
        'useQueryOptions': false,
        'injectUserContext': true,
        'useConnectionFactory': true,
        'removeColumnPrefixes': true,
        'queryOptions': {            
            'ClassName': 'QueryOptions',
            'variableName': 'userContext',            
            'userProperty': 'UserName',
            'userType': 'string',            
            'codeLocationKey': 'api.interface'
        },
        'userContext': {            
            'interfaceName': 'IUserContext',
            'variableName': 'userContext',            
            'userProperty': 'UserName',
            'userType': 'string',            
            'codeLocationKey': 'api.interface'
        },
        'locations': [
            {
                'key': 'api.interfaces',
                'namespace': '{{ rootNamespace }}.Api.Core.Interfaces',
                'path': '.\\src\\{{ rootNamespace }}.Api.Core{{ option.CurrentFolder }}\\Interfaces',
                'namePostfix': 'Repository',
                'namePrefix': 'I'
            },
            {
                'key': 'api.models',
                'namespace': '{{ rootNamespace }}.Api.Core.Models',
                'path': '.\\src\\{{ rootNamespace }}.Api.Core\\Models',
                'namePostfix': 'Model'
            }
        ],
        'connectionFactory': {
            'interface': 'IConnectionFactory',
            'variable': 'connection',
            'method': 'CreateForPatoLab()',
            'codeLocationKey': 'api.interface'
        }
    }
    
}";
    }

}
