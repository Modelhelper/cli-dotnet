using System.Collections.Generic;

namespace ModelHelper.Core.Help
{
    public class HelpItem
    {
       

        public HelpItem()
        {
            Options = new List<HelpOption>();
            ConsoleMessages = new Dictionary<string, string>();
            Samples = new List<HelpSample>();
        }
        public string Key { get; set; }
        public string Alias { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public List<HelpOption> Options { get; set; }

        public List<HelpSample> Samples { get; set; }
        public string GetMessage(string key, params string[] args)
        {
            if (ConsoleMessages.ContainsKey(key))
            {
                return ConsoleMessages[string.Format(key, args)];
            }
            else
            {
                return string.Empty;
            }
        }
        public Dictionary<string, string> ConsoleMessages { get; set; }
        public string Title { get; set; }
        public string Signature { get; set; }
    }
}