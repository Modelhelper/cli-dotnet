using DotLiquid;

namespace ModelHelper.Core.Drops
{
    public class CodeLocationDrop : Drop
    {
        public CodeLocationDrop(string key, string ns, string path)
        {
            Key = key;
            Namespace = ns;
            Path = path;
        }
        public string Key { get; }
        public string Namespace { get;  }
        public string Path { get; }
    }
}