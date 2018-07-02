using System;
using System.Collections.Generic;

namespace Reporting
{
    public class Report
    {
        public FactTable FactTable;
        public ReportColumn[] Columns = new ReportColumn[0];
        public DimensionAttribute[] Rows = new DimensionAttribute[0];
        public Measure[] Measures;
        public List<BaseFilter> Filters = new List<BaseFilter>();
        public DimensionAttribute[] OrderBys = new DimensionAttribute[0];
    }

    public class ReportColumn
    {
        public DimensionAttribute Attribute;
        public bool Pivot;
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
            SortExpression = Expression = expression;
        }

        public string Name { get; set; }
        public Func<string, string> Expression { get; }
        public Func<string, string> SortExpression { get; set; }
        public Dimension Dimension { get; set; }
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

    public abstract class BaseFilter
    {
        public DimensionAttribute Filter;
    }
    
    public class SimpleFilter : BaseFilter
    {
        public string Value;
        public string Operation;

        public override string ToString()
        {
            return $"{Filter.Name} {Operation} '{Value}'";
        }
    }

    public class InFilter : BaseFilter
    {
        public bool Not;
        public IEnumerable<string> InValues;

        public override string ToString()
        {
            return Not ? $" {Filter.Name} NOT IN ({string.Join(", ", InValues )})" : $" {Filter.Name} IN ({string.Join(", ", InValues )})";
        }
    }

    public class BetweenFilter : BaseFilter
    {
        public string Value1;
        public string Value2;
        public bool Not;

        public override string ToString()
        {
            return Not ? $"NOT({Filter.Name} BETWEEN '{Value1}' and '{Value2}')" : $"{Filter.Name} BETWEEN '{Value1}' and '{Value2}'";
        }
    }
}