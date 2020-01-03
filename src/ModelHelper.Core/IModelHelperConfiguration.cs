namespace ModelHelper.Core
{
    public interface IModelHelperConfiguration
    {
        string ConfigVersion { get;}
        string AppVersion { get;}
        string RemoteTemplateDownloadLocation { get;}
        string LogLocation { get;}

        
    }
}