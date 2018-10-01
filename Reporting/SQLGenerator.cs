using System.Collections.Generic;
using System.Linq;

namespace Reporting
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
            var where = new List<string>();
            var groupBys = new List<string>();
            var orderBys = new List<string>();

            var aliases = new Dictionary<Dimension, string>();

            foreach (var gr in GetReportColumns().GroupBy(da => da.Dimension))
            {
                var dim = gr.Key;
                var alias = dim.Table != null ? $"d{joins.Count}" : factTable;
                aliases.Add(dim, alias);

                if (dim.Table != null)
                    joins.Add($"left join {dim.Table} [{alias}] on [{alias}].[Id] = [{factTable}].[{dim.Name}Id]");

                groupBys.AddRange(gr.Select(da => $"{da.Expression(alias)}"));
                selects.AddRange(gr.Select(da => $"{da.Expression(alias)} [{da.Name}]"));
            }

            foreach (var da in _report.OrderBys)
            {
                var sortExpr = $"{da.SortExpression(aliases[da.Dimension])}";
                orderBys.Add(sortExpr);
                groupBys.Add(sortExpr);
                selects.Add($"{sortExpr} [{da.Name}Order]");

            }

            selects.AddRange(_report.Measures.Select(m => $"{m.AggregationFunction}({m.Expression(factTable)}) [{m.Name}]"));
           
            if (_report.Filters.Count > 0)
                where.AddRange(_report.Filters.Select(f => f.ToString()));

            var sql = $"select {string.Join(",\n\t", selects)}\n" +
                $"from {_report.FactTable.Table} [{factTable}]\n" +
                $"{string.Join("\n", joins)}\n";

            if (where.Count > 0)
                sql += $"where {string.Join(" and ", where)}\n";

            sql += $"group by {string.Join(", ", groupBys)}\n";

            if (orderBys.Count > 0)
                sql += $"order by {string.Join(", ", orderBys)}";

            return sql;
        }

        public DimensionAttribute[] GetReportColumns()
        {
            return _report.Columns.Select(x => x.Attribute).Concat(_report.Rows).ToArray();
        }

      }
}