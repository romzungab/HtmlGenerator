using System.Diagnostics;

namespace HtmlGenerator
{
    public class Report
    {
        public string BaseTable { get; set; }
        public Grouping[] Columns { get; set; }
        public Grouping[] Rows { get; set; }

        public ReportDataRow[] Data { get; set; }

        public Report()
        {
            Rows = new Grouping[0];
        }
    }

    public class ReportDataRow
    {
        public DimensionValue[] Dimensions { get; set; }
        public object[] Measures { get; set; }
    }

    [DebuggerDisplay("{Dimension} = {Value}")]
    public class DimensionValue
    {
        public Dimension Dimension { get; set; }
        public object Value { get; set; }
    }

}

