namespace HtmlGenerator
{
    public class Report
    {
        public FactTable BaseTable { get; set; }
        public Grouping[] Rows { get; set; }
        public Grouping[] Columns { get; set; }

        public Report()
        {
            Rows = new Grouping[0];
        }
    }

}

