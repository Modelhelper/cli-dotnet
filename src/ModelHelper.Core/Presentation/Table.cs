using System.Collections.Generic;

namespace ModelHelper.Extensions.Presentation
{
    public class Table
    {
        public Table()
        {
            Header = new Row();
            Rows = new List<Row>();
            UseHeader = true;
        }
        public bool UseHeader { get; set; }

        public Row Header { get; set; }
        public List<Row> Rows { get; set; }

        
    }

    public class Row
    {
        public Row()
        {
            Values = new List<RowValue>();
        }
        public List<RowValue> Values { get; set; }
        

    }

    public class RowValue
    {
        public RowValue()
        {
            Alignment = RowValueAlignment.Left;
        }
        public string Value { get; set; }

        public RowValueAlignment Alignment { get; set; }
        public bool UseColor { get; set; }
        public System.ConsoleColor Color { get; set; }
    }

    public enum RowValueAlignment
    {
        Left,
        Right,
        Center
    }
}