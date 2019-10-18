using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using ModelHelper.Commands;
using ModelHelper.Core;
using ModelHelper.Core.Configuration;
using ModelHelper.Core.Extensions;
using ModelHelper.Core.Templates;
using ModelHelper.Extensions;
using ModelHelper.Update;

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

            // first use?
            if (!ApplicationDefaults.RootDirectory.Exists)
            {
                var installer = new ApplicationInstaller();
                installer.Run();

                Console.WriteLine("Press any key to exit applicatoin");
                Console.ReadLine();
                return;
            }
            else
            {
                // do we need to update
                
                var updater = new ApplicationUpdater();

                try
                {
                    var updated = updater.Run(args);

                    if (!updater.ContinueWithCommand)
                    {
                        //return;
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine("Update was interrupted");
                    return;
                }
                
            }

            

            if (DoEaster(args))
            {
                RunEaster(args);
            }
            else 
            {
                var context = CreateContextBuilder(args, ApplicationDefaults.CurrentProjectDirectory)
                    .Build();

                // need to load templates
                // 1: global
                // 2: shared
                // 3: project

                // Application.Templates = Application.LoadTemplates();


                // remove this (start)
                var modelHelperData = ModelHelperConfig.TemplateLocation; // ConsoleExtensions.UserTemplateDirectory();

                var templateReader = new JsonTemplateReader();
                var customTemplatePath = Path.Combine(Directory.GetCurrentDirectory(), "templates");

                var templateFiles = new List<TemplateFile>();

                templateFiles.AddRange(customTemplatePath.GetTemplateFiles("project"));
                templateFiles.AddRange(modelHelperData.GetTemplateFiles("mh"));

                Application.Templates =
                    templateFiles.Select(t => templateReader.Read(t.FileInfo.FullName, t.Name)).ToList();

                // end

                p.CommandExecutor.Execute(context);
                
            }
            

#if DEBUG
            System.Console.ReadLine();
#endif
            
        }                

        static IApplicationContextBuilder CreateContextBuilder(string[] args, DirectoryInfo projectDirectory)
        {
            return new ApplicationContextBuilder(args, projectDirectory);
        }

        static bool DoEaster(string[] args)
        {
            var playTetris = args.Length > 0 && (args[0] == "tetris" || args[0] == "easter");
            var doMatrix = args.Length > 0 && (args[0] == "matrix" || args[0] == "easter");
            return doMatrix || playTetris;
        }

        static void RunEaster(string[] args)
        {
            Console.Clear();
            var playTetris = args.Length > 0 && (args[0] == "tetris" || args[0] == "easter");
            var doMatrix = args.Length > 0 && (args[0] == "matrix" || args[0] == "easter");

            if (playTetris)
            {
                Console.Clear();
                EasterEgg.EasterEgg.Start();
            }
            else if (doMatrix)
            {
                
            }
        }
    }
}