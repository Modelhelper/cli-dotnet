using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Fluid;
using Fluid.Ast;
using Fluid.Tags;
using Fluid.Values;
using Irony.Parsing;
using Microsoft.Extensions.FileProviders;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Rendering;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using ModelHelper.Cli.Commands;
using Microsoft.Extensions.Logging;
using ModelHelper.Core;
using ModelHelper.Core.Project;
using ModelHelper.IO;

namespace ModelHelper.Cli
{

    class Program
    {
        static async Task Main(string[] args)
        {
            var defaults = new ModelHelperDefaults();
            
            // check for configuration
            if (!defaults.ConfigurationFile.Exists)
            {
                // Install
                if (!defaults.RootDirectory.Exists)
                {
                    defaults.RootDirectory.Create();
                }

                var defaultConfig = ModelHelperConfiguration.CreateDefault();
                var writer = new YamlWriter<ModelHelperConfiguration>();
                writer.Write(defaults.ConfigurationFile.FullName, defaultConfig);
            }

            // check for project file.
            var configReader = new YamlReader<ModelHelperConfiguration>();
            var config = configReader.Read(defaults.ConfigurationFile.FullName);


            var projectReader = new ProjectReader();
            // System.CommandLine.Rendering.Views.

            var currentProject = projectReader.Read(defaults.CurrentProjectFile.FullName);
            //setup our DI
            var serviceProvider = new ServiceCollection()
               .AddLogging()               
               .AddTransient<GenerateCommand>()
               .AddTransient<ProjectCommand>()
               .AddTransient<TemplateCommand>()
               .AddTransient<ModelHelperRootCommand>()
               .AddSingleton<IModelHelperConfiguration>(config)
               .AddSingleton<IModelHelperDefaults>(defaults)
               .AddSingleton<IProject3>(currentProject)
               .AddTransient<IConsole, System.CommandLine.IO.SystemConsole>()
            //    .AddTransient<ITerminal, System.CommandLine.Rendering.SystemConsoleTerminal>()

            //    .AddSingleton<IBarService, BarService>()
               .BuildServiceProvider();

            ////configure console logging
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder
                    // .AddFilter("Microsoft", LogLevel.Warning)
                    // .AddFilter("System", LogLevel.Warning)
                    .AddFilter("TestingFluidTemplate.Program", LogLevel.Debug)
                    .AddConsole();

            });

            var logger = serviceProvider.GetService<ILoggerFactory>()
               .CreateLogger<Program>();

            var terminal = serviceProvider.GetService<ITerminal>();

            logger.LogInformation("Starting application");            

            var parser = new CommandLineBuilder(serviceProvider.GetService<ModelHelperRootCommand>().Create())
                .AddCommand(serviceProvider.GetService<GenerateCommand>().Create())
                .AddCommand(serviceProvider.GetService<TemplateCommand>().Create())
                .AddCommand(serviceProvider.GetService<ProjectCommand>().Create())
                
                .UseAnsiTerminalWhenAvailable()
                .UseVersionOption()
                .UseHelp()
                .UseTypoCorrections()
                .UseParseDirective()
                .UseSuggestDirective()  
                .RegisterWithDotnetSuggest()              
                .Build();

            var parseresult = parser.Parse(args);
            
            // await parser.InvokeAsync(args);

            // terminal.WriteSlogan();

