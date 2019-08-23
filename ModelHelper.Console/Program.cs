using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using ModelHelper.Commands;
using ModelHelper.Core;
using ModelHelper.Core.Extensions;
using ModelHelper.Core.Templates;
using ModelHelper.Extensions;

namespace ModelHelper
{    
    class Program
    {

        private CompositionContainer _container;

        [Import(typeof(ICommandExecutor))]
        public ICommandExecutor CommandExecutor;

        public Program()
        {
            var catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(Program).Assembly));
            catalog.Catalogs.Add(new DirectoryCatalog(Environment.CurrentDirectory));
            //Create the CompositionContainer with the parts in the catalog  
            _container = new CompositionContainer(catalog);

            //Fill the imports of this object  
            try
            {
                this._container.ComposeParts(this);
            }
            catch (CompositionException compositionException)
            {
                Console.WriteLine(compositionException.ToString());
            }
        }
        [STAThread]
        static void Main(string[] args)
        {

            Program p = new Program();

            Console.Title = "model-helper CLI";
            //p.CommandExecutor.Execute("h", null);

            //System.Console.ReadLine();

            var playTetris = args.Length > 0 && (args[0] == "tetris" || args[0] == "easter");
            var init = !ModelHelperExtensions.RootDirectoryExists();
            var options = args.Length > 0 ? args.ToList().GetRange(1, args.Length - 1) : new List<string>();
            var key = args.Length > 0 ? args[0] : "about";

            if (init)
            {
                var initCommand = new InitCommand();

                initCommand.Help = HelpFactory.Init();
                initCommand.Execute(options);

                //var command = CommandFactory.Init();
                //command.Execute(null);

                return;
            }
            else if (!playTetris)
            {
                ModelHelperConfig.ReadConfig();

                var modelHelperData = ConsoleExtensions.UserTemplateDirectory();

                var templateReader = new JsonTemplateReader();
                var customTemplatePath = ModelHelperConfig.TemplateLocation; // Path.Combine(Directory.GetCurrentDirectory(), "templates");

                var templateFiles = new List<TemplateFile>();

                templateFiles.AddRange(customTemplatePath.GetTemplateFiles("project"));
                templateFiles.AddRange(modelHelperData.GetTemplateFiles("mh"));

                ModelHelperConfig.Templates =
                    templateFiles.Select(t => templateReader.Read(t.FileInfo.FullName, t.Name)).ToList();

                p.CommandExecutor.Execute(key, options);
                
            }
            else
            {
                Console.Clear();
                EasterEgg.EasterEgg.Start();
            }

#if DEBUG
            System.Console.ReadLine();
#endif
            
        }                
    }
}