using System.Collections.Generic;
using System.Linq;

namespace HtmlGenerator
{
    public class SQLGenerator
    {
        private readonly Report _report;

        public SQLGenerator(Report report)
        {
            _report = report;
        }

        public string BuildSQL()
        {
            const string factTable = "f";

            var joins = new List<string>();
            var selects = new List<string>();
            var groupBys = new List<string>();

            foreach (var gr in GetReportColumns().GroupBy(da => da.Dimension))
            {
                var dim = gr.Key;
                var alias = dim.Table != null ? $"d{joins.Count}" : factTable;

                if (dim.Table != null)
                    joins.Add($"left join {dim.Table} [{alias}] on [{alias}].[Id] = [{factTable}].[{dim.Table}Id]");

                groupBys.AddRange(gr.Select(da => $"{da.Expression(alias)}"));
                selects.AddRange(gr.Select(da => $"{da.Expression(alias)} [{da.Name}]"));
            }

            selects.AddRange(_report.Measures.Select(m => $"{m.AggregationFunction}({m.Expression(factTable)}) [{m.Name}]"));

            return $"select {string.Join(",\n\t", selects)}\n" +
                $"from {_report.FactTable.Table} [{factTable}]\n" +
                $"{string.Join("\n", joins)}\n" +
                $"group by {string.Join(", ", groupBys)}";
        }

        public DimensionAttribute[] GetReportColumns()
        {
            return _report.Columns.Select(x => x.Attribute).Concat(_report.Rows).ToArray();
        }

        public static string SelectFromFactTable(Report report)
        {
            var dims = GetReportDimensions(report);
            var sql = "select\n\t";
            var cols = string.Empty;
            var measures = string.Empty;

            foreach (var d in dims)
            {
                if (string.IsNullOrEmpty(d.Table))
                    cols += $"f.{d.Name}, ";
                else
                    cols += $"f.{d.Table}Id, ";
            }
            var groupBy = cols;

            foreach (var m in report.Measures)
            {
                if (m == report.Measures.Last())
                {
                    measures += $"{m.AggregationFunction} ({m.Expression("f")}) as {m.Name}\n"; ;
                }
                else
                {
                    measures += $"{m.AggregationFunction} ({m.Expression("f")}) as {m.Name},";
                }
            }

            sql = sql + cols + measures;
            sql = sql + "from " + report.FactTable.Table + " f \n";

            //TODO: add where clause for filters here

            if (report.Measures != null)
                sql = sql + "group by " + groupBy + "\n";

            return sql;
        }

        private static IEnumerable<Dimension> GetReportDimensions(Report report)
        {
            return report.Columns.Select(c => c.Attribute).Concat(report.Rows).Select(da => da.Dimension).Distinct();
        }
    }
}