            //Console.ReadLine();

        }


        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)


                .ConfigureAppConfiguration((context, config) =>
                {
                    // Configure the app here.
                })
                .ConfigureServices((hostContext, services) =>
                {

                    //services.AddSingleton<IPatoLabConfiguration, PatoLabConfiguration>();
                    // services.AddTransient<IConnectionFactory, ConnectionFactory>();
                    // services.AddTransient<IHistologyRepository, HistologyRepository>();

                    // services.AddHostedService<Workers.HistologyReceiptWorker>();
                    // services.AddHostedService<Workers.HistologyOrderWorker>();
                    // services.AddHostedService<Workers.UpdateHistologyStatusWorker>();
                    // services.AddHostedService<Workers.SyncMissingOrderCodeOnStatusTableWorker>();
                    // services.AddHostedService<Workers.UpdateHistologyStatusWorker>();

                })

                //.UseInvocationLifetime()
                //.UseSerilog()

                // .ConfigureWebHostDefaults(webBuilder =>
                // {
                //     webBuilder.UseConfiguration(_config);
                //     webBuilder.UseKestrel(options =>
                //     {
                //         options.ListenLocalhost(_config.GetValue<int>("HealthPort"));
                //     });

                //     webBuilder.UseStartup<Startup>();
                // })
                ;
    }
    #region close off    

    public class Model
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }

        public bool IsRich { get; set; }
        public List<DataItem> Data { get; set; } = new List<DataItem>();

    }

    public class DataItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string DataType { get; set; }

        public bool IsParent { get; set; }
    }
    public class FluidThings
    {
        public FluidThings()
        {
        }

        public void Do()
        {
            var model = new Model
            {
                Firstname = "Bill",
                Lastname = "Gates",
                IsRich = true,

            };
            model.Data.Add(new DataItem { DataType = "varchar", Name = "Frank", Id = 1, IsParent = true });
            model.Data.Add(new DataItem { DataType = "varchar", Name = "Claire", Id = 2 });

            var snippet = @"{{ p.Firstname }}";
            var rowSnippet = @"{{ row.Name }} - {{ row.DataType | datatype: 'cs' }}";
            var source = @"
Hello {{ p.Firstname | upcase }} {{ p.Lastname }}{% if p.IsRich %} Rich guy
{% endif %}
{% capture test %} works {% endcapture %}

namespace {% namespace cs-data %} 

{{ test | kebab: 'cs' }}

{% for row in m.Data %}
    {{ forloop.index0 }} {{ forloop.index }} {% include 'row' %} {{ forloop.first }} - {{ forloop.last }}
{% endfor %}

Should show: {% include 'cs.test' %} - the 

{% argumentstag 'defaultvalue', arg1: 'value1', arg2: 123 %}

Kommentar: {% comment %}Dette skal ikke vises {% endcomment %}

{{ 'now' | date: '%v %H:%M' }}

Code generated on: {{ 'now' | date: '%Y.%m %d' }}

{% assign beatles = 'John, Paul, George, Ringo' | split: ', ' %}

{{ beatles | join: ' and ' }}


{% capture t %}{% for a in p.Data %}{{ a.Name }}, {% endfor %}{% endcapture %}
{{ t }}
{% assign x = t | split: ', ' %}
{{ t | split: ', ' | join: ' fantastic ' }}

{% assign my_array = 'ants, bugs, bees, bugs, ants' | split: ', ' %}

{{ my_array | uniq | join: ', ' }}


{% assign kitchen_products = p.Data | where: 'Id', 2 %}

Kitchen products:
{% for product in kitchen_products %}
            - {{ product.Name }}
            {% endfor %}

            ";
            TemplateContext.GlobalMemberAccessStrategy.Register<Model>();
            TemplateContext.GlobalMemberAccessStrategy.Register<DataItem>();
            TemplateContext.GlobalFilters.AddFilter("kebab", StringFilters.Downcase);
            TemplateContext.GlobalFilters.AddFilter("datatype", StringFilters.Datatype);


            FluidTemplate.Factory.RegisterTag<ShoutTag>("shout");
            FluidTemplate.Factory.RegisterTag<IceTag>("ice");
            FluidTemplate.Factory.RegisterTag<NamespaceTag>("namespace");
            FluidTemplate.Factory.RegisterTag<MoreTag>("more");
            FluidTemplate.Factory.RegisterTag<CustomArgumentsTag>("argumentstag");

            FluidTemplate.Factory.RegisterBlock<RepeatBlock>("repeat");
            FluidTemplate.Factory.RegisterBlock<CustomIdentifierBlock>("identifier");
            FluidTemplate.Factory.RegisterBlock<CustomSimpleBlock>("simple");
            FluidTemplate.Factory.RegisterBlock<CustomExpressionBlock>("exp");
            FluidTemplate.Factory.RegisterBlock<CustomArgumentsBlock>("argumentsblock");

            if (FluidTemplate.TryParse(source, out var template, out var errors))
            {
                var context = new TemplateContext();
                context.FileProvider = new MockFileProvider()
                    .Add("cs.test.liquid", snippet)
                    .Add("row.liquid", rowSnippet);


                context.MemberAccessStrategy.Register(model.GetType()); // Allows any public property of the model to be used
                                                                        // context.SetValue("p", model);
                context.SetValue("p", model);
                context.SetValue("m", model);


                if (errors.Any())

                {
                    errors.ToList().ForEach(x => Console.WriteLine(x));
                }
                Console.WriteLine(template.Render(context));
            }

        }
    }
   

    public class NamespaceTag : IdentifierTag
    {
        public NamespaceTag()
        {
            this.namespaces
                .Add("cs-data", "ModelHelper.Data.{{ p.FirstName }}");
        }
        protected Dictionary<string, string> namespaces = new Dictionary<string, string>();

        public override async ValueTask<Completion> WriteToAsync(TextWriter writer, TextEncoder encoder, TemplateContext context, string identifier)
        {
            var n = namespaces.ContainsKey(identifier.ToLower()) ? namespaces[identifier.ToLower()] : string.Empty;
            await writer.WriteAsync(n);
            return Completion.Normal;
        }
    }

    public class MockFileProvider : IFileProvider
    {
        private Dictionary<string, MockFileInfo> _files = new Dictionary<string, MockFileInfo>();

        public MockFileProvider()
        {
        }

        public IDirectoryContents GetDirectoryContents(string subpath)
        {
            throw new NotImplementedException();
        }

        public IFileInfo GetFileInfo(string path)
        {
            if (_files.ContainsKey(path))
            {
                return _files[path];
            }
            else
            {
                return null;
            }
        }

        public MockFileProvider Add(string path, string content)
        {
            _files[path] = new MockFileInfo(path, content);
            return this;
        }

        public Microsoft.Extensions.Primitives.IChangeToken Watch(string filter)
        {
            throw new NotImplementedException();
        }
    }


    public class MockFileInfo : IFileInfo
    {
        public MockFileInfo(string name, string content)
        {
            Name = name;
            Content = content;
        }

        public string Content { get; set; }
        public bool Exists => true;

        public bool IsDirectory => false;

        public DateTimeOffset LastModified => DateTimeOffset.MinValue;

        public long Length => -1;

        public string Name { get; }

        public string PhysicalPath => null;

        public Stream CreateReadStream()
        {
            var data = System.Text.Encoding.UTF8.GetBytes(Content);
            return new MemoryStream(data);
        }
    }
    public static class StringFilters
    {
        public static FluidValue Downcase(FluidValue input, FilterArguments arguments, TemplateContext context)
        {
            return new StringValue(input.ToStringValue().ToUpper());
        }

        public static FluidValue Datatype(FluidValue input, FilterArguments arguments, TemplateContext context)
        {
            return new StringValue(input.ToStringValue().ToUpper());
        }
    }


    public class QuoteTag : ExpressionTag
    {
        public override async ValueTask<Completion> WriteToAsync(TextWriter writer, TextEncoder encoder, TemplateContext context, Expression expression)
        {
            var value = (await expression.EvaluateAsync(context)).ToStringValue();
            await writer.WriteAsync("'" + value + "'");

            return Completion.Normal;
        }

    }

    public class ShoutTag : ITag
    {
        public BnfTerm GetSyntax(FluidGrammar grammar)
        {
            return grammar.Identifier + grammar.Range;
        }

        public Statement Parse(ParseTreeNode node, ParserContext context)
        {
            var identifier = node.ChildNodes[0].ChildNodes[0].Token.Text;
            var range = node.ChildNodes[0].ChildNodes[1];

            return new ForStatement(
                new List<Statement> { new OutputStatement(new LiteralExpression(new StringValue(identifier))) },
                identifier,
                DefaultFluidParser.BuildRangeExpression(range),
                null,
                null,
                false);
        }
    }

    public class IceTag : IdentifierTag
    {
        public override async ValueTask<Completion> WriteToAsync(TextWriter writer, TextEncoder encoder, TemplateContext context, string identifier)
        {
            await writer.WriteAsync("here is some ice " + identifier);
            return Completion.Normal;
        }
    }

    public class MoreTag : ExpressionTag
    {
        public override async ValueTask<Completion> WriteToAsync(TextWriter writer, TextEncoder encoder, TemplateContext context, Expression expression)
        {
            var value = await expression.EvaluateAsync(context);
            await writer.WriteAsync("here is some more " + value.ToStringValue());
            return Completion.Normal;
        }
    }

    public class RepeatBlock : ITag
    {
        public BnfTerm GetSyntax(FluidGrammar grammar)
        {
            return grammar.Range;
        }

        public Statement Parse(ParseTreeNode node, ParserContext context)
        {
            var range = context.CurrentBlock.Tag.ChildNodes[0];

            return new ForStatement(
                context.CurrentBlock.Statements,
                "i",
                DefaultFluidParser.BuildRangeExpression(range),
                null,
                null,
                false);
        }
    }

    public class CustomIdentifierBlock : IdentifierBlock
    {
        public override async ValueTask<Completion> WriteToAsync(TextWriter writer, TextEncoder encoder, TemplateContext context, string identifier, List<Statement> statements)
        {
            await writer.WriteAsync(identifier);

            await RenderStatementsAsync(writer, encoder, context, statements);

            return Completion.Normal;
        }
    }

    public class CustomExpressionBlock : ExpressionBlock
    {
        public override async ValueTask<Completion> WriteToAsync(TextWriter writer, TextEncoder encoder, TemplateContext context, Expression expression, List<Statement> statements)
        {
            await writer.WriteAsync((await expression.EvaluateAsync(context)).ToStringValue());

            await RenderStatementsAsync(writer, encoder, context, statements);

            return Completion.Normal;
        }
    }

    public class CustomSimpleBlock : SimpleBlock
    {
        public override async ValueTask<Completion> WriteToAsync(TextWriter writer, TextEncoder encoder, TemplateContext context, List<Statement> statements)
        {
            await writer.WriteAsync("simple");

            await RenderStatementsAsync(writer, encoder, context, statements);

            return Completion.Normal;
        }
    }

    public class CustomArgumentsBlock : ArgumentsBlock
    {
        public override async ValueTask<Completion> WriteToAsync(TextWriter writer, TextEncoder encoder, TemplateContext context, FilterArgument[] arguments, List<Statement> statements)
        {
            foreach (var argument in arguments)
            {
                await writer.WriteAsync(argument.Name + ":");
                await writer.WriteAsync((await argument.Expression.EvaluateAsync(context)).ToStringValue());
            }

            await RenderStatementsAsync(writer, encoder, context, statements);

            return Completion.Normal;
        }
    }

    public class CustomArgumentsTag : ArgumentsTag
    {
        public override async ValueTask<Completion> WriteToAsync(TextWriter writer, TextEncoder encoder, TemplateContext context, FilterArgument[] arguments)
        {
            foreach (var argument in arguments)
            {

                //await writer.WriteAsync(argument.Name + ":");
                await writer.WriteAsync((await argument.Expression.EvaluateAsync(context)).ToStringValue());
            }

            return Completion.Normal;
        }
    }

    #endregion
}
