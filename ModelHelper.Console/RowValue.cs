using System;

namespace ModelHelper
{
    public class RowValue
    {
        public RowValue()
        {
            Alignment = RowValueAlignment.Left;
        }
        public string Value { get; set; }

        public RowValueAlignment Alignment { get; set; }
        public bool UseColor { get; set; }
        public ConsoleColor Color { get; set; }
    }

    public enum RowValueAlignment
    {
        Left,
        Right,
        Center
    }
}