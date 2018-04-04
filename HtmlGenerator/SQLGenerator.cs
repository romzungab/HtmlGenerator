using System.Collections.Generic;
using System.Linq;


namespace HtmlGenerator
{
    public static class SQLGenerator
    {
        public static string BuildSQL(Report report)
        {
            //with the assumption that there is already a fact table

            var sql = SelectFromFactTable(report);
            return JoinFactTableWithDimensions(sql, report) + "\norder by 1, 2";
        }
        private static List<Column> GetAllColumns(Report report)
        {
            var allColumns = new List<Column>();
            foreach (var c in report.Columns)
            {
                if (allColumns.Contains(c.Column))
                    continue;
                else allColumns.Add(c.Column);
            }

            foreach (var e in report.Rows)
            {
                if (allColumns.Contains(e.Column))
                    continue;
                else allColumns.Add(e.Column);
            }
            return allColumns ;
        }

        public static string SelectColumns(List<Column> columns)
        {
            var sql = "select\n\t";
            foreach (var c in columns)
            {
                if (c == columns.Last())
                    sql = sql + c.Dimension.Table+ "."+ c.Name;
                else
                    sql = sql + c.Dimension.Table + "." + c.Name + ",\n\t";
            }
            return sql;
        }

        public static string SelectMeasures(Measure[] measures)
        {
            var sql=string.Empty;
            foreach (var m in measures)
            {
                if (m == measures.Last())
                    sql = m.Aggregation + " ( " + m.Expression + " ) as " + m.Name;
                else
                    sql = m.Aggregation + " ( " + m.Expression + " ) as " + m.Name +",";
            }
            return sql;
        }

        public static string SelectFromFactTable(Report report)
        {
            var sql = "select\n\t";
            sql = sql + report.FactTable.Table +".*,\n\t";
            sql = sql + "count(1) Measure\n";
            sql = sql + "from "+ report.FactTable.Table +"\n";
            sql = sql + "group by ";
            for (var i = 0; i < report.FactTable.Columns.Length; i++)
            {
                if (i == report.FactTable.Columns.Length - 1)
                    sql = sql + report.FactTable.Table + "." + report.FactTable.Columns[i] + "\n";
                else
                    sql = sql + report.FactTable.Table + "." + report.FactTable.Columns[i] + ", ";
            }
            return sql;
        }

        public static string JoinFactTableWithDimensions(string fsql, Report report)
        {
            var allCol = GetAllColumns(report);
            var selectColumns = SelectColumns(allCol);
            var selectMeasures = SelectMeasures(report.Measures);
            var  sql = selectColumns + "\nfrom (\n" + fsql + ") as "+ report.FactTable.Table;
            return report.FactTable.Dimensions.Aggregate(sql, (current, dim) => current + "\nleft join " + dim.Table + " on " + dim.Table + "." + dim.PrimaryKey + " = " + report.FactTable.Table + "." + dim.PrimaryKey);
        }

    }
}