
using System;

namespace HtmlGenerator
{
    public class Report
    {
        public FactTable FactTable { get; set; }
        public Grouping[] Rows { get; set; }
        public Grouping[] Columns { get; set; }
        public Measure[] Measures { get; set; }
        public ReportDataRow[] Data { get; set; }
        public ReportFilter[] Filters { get; set; }

        public Report()
        {
            Rows = new Grouping[0];
        }
    }

    public class Grouping
    {
        public ViewSource Source { get; set; }
        public Column Column { get; set; }
        public bool Group { get; set; }
    }

    public class ReportDataRow
    {
        public ColumnValue[] ColumnValues { get; set; }
        public MeasureValue[] MeasureValues { get; set; }
    }

    public class ReportFilter
    {
       public string Expression { get; set; }
    }
}

