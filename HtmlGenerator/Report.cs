namespace HtmlGenerator
{
    public class Report
    {
        public Grouping[] Columns { get; set; }
        public Grouping[] Rows { get; set; }

        public Report()
        {
            Rows = new Grouping[0];
        }
    }
}
