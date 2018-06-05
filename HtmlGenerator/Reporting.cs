using System;
using System.Security.Cryptography.X509Certificates;

namespace HtmlGenerator
{
    public class Report
    {
        public FactTable FactTable;
        public ReportColumn[] Columns = new ReportColumn[0];
        public DimensionAttribute[] Rows = new DimensionAttribute[0];
        public Measure[] Measures;
        public ReportFilter[] Filters = new ReportFilter[0];
    }

    public class ReportColumn
    {
        public DimensionAttribute Attribute;
        public bool Grouped;
    }

    public class FactTable
    {
        public string Name;
        public string Table;
        public Dimension[] Dimensions;
        public Measure[] Measures;
    }

    public class Dimension
    {
        public string Name;
        public string Table;
        public DimensionAttribute[] Attributes;
    }

    public class DimensionAttribute
    {
        public DimensionAttribute(string expression)
            : this(t => $"{t}.{expression}")
        {
        }

        public DimensionAttribute(Func<string, string> expression)
        {
            Expression = expression;
        }

        public string Name;
        public Func<string, string> Expression { get; }
        public Dimension Dimension;
    }

    public class Measure
    {
        public Measure(string expression)
            : this(t => $"{t}.{expression}")
        {
        }

        public Measure(Func<string, string> expression)
        {
            Expression = expression;
        }

        public string Name;
        public Func<string, string> Expression;
        public string AggregationFunction;
        public FactTable Table;
    }

    public class ReportFilter
    {
        public DimensionAttribute Filter;
        public string Value;
        public string Operation;
    }
}
