using System;

namespace ModelHelper
{
    public class LogItem
    {
        public DateTime Date { get; set; }
        public string Title { get; set; }
        public bool IsWarning { get; set; }
        public bool IsError { get; set; }
        public string Content { get; set; }
    }
}