using System.Collections.Generic;

namespace ModelHelper
{
    public class ConsoleTable
    {
        public ConsoleTable()
        {
            Header = new ConsoleTableRow();
            Rows = new List<ConsoleTableRow>();
            UseHeader = true;
        }
        public bool UseHeader { get; set; }

        public ConsoleTableRow Header { get; set; }
        public List<ConsoleTableRow> Rows { get; set; }

        
    }
}