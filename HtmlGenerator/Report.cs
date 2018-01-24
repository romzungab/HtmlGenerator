namespace HtmlGenerator
{
    public class Report
    {
        public FactTable FactTable { get; set; }
        public Grouping[] Rows { get; set; }
        public Grouping[] Data { get; set; }

        public Report()
        {
            Rows = new Grouping[0];
        }
    }

}

