namespace HtmlGenerator
{
    public class Report
    {
        public FactTable FactTable { get; set; }
        public Grouping[] Rows { get; set; }
        public Grouping[] Columns { get; set; }
        public ReportDataRow[] Data { get; set; }

        public Report()
        {
            Rows = new Grouping[0];
        }
    }

    public class ReportDataRow
    {
        public DimensioValue[] DimensionValues { get; set; }
        public Measure Measure { get; set; }
    }

    public class Measure
    {
        public string Expression { get; set; }
        public string AggregationFunction { get; set; }
    }
}

