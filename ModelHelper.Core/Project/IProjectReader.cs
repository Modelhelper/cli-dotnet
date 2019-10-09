using System;
using System.Collections.Generic;
using ModelHelper.Core.Project.V1;

namespace ModelHelper.Core.Project
{
    public interface IProjectReader<T>
    {
        string CurrentVersion { get;  }
        T Read(string path);
        ProjectVersion CheckVersion(string path);


    }

    public interface IProjectConverter<TFrom, TTo>
    {
        TTo Convert(TFrom from);
    }
    public interface IProjectVersionChecker
    {
        VersionCheckerResult Check(string path);
    }

    public class VersionCheckerResult
    {
        public bool MustConvert { get; set; }

    }

    public class ProjectVersion
    {
        public bool MustUpdate { get; internal set; }
        public string Version { get; set; }

        public int? Major { 
            get 
            {
                if (!string.IsNullOrEmpty(Version))
                {
                    
                    var va = Version.Split(new string[]{"."}, StringSplitOptions.RemoveEmptyEntries);                    
                    int v = 0;

                    if (va.Length > 0)
                    {
                        Int32.TryParse(va[0].ToString(), out v);
                        return v;
                    }
                    
                }

            return null;
        } }
        public int? Minor { 
            get
            {
                if (!string.IsNullOrEmpty(Version))
                {
                    
                    var va = Version.Split(new string[]{"."}, StringSplitOptions.RemoveEmptyEntries);                    
                    int v = 0;

                    if (va.Length > 1)
                    {
                        Int32.TryParse(va[1].ToString(), out v);
                        return v;
                    }
                    
                }

            return null;
        }
         }
        public bool IsBeta
        {
            get { return string.IsNullOrEmpty(Version); }
        }


    }
}