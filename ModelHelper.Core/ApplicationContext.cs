using ModelHelper.Core.Configuration;
using ModelHelper.Core.Project;
using ModelHelper.Core.Templates;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelHelper.Core
{
    public class ApplicationContext
    {
        internal ApplicationContext()
        {
            Id = Guid.NewGuid();
        }

        public string CommandKey { get; set; }
        public List<string> Options { get; set; }

        public GlobalConfig Config { get; set; }

        public IProject CurrentProject { get; set; }

        public string[] Arguments { get; set; }

        public List<ITemplate> Templates { get; set; }

        public Guid Id { get; private set; }

        public DirectoryInfo RootDirectory { get; set; }     
        public DirectoryInfo ProjectDirectory { get; set; }

        public FileInfo ProjectFile { get; set; }  

    }

    public interface IApplicationContextBuilder
    {
        ApplicationContext Build();
    }

    public class ApplicationContextBuilder : IApplicationContextBuilder       
    {
        private readonly string[] args;
        private readonly DirectoryInfo projectDiretory;

        public ApplicationContextBuilder(string[] args, DirectoryInfo projectDiretory)
        {
            this.args = args;
            this.projectDiretory = projectDiretory;
        }
        public ApplicationContext Build()
        {
            var context = new ApplicationContext
            {
                Config = GlobalConfig.Load(),
                RootDirectory = ApplicationDefaults.RootDirectory,
                Arguments = args,
                ProjectDirectory = this.projectDiretory,
                CommandKey = args.Length > 0 ? args[0] : "about",
                Options = args.Length > 0 ? args.ToList().GetRange(1, args.Length - 1) : new List<string>(),
                // ProjectFile = 

        };
            
            return context;
        }
        
    }
}
