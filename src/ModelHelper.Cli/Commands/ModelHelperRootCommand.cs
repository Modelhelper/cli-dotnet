using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.Rendering;
using System.Reflection;
using ModelHelper.Core;
using ModelHelper.Core.Project;
using ModelHelper.Cli.Extensions;

namespace ModelHelper.Cli.Commands
{
    public class ModelHelperRootCommand
    {
        private readonly ITerminal _terminal;
        private readonly IModelHelperDefaults _modelHelperDefaults;
        private readonly IProject3 _currentProject;
        private readonly SpanFormatter _spanFormatter = new SpanFormatter();
        public ModelHelperRootCommand(ITerminal terminal, IModelHelperDefaults modelHelperDefaults, IProject3 currentProject)
        {
            this._terminal = terminal;
            this._modelHelperDefaults = modelHelperDefaults;
            this._currentProject = currentProject;
        }

        public RootCommand Create()
        {
            var rootCommand = new RootCommand();
            rootCommand.Description = "ModelHelper";

            // rootCommand.AddCommand(generateCommand.Create());

            rootCommand.Handler = CommandHandler.Create(() =>
            {
                Assembly execAssembly = Assembly.GetEntryAssembly();

                AssemblyName name = execAssembly.GetName();
                var fullVersion = name.Version;
                var logoVersion = $"v.{fullVersion.Major.ToString()}.{fullVersion.Minor.ToString()}";
                
                _terminal.WriteLogo(logoVersion);

                _terminal.Out.Write($"\n\nVersion: \t\t{fullVersion.ToString()}"); //  for .Net ({execAssembly.ImageRuntimeVersion})
                _terminal.Out.Write($"\nApp name: \t\t{name.Name}.exe");
                _terminal.Out.Write($"\nApp Location: \t\t{Assembly.GetExecutingAssembly().Location}");

                _terminal.Out.Write($"\nConfiguration: \t\t{_modelHelperDefaults.RootDirectory.FullName}");

                _terminal.WriteProjectInfo(_currentProject);

            });

            return rootCommand;
        }
    }
}