using System.Collections.Generic;

namespace ModelHelper
{
    public class ConsoleTableRow
    {
        public ConsoleTableRow()
        {
            Values = new List<RowValue>();
        }
        public List<RowValue> Values { get; set; }
        

    }
}