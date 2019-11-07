using System;
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
            var expected = "ModelHelper";
            // act
            var project = ModelHelper.Extensions.ProjectExtensions.LoadContent(ValidJson);
            var actual = project.RootNamespace;
            // assert
            Assert.Equal(expected, actual);
        }

        public static string ValidJson = @"
{
    'RootNamespace': 'ModelHelper',
    'Customer': 'ModelHelper',
    'Data': {
        'DefaultConnection': 'lab',
        'Connections': [
            {
                'Name': 'hist',
                'ConnectionString': 'Data Source=patest01;Initial Catalog=PatoLabHistology_test;Integrated Security=True;Connect Timeout=10;Application Name=ModelHelper',
                'DefaultSchema': 'dbo',
                'DbType': 'mssql',
                'ColumnMapping': [],
                'ConnectionMethod': 'CreateForHistology()'
            },
            {
                'Name': 'raw',
                'ConnectionString': 'Data Source=patest01;Initial Catalog=PatoLabRaw_test;Integrated Security=True;Connect Timeout=10;Application Name=ModelHelper',
                'DefaultSchema': 'dbo',
                'DbType': 'mssql',
                'ModelOptions': [],
                'ColumnMapping': [],
                'ConnectionMethod': 'CreateForPatolabRaw()'
            },
            {
                'Name': 'lab',
                'ConnectionString': 'Data Source=patest01;Initial Catalog=PatoLab_test;Integrated Security=True;Connect Timeout=10;Application Name=ModelHelper',
                'DefaultSchema': 'dbo',
                'DbType': 'mssql',                                
                'ModelOptions': {
                    'option_1': 'value_1'
                },
                'ColumnMapping': [],
                'EntityGroups': [
                    { 
                        'Name': 'Histology', 
                        'ModelOptions': {
                            'CurrentFolder': 'Histology'
                        }
                        ,
                        'Entities': [
                            'HistologyPathologyType', 
                            'OrderHistologyTubeSampleHistologyPathologyType', 
                            'HistologyPathologyTypeSampleType', 
                            'OrderHistologyPriorityType', 
                            'OrderHistologyStatusType', 
                            'OrderHistologyHistory', 
                            'OrderHistology', 
                            'OrderHistologyTubeHistory', 
                            'OrderHistologyTube', 
                            'OrderTubeOrderHistologyTube', 
                            'OrderHistologyTubeSampleHistory', 
                            'OrderHistologyTubeSample', 
                            'Attachment', 
                            'OrderHistologyTubeSampleAttachment', 
                            'HistologyDiagnoseTypeOrderHistologyTubeType', 
                            'HistologyDiagnoseTypeOrderHistologyTube', 
                            'HistologyDiagnoseType', 
                            'HistologyMagnitudeType', 
                            'AttachmentType', 
                            'HistologyQuantificationType', 
                            'HistologyDistributionType', 
                            'OrderHistologyTubeSampleHistologyPathologyTypeHistologyPathologyDescriptionType', 
                            'HistologyPathologyDescriptionType', 
                            'OrderHistologyTubeSampleHistologyPathologyTypeSampleComponentType', 
                            'SampleTypeHistologyPathologyDescriptionType', 
                            'SampleTypeSampleComponentType', 
                            'SampleComponentType', 
                            'SampleSubType', 
                            'HistologyPathologyCategory', 
                            'HistologyDiagnoseTypeOrderHistology' 
                        ]
                    },
                    {
                        'Name': 'Shared',
                        'ModelOptions': {
                            'CurrentFolder': 'Shared'
                        }
                    }
                ]
            },
            {
                'Name': 'log',
                'ConnectionString': 'Data Source=patest01;Initial Catalog=Patolog_test;Integrated Security=True;Connect Timeout=10;Application Name=ModelHelper',
                'DefaultSchema': 'dbo',
                'DbType': 'mssql',
                'ModelOptions': {},
                'ColumnMapping': []
            },
            {
                'Name': 'stage',
                'ConnectionString': 'Data Source=patest01;Initial Catalog=PatoStage_test;Integrated Security=True;Connect Timeout=10;Application Name=ModelHelper',
                'DefaultSchema': 'dbo',
                'DbType': 'mssql',     
                'ModelOptions': {},        
                'ColumnMapping': []
            },
            {
                'Name': 'crm',
                'ConnectionString': 'Data Source=pasql03;Initial Catalog=PatoGen_Analyse_AS_MSCRM;Integrated Security=True;Connect Timeout=10;Application Name=ModelHelper',
                'DefaultSchema': 'dbo',
                'DbType': 'mssql',
                'ModelOptions': {},
                'ColumnMapping': []
            }
        ],
        'ColumnMapping': [
            {
                'Name': 'IsActive',
                'IsIgnored': false,
                'UsedAs': 'DeletedMarker'
            },
            {
                'Name': 'CreatedOn',
                'IsIgnored': true,
                'UsedAs': 'CreatedOn'
            },
            {
                'Name': 'CreatedDate',
                'IsIgnored': 'True',
                'UsedAs': 'CreatedOn'
            },
            {
                'Name': 'CreatedBy',
                'IsIgnored': 'True',
                'UsedAs': 'CreatedBy'
            },
            {
                'Name': 'ChangedOn',
                'IsIgnored': 'True',
                'UsedAs': 'ModifiedOn'
            },
            {
                'Name': 'ChangedDate',
                'IsIgnored': 'True',
                'UsedAs': 'ModifiedOn'
            },
            {
                'Name': 'ChangedBy',
                'IsIgnored': 'True',
                'UsedAs': 'ModifiedBy'
            },
            {
                'Name': 'ModifiedOn',
                'IsIgnored': 'True',
                'UsedAs': 'ModifiedOn'
            },
            {
                'Name': 'ModifiedBy',
                'IsIgnored': 'True',
                'UsedAs': 'ModifiedBy'
            },
            {
                'Name': 'ValidFrom',
                'IsIgnored': 'True'
            },
            {
                'Name': 'ValidTo',                
                'IsIgnored': 'True'
            }
        ]
    },
    'Name': 'PatoLab.Api',
    'Description': null,
    'RootPath': null,
    'ModelOptions': {
        'CurrentFolder': 'Histology'
    },
    'Code': {
        'UseQueryOptions': false,
        'QueryOptions': {
            'UseQueryOptions': false,
            'ClassName': 'QueryOptions',
            'Namespace': 'PatoLab.Api.Core',
            'UserIdProperty': 'UserName',
            'UserIdType': 'int',
            'UseClaimsPrincipalExtension': false,
            'ClaimsPrincipalExtensionMethod': 'ToQueryOptions',
            'ClaimsPrincipalExtensionNamespace': 'PatoLab.Api.Core.Extensions'
        },
        'InjectUserContext': true,
        'userContext': {            
            'InterfaceName': 'IUserContext',
            'VariableName': 'userContext',
            'Namespace': 'PatoLab.Api.Core',
            'UserProperty': 'UserName',
            'UserType': 'string'            
        },
        'Keys': [],
        'Locations': [
            {
                'Key': 'api.interfaces',
                'Namespace': 'PatoLab.Api.Core.Interfaces',
                'Path': '.\\src\\PatoLab.Api.Core\\Interfaces',
                'NamePostfix': 'Repository',
                'NamePrefix': 'I'
            },
            {
                'Key': 'api.models',
                'Namespace': 'PatoLab.Api.Core.Models',
                'Path': '.\\src\\PatoLab.Api.Core\\Models',
                'NamePostfix': 'Model'
            },
            {
                'Key': 'api.viewmodels',
                'Namespace': 'PatoLab.Api.Core.ViewModels',
                'Path': '.\\src\\PatoLab.Api.Core\\ViewModels',
                'NamePostfix': 'ViewModel'
            },
            {
                'Key': 'api.controllers',
                'Namespace': 'PatoLab.Api.Controllers',
                'Path': '.\\controllers',
                'NamePostfix': 'Controller'
            },
            {
                'Key': 'api.repositories',
                'Namespace': 'PatoLab.Api.Data.Repositories',
                'Path': '.\\src\\PatoLab.Api.Data\\Repositories',
                'NamePostfix': 'Repository'
            },
            {
                'Key': 'gql.query.types',
                'Namespace': 'PatoLab.Api.GraphQL.Histology.Query.Types',
                'Path': '.\\src\\PatoLab.Api\\GraphQL\\{{ CurrentFolder }}\\Query\\Types',
                'NamePostfix': 'QueryType'
            },
            {
                'Key': 'gql.query.groups',
                'Namespace': 'PatoLab.Api.GraphQL.Histology.Query',
                'Path': '.\\src\\PatoLab.Api\\GraphQL\\{{ CurrentFolder }}\\Query',
                'NamePostfix': 'QueryGroup'
            },            
            {
                'Key': 'gql.mutation.types',
                'Namespace': 'PatoLab.Api.GraphQL.Histology.Mutation.Types',
                'Path': '.\\src\\PatoLab.Api\\GraphQL\\{{ CurrentFolder }}\\Mutation\\Types',
                'NamePostfix': 'InputType'
            },
            {
                'Key': 'gql.mutation.groups',
                'Namespace': 'PatoLab.Api.GraphQL.Histology.Mutation',
                'Path': '.\\src\\PatoLab.Api\\GraphQL\\{{ CurrentFolder }}\\Mutation',
                'NamePostfix': 'MutationGroup'
            }
        ],
        'ConnectionFactory': {
            'Interface': 'IConnectionFactory',
            'Variable': 'connection',
            'Method': 'CreateForPatoLab()'
        }
    },
    'Version': '3.0.0'
}
";
    }
}
