using System;
using System.IO;
using ModelHelper.Core;

namespace ModelHelper.Cli
{
    public class ModelHelperDefaults : IModelHelperDefaults
    {
        public DirectoryInfo RootDirectory
        {
            get
            {
                var p = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "ModelHelper");
                return new DirectoryInfo(p);
            }
        }

        public DirectoryInfo CurrentProjectDirectory
        {
            get
            {
                var p = Path.Combine(Directory.GetCurrentDirectory(), this.ProjectDirectoryName);
                return new DirectoryInfo(p);
            }
        }

        public FileInfo CurrentProjectFile
        {
            get
            {
                var p = Path.Combine(Directory.GetCurrentDirectory(), ProjectDirectoryName, ProjectFileName);
                return new FileInfo(p);
            }
        }
        public FileInfo ConfigurationFile
        {
            get
            {
                var p = Path.Combine(RootDirectory.FullName, VersionfolderName, ConfigFileName);
                return new FileInfo(p);
            }
        }
        
        public string ProjectFileName => "project.json";
        public string ProjectDirectoryName => ".model-helper";
        public string ConfigFileName => "config.yaml";
        public string VersionfolderName => "v3";
        public string RemoteProjectDefinitionZipFile => "project-definitions.zip";
        public string RemoteCodeDefinitionZipFile => "code-definitions.zip";
        public string RemoteTemplateZipFile => "mh-templates.zip";
    }

    
}
