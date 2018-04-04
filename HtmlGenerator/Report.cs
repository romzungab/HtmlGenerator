namespace HtmlGenerator
{
    public class Report
    {
        public FactTable FactTable { get; set; }
        public Grouping[] Rows { get; set; }
        public Grouping[] Columns { get; set; }
        public Measure[] Measures { get; set; }
        public ReportDataRow[] Data { get; set; }

        public Report()
        {
            Rows = new Grouping[0];
        }
    }

    public class ReportDataRow
    {
        public ColumnValue[] ColumnValues { get; set; }
        public MeasureValue[] MeasureValues { get; set; }
    }


}

