using System.Collections.Generic;
using System.Linq;


namespace HtmlGenerator
{
    public static class SQLGenerator
    {
        public static string BuildSQL(Report report)
        {
            //with the assumption that there is already a fact table

            var sql = SelectColumns(AllColumns(report));
            return JoinFactTableWithDimensions(sql, report) + "\norder by 1, 2";
        }
        private static List<Column> AllColumns(Report report)
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
            for (var i = 0; i < columns.Count; i++)
            {
                if (i == columns.Count - 1)
                    sql = sql + columns[i].Name+"\n";
                else
                    sql = sql + columns[i].Name + ",\n\t";
            }
            return sql;
        }

        public static string BuildFactTableActivity(Report report)
        {
            var allDim = AllColumns(report);

            var sql = "select\n\t";
            for (var i = 0; i < allDim.Count; i++)
            {
                if (i == allDim.Count - 1)
                    sql = sql + allDim[i].Table + "." + allDim[i].PrimaryKey + " as " + allDim[i].PrimaryKey + "\n";
                else
                    sql = sql + allDim[i].Table + "." + allDim[i].PrimaryKey + " as " + allDim[i].PrimaryKey + ",\n\t";
            }
            sql = sql + "from " + report.BaseTable + "\n";

            return allDim.Aggregate(sql, (current, r) => current + " inner join " + r.Table + " on " + r.Table + "." + r.PrimaryKey + " = " + report.BaseTable + "." + r.PrimaryKey + "\n");
        }
        
        private static List<Dimension> AllDimensions(Report report)
        {
            var allDim = new List<Dimension>();
            foreach (var c in report.Columns)
            {
                if (allDim.Where(d => d.Table == c.Dimension).Contains(c.Dimension))
                    continue;
                allDim.Add(c.Dimension);
            }

            foreach (var r in report.Rows)
            {
                if (allDim.Where(d => d.Table == r.Dimension.Table).Contains(r.Dimension))
                    continue;
                allDim.Add(r.Dimension);
            }
            return allDim;
        }

        private static List<Dimension> GetAllUniqueDimensions(IEnumerable<Dimension> allDim)
        {
            var uniqueDim = new List<Dimension>();
            foreach (var dim in allDim)
            {
                if (!(uniqueDim.Any(d => d.Table == dim.Table)))
                    uniqueDim.Add(dim);
            }
            return uniqueDim;
        }

        public static string GroupFactTable(string factSql, Report report)
        {
            var allDim = AllDimensions(report);
            var sql = "(\nselect\n\t";
            for (var i = 0; i < allDim.Count; i++)
            {
                if (i == allDim.Count - 1)
                    sql = sql + report.BaseTable + allDim[i].PrimaryKey + "\n";
                else
                    sql = sql + report.BaseTable + allDim[i].PrimaryKey + ",\n\t";
            }
            sql = sql + "from(\n" + factSql + ") as factTable\n";
            sql = sql + "group by ";
            for (var i = 0; i < allDim.Count; i++)
            {
                if (i == allDim.Count - 1)
                    sql = sql + report.BaseTable + "." + allDim[i].PrimaryKey + "\n";
                else
                    sql = sql + report.BaseTable + "." + allDim[i].PrimaryKey + ", ";
            }

            return sql;
        }

        public static string SelectFromFactTable(Report report)
        {
            var sql = "select\n\t";
            sql = sql + report.BaseTable +".*\n\t";
            sql = sql + "count(1) Measure\n";
            sql = sql + "from "+ report.BaseTable +"\n";
            sql = sql + "group by ";
            for (var i = 0; i < report.BaseTable.Columns.Length; i++)
            {
                if (i == report.BaseTable.Columns.Length - 1)
                    sql = sql + report.BaseTable + "." + report.BaseTable.Columns[i] + "\n";
                else
                    sql = sql + report.BaseTable + "." + report.BaseTable.Columns[i] + ", ";
            }

            return sql;
        }

        public static string JoinFactTableWithDimensions(Report report)
        {
            var selectColumns = SelectColumns(AllColumns(report));
            selectColumns = selectColumns + "Measure";
           var  sql = selectColumns + "\nfrom (\n" + SelectFromFactTable(report) + ") as g\n";

            return allDim.Aggregate(sql, (current, dim) => current + "\nleft join " + dim.Table + " on " + dim.Table + "." + dim.PrimaryKey + " = " + "g." + dim.PrimaryKey);
        }

    }
}
