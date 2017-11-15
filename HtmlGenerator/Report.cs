namespace HtmlGenerator
{
    public class Report
    {
        public string BaseTable { get; set; }
        public Grouping[] Columns { get; set; }
        public Grouping[] Rows { get; set; }

        public Report()
        {
            Rows = new Grouping[0];
        }
    }
}
