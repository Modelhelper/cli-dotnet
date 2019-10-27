using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using ModelHelper.Core.Configuration;
using Newtonsoft.Json;

namespace ModelHelper.Core.Extensions
{
    [Obsolete]
    public static class ModelHelperExtensions
    {
        [Obsolete("Moved to Application.RootFolder")]
        public static string RootFolder
        {
            get
            {
                return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                    "ModelHelper");
            }
        }

        [Obsolete("moved to application")]
        [DllImport("user32.dll")]
        internal static extern bool OpenClipboard(IntPtr hWndNewOwner);

        [Obsolete("moved to application")]
        [DllImport("user32.dll")]
        internal static extern bool CloseClipboard();

        [Obsolete("moved to application")]
        [DllImport("user32.dll")]
        internal static extern bool SetClipboardData(uint uFormat, IntPtr data);

        [Obsolete("moved to application")]
        public static bool ProjectFileExists()
        {
            var file = Path.Combine(Directory.GetCurrentDirectory(), ".model-helper");
            return File.Exists(file);
        }

        [Obsolete("moved to application")]
        public static bool RootDirectoryExists()
        {            
            return Directory.Exists(RootFolder);
        }

        

        [Obsolete]
        public static void CreateRootDirectory()
        {
            var binPath = new FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location);
            var rootFolder = RootFolder;

            var config = (File.Exists(Path.Combine(binPath.DirectoryName, "config.json"))) ?
                ModelHelperConfig.ReadConfig(Path.Combine(binPath.DirectoryName, "config.json")) :
                new Config
                {
                    RemoteTemplateLocation = "\\\\file.aventoloc.local\\software\\ModelHelper\\templates"
                };
           

            
            if (!Directory.Exists(rootFolder))
            {
                var security = new DirectorySecurity();
               
                var root = Directory.CreateDirectory(rootFolder);
                Directory.CreateDirectory(Path.Combine(rootFolder, "templates"));
                
                var file = Path.Combine(rootFolder, "config.json");

                var json = JsonConvert.SerializeObject(config);
                File.WriteAllText(file, json);
            }
        }

        [Obsolete]
        public static int FetchRemoteTemplates(string remoteLocation, string modelHelperRoot, bool overwrite, Action<int, int> progress = null)
        {
            var index = 0;
            if (Directory.Exists(remoteLocation))
            {
                var fileList = remoteLocation.GetTemplateFiles("");

                var files = Directory.GetFiles(remoteLocation);
                var totalCount = fileList.Count; //Length;
                
                foreach (var file in fileList)
                {

                    var fileInfo = file.FileInfo; // new FileInfo(file);
                    var fileCopy = Path.Combine(modelHelperRoot, file.SubFolder);
                    var fileCopyInfo = new FileInfo(fileCopy);
                    try
                    {
                        if (!File.Exists(fileCopy))
                        {
                            if (!Directory.Exists(fileCopyInfo.DirectoryName))
                            {
                                Directory.CreateDirectory(fileCopyInfo.DirectoryName);
                            }

                            fileInfo.CopyTo(fileCopy, overwrite);
                        }
                        else
                        {
                            if (overwrite)
                            {
                                fileInfo.CopyTo(fileCopy, overwrite);
                            }
                        }
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Could not copy file");
                        //throw;
                    }
                    
                    

                    progress?.Invoke(index, totalCount);
                    index++;
                }
            }
            else
            {
                Console.ResetColor();
                Console.WriteLine("Ser ikke ut til at det er mulig å koble seg opp til serveren for å hente maler");
            }

            return index;
        }

        [Obsolete]
        public static List<TemplateFile> GetTemplateFiles(this string folderPath, string scope)
        {
            var templateFiles = new List<TemplateFile>();

            try
            {
                if (Directory.Exists(folderPath))
                {
                    var templateDirectory = new DirectoryInfo(folderPath);
                    var customFiles = templateDirectory.GetFiles("*.json", SearchOption.AllDirectories).ToList();
                    //var customFiles = Directory.GetFiles(folderPath, "*.json").Select(f => new FileInfo(f))
                    //    .ToList();

                    //foreach (var fileInfo in customFiles)
                    //{
                    //    var fileKey = fileInfo.FullName.Replace(folderPath, "").Replace("\\", "-").TrimStart('-').Replace(fileInfo.Extension, "");
                    //    var templateName = fileInfo.Name.LastIndexOf(".", StringComparison.Ordinal) > 0
                    //        ? fileInfo.Name.Substring(0, fileInfo.Name.LastIndexOf(".", StringComparison.Ordinal))
                    //        : fileInfo.Name;

                    //}

                    templateFiles.AddRange(customFiles.Select(fileInfo => new TemplateFile
                    {
                        FileInfo = fileInfo,
                        Location = fileInfo.FullName,
                        Scope = scope,
                        SubFolder = fileInfo.FullName.Replace(folderPath, "").TrimStart('\\'),
                        Name = fileInfo.FullName.Replace(folderPath, "").Replace("\\", "-").TrimStart('-').Replace(fileInfo.Extension, "")
                        //Name = fileInfo.Name.LastIndexOf(".", StringComparison.Ordinal) > 0
                        //        ? fileInfo.Name.Substring(0, fileInfo.Name.LastIndexOf(".", StringComparison.Ordinal))
                        //        : fileInfo.Name
                    }));
                }
            }
            catch (Exception)
            {

                //throw;
            }
            

            return templateFiles;
        }

        [Obsolete]
        public static void ToClipboard(string text)
        {
            OpenClipboard(IntPtr.Zero);
            var ptr = Marshal.StringToHGlobalUni(text);
            SetClipboardData(13, ptr);
            CloseClipboard();
            Marshal.FreeHGlobal(ptr);
        }
    }
}