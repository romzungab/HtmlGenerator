using System;

namespace HtmlGenerator
{
    public class Dimension
    {
        public string Table { get; set; }
        public string PrimaryKey { get; set; }
        public string ColName { get; set; }
        public string[] Values { get; set; }
    }

    public class Grouping
    {
        public Dimension Dimension { get; set; }
        public bool Group { get; set; }
    }
}

