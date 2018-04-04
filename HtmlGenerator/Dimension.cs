using System;

namespace HtmlGenerator
{
    public class Dimension
    {
        public string Table { get; set; }
        public string PrimaryKey { get; set; }
    }

    public class Column
    {
        public Dimension Dimension { get; set; }
        public string Name { get; set; }
        public string Expression { get; set; }
    }

    public class Grouping
    {
        public Column Column { get; set; }
        public bool Group { get; set; }
    }

    public class ColumnValue
    {
        public Column Column { get; set; }
        public object Value { get; set; }
    }
}


