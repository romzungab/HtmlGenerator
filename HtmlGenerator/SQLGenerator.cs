using System;
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

        public static string BuildFactTableActivity(Report report)
        {
            var allDim = AllDimensions(report);

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
                if (allDim.Contains(c.Dimension))
                    continue;
                allDim.Add(c.Dimension);
            }
            
            foreach (var r in report.Rows)
            {
                if (allDim.Contains(r.Dimension))
                    continue;
                allDim.Add(r.Dimension);
            }
            return allDim;
        }

        public static string GroupFactTable(string factSql, Report report)
        {
            var allDim = AllDimensions(report);
            var sql = "select\n\t";
            for (var i = 0; i < allDim.Count; i++)
            {
                if (i == allDim.Count - 1)
                    sql = sql + "factTable."+ allDim[i].PrimaryKey + "\n";
                else
                    sql = sql + "factTable." + allDim[i].PrimaryKey + ",\n\t";
            }
            sql = sql + "from(\n" + factSql + ") as factTable\n"; 
            sql = sql + "group by ";
            for (var i = 0; i < allDim.Count; i++)
            {
                if (i == allDim.Count - 1)
                    sql = sql + "factTable." + allDim[i].PrimaryKey + "\n";
                else
                    sql = sql + "factTable." + allDim[i].PrimaryKey + ", ";
            }
            
            return sql;
        }

        public static string SelectFromFactTable(Report report)
        {
            var allDim = AllDimensions(report);
            var sql = allDim.Aggregate("select\n\t", (current, d) => current + "factTable." + d.PrimaryKey + ",\n\t");
            sql = sql + "count(1) Measure\n";
            sql = sql + "from factTable\n";
            sql = sql + "group by ";
            for (var i = 0; i < allDim.Count; i++)
            {
                if (i == allDim.Count - 1)
                    sql = sql + "factTable." + allDim[i].PrimaryKey + "\n";
                else
                    sql = sql + "factTable." + allDim[i].PrimaryKey + ", ";
            }

            return sql;
        }

        public static string JoinFactTableWithDimensions(string factSql, Report report)
        {
            var allDim = AllDimensions(report);
            var sql = allDim.Aggregate("select \n\t", (current, d) => current + d.ColName + ", ");
            sql = sql + "Measure";
            sql = sql + "\nfrom (\n" + factSql + ") as groupedFactable\n";

            return allDim.Aggregate(sql, (current, dim) => current + "\nleft join " + dim.Table + " on " + dim.Table + "." + dim.PrimaryKey + " = " + "groupedFactable." + dim.PrimaryKey);
        }

    }
}
