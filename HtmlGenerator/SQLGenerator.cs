using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HtmlGenerator
{
    public static class SQLGenerator
    {
        public static string BuildSQL(Report report)
        {

            var factTable = BuildFactTableActivity(report);
            factTable = GroupFactTable(factTable, report);
            return JoinFactTableWithDimensions(factTable, report);

           // return BuildFactTableActivity(report);

        }

        public static string BuildFactTableActivity(Report report)
        {
            var allDim = AllDimensions(report);

            var sql = "select\n\t";
            for (var i = 0; i < allDim.Count; i++)
            {
                if (i == allDim.Count - 1)
                    sql = sql + allDim[i].Table+ "."+allDim[i].PrimaryKey +" as " + allDim[i].PrimaryKey +"\n";
                else
                    sql = sql + allDim[i].Table + "." + allDim[i].PrimaryKey + " as " + allDim[i].PrimaryKey + ",\n\t";
            }
            sql = sql + "from "+report.BaseTable+"\n";

            return allDim.Aggregate(sql, (current, r) => current + " inner join " + r.Table + " on " + r.Table + "." + r.PrimaryKey + " = "+report.BaseTable+"." + r.PrimaryKey + "\n");
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
                    sql = sql + "factTable." + allDim[i].PrimaryKey + ",";
            }
            
            return sql;
        }

        public static string JoinFactTableWithDimensions(string factSql, Report report)
        {
            var sql = "select \n\t";
            var allDim = AllDimensions(report);
            for (var i = 0; i < allDim.Count; i++)
            {
                if (i == allDim.Count - 1)
                    sql = sql + allDim[i].ColName;
                else sql = sql + allDim[i].ColName + ", ";
            }
            sql = sql + "\nfrom (" + factSql + ") as groupFactTable\n";

            return allDim.Aggregate(sql, (current, dim) => current + "\ninner join " + dim.Table + " on " + dim.Table + "." + dim.PrimaryKey + " = " + "groupFactTable." + dim.PrimaryKey);
        }

    }
}
