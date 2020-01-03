using System;

namespace ModelHelper.Core
{
    
    public interface IModelHelperConfiguration
    {
        string ConfigVersion { get; set; }
        string AppVersion { get; set; }
        string RemoteTemplateDownloadLocation { get; set; }
        string LogLocation { get; set;  }

         ConfigLocation Shared { get; set; }
         ConfigLocation Global { get; set; }
    }
}