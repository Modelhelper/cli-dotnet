using System.IO;

namespace ModelHelper.Core
{
    public interface IModelHelperDefaults
    {
        DirectoryInfo RootDirectory { get; }

        DirectoryInfo CurrentProjectDirectory { get; }
        FileInfo CurrentProjectFile { get; }
        string ProjectFileName { get; } //= "project.json";
        string ProjectDirectoryName { get; } // = ".modelhelper"
        string ConfigFileName { get; } // = ".config.yaml"
        string RemoteProjectDefinitionZipFile { get; } // = "project-definitions.zip"
        string RemoteCodeDefinitionZipFile { get; } // = "code-definitions.zip"
        string RemoteTemplateZipFile { get; } // = "mh-templates.zip"


    }
}