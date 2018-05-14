using System.Collections.Generic;
using System.Linq;
using System.Threading;


namespace HtmlGenerator
{
    public static class SQLGenerator
    {
        public static string BuildSQL(Report report)
        {
            //with the assumption that there is already a fact table
            var sql = JoinFactTableWithDimensions(SelectFromFactTable(report), report);
            if (report.Measures != null)
                sql = AddGroupBy(sql, report);
            return sql;
        }

        //private static IEnumerable<Grouping> GetAllColumns(Report report)
        //{
        //    var allColumns = new List<Grouping>();
        //    foreach (var group in report.Columns)
        //    {
        //        if (allColumns.Contains(group))
        //            continue;
        //        else allColumns.Add(group);
        //    }

        //    foreach (var e in report.Rows)
        //    {
        //        if (allColumns.Contains(e.Column))
        //            continue;
        //        else allColumns.Add(e.Column);
        //    }

        //    return allColumns;
        //}

        public static string SelectColumns(Grouping[] groups)
        {
            var sql = "select\n\t";
            foreach (var g in groups)
            {
                if (string.IsNullOrEmpty(g.Column.Expression))
                {
                    if (g == groups.Last())
                        sql = sql + g.Source.Table + "." + g.Column.Name;
                    else
                        sql = sql + g.Source.Table + "." + g.Column.Name + ",\n\t";
                }
                else
                {
                    if (g == groups.Last())
                        sql = sql + g.Column.Expression + " " + g.Column.Name;
                    else
                        sql = sql + g.Column.Expression + " " + g.Column.Name + ",\n\t";
                }
            }

            return sql;
        }

        public static string SelectMeasures(Measure[] measures)
        {
            var sql = ",\n\t";
            foreach (var m in measures)
            {
                if (m == measures.Last())
                    sql += m.Aggregation + " ( " + m.Expression + " ) as " + m.Name;
                else
                    sql += m.Aggregation + " ( " + m.Expression + " ) as " + m.Name + ",\n\t";
            }

            return sql;
        }

        public static string SelectFromFactTable(Report report)
        {
            var sql = "select\n\t";
            sql = sql + report.FactTable.Table + ".*\n\t";
            sql = sql + "from " + report.FactTable.Table + "\n";
          
            return sql;
        }

        public static string JoinFactTableWithDimensions(string fsql, Report report)
        {
            var selectColumns = SelectColumns(report.Columns.Concat(report.Rows).ToArray());
            var selectMeasures = string.Empty;
            if (report.Measures != null && report.Measures.Length > 0)
                selectMeasures = SelectMeasures(report.Measures);
            var sql = selectColumns + " " + selectMeasures + "\nfrom (\n" + fsql + ") as " + report.FactTable.Table;
            return report.FactTable.Dimensions.Aggregate(sql, (current, dim) => current + "\nleft join " + dim.Table + " on " + dim.Table + "." + dim.PrimaryKey + " = " + report.FactTable.Table + "." + dim.PrimaryKey);
        }

        public static string AddGroupBy(string fsql, Report report)
        {
            var allCols = report.Columns.Concat(report.Rows).ToArray();
            var sql = fsql + "\ngroup by ";
            foreach (var c in allCols)
            {
                if (c.Column.GroupBys != null && c.Column.GroupBys.Length > 0)
                {
                    sql = c.Column.GroupBys.Aggregate(sql, (current, t) => current + t);
                }
                else
                {
                    if (string.IsNullOrEmpty(c.Column.Expression))
                        sql += c.Source.Table + "." + c.Column.Name;
                    else
                        sql += c.Column.Expression ;
                }
                if(c != allCols.LastOrDefault())
                    sql += ",\n";
            }

            return sql;
        }
    }
}

