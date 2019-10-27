using ModelHelper.Core.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelHelper.Core.Remote
{
    public static class Download
    {

        private  static void GetDownload(Uri uri, string downloadPath, Action<int> progress, Action<DownloadInfo> completed)
        {
            
            if (File.Exists(downloadPath))
            {
                File.Delete(downloadPath);
            }

            try
            {                
                var wc = new System.Net.WebClient();
                
                wc.DownloadProgressChanged += (object sender, System.Net.DownloadProgressChangedEventArgs e) =>
                {
                    if (progress != null) progress.Invoke(e.ProgressPercentage);
                };

                wc.DownloadFileCompleted += (object sender, System.ComponentModel.AsyncCompletedEventArgs e) =>
                {
                    if (completed != null) completed.Invoke(new DownloadInfo(downloadPath));
                };

                wc.DownloadFileTaskAsync(uri, downloadPath).Wait();

               
                
                
            }
            catch (Exception e)
            {

                throw;
            }
        }

        
        //public static DownloadInfo ProjectDefinitions(Uri uri)
        //{            
        //    var downloadPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), Constants.RemoteProjectDefinitionZipFile);            

        //    var info = GetDownload(uri, downloadPath);

        //    return info;
        //}

        //public static DownloadInfo CodeDefinitions(Uri uri)
        //{            
        //    var downloadPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), Constants.RemoteCodeDefinitionZipFile);


        //    var info = GetDownload(uri, downloadPath);

        //    return info;

        //}

        public static void Templates(Uri uri, Action<int> progress, Action<DownloadInfo> completed)
        {

            var downloadPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), Constants.RemoteTemplateZipFile);           

            GetDownload(uri, downloadPath, progress, completed);


        }
    }

    public static class ContentExtacor
    {
        public static void ExtractFiles(this DownloadInfo info, string destination, bool clearContent = true)
        {
            
            var di = new DirectoryInfo(destination);
            if (!di.Exists)
            {
                di.Create();
            }

            if (clearContent)
            {                
                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }
                foreach (DirectoryInfo dir in di.GetDirectories())
                {
                    dir.Delete(true);
                }
            }            
            
            ZipFile.ExtractToDirectory(info.DownloadPath, di.FullName);
        }
    }

    public class DownloadInfo
    {
        public DownloadInfo(string path)
        {
            DownloadPath = path;
        }
        public string DownloadPath { get; private set; }
        
    }
}
