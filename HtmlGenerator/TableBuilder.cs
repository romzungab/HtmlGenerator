using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HtmlGenerator
{
    public class TableBuilder
    {
        public static string BuildHtml(Report report)
        {
            var table = new StringBuilder(@"
<style>
table {
   border-collapse: collapse;
}

td, th {
   padding: .25em .5em;
}

th {
   background-color: #eee;
}
</style>
<table border>");
            var colCount = 0;

            var ungroup = report.Data.Where(c => c.Group == false);
            var ungroups = ungroup.Select(g => g.Column).ToList();

            var group = report.Columns.Where(c => c.Group);
            
            var rgroup = report.Rows.Where(r => r.Group);
            var rgroups = rgroup.Select(g => g.Dimension.Values).ToList();
            var grouped = false;

            table.Append("<tr>\n");
            if (group.Count() < report.Columns.Length)
            {
                foreach (var col in report.Columns)
                {
                    if (col.Group)
                    {
                        grouped = true;
                        foreach (var dval in col.Dimension.Values)
                        {
                            table.Append("<th>\n");
                            table.Append(dval);
                            table.Append("\n</th>\n");
                            colCount++;
                        }
                    }
                    else
                    {
                        table.Append("<th>\n");
                        table.Append(col.Dimension.ColName);
                        table.Append("\n</th>\n");
                        colCount++;
                    }
                }
                if (!grouped)
                {
                    table.Append("<th>\n");
                    table.Append("Measure");
                    table.Append("\n</th>\n");
                    colCount++;
                }

                table.Append("</tr>\n");

                if (rgroups.Count > 0)
                    table.Append(GetRowGroupingCombi(report, rgroups, colCount, ungroups));
                else
                    GenerateColumns(report, colCount, ungroups, table);
                
            }
            else
            {
                table.Append("<th>\n");
                table.Append("Measure");
                table.Append("\n</th>\n");
                table.Append("<tr>\n");
                table.Append("<td>\n");
                table.Append("1");
                table.Append("\n</td>\n");
                table.Append("</tr>\n");
            }

            table.Append("</tr>\n");

            table.Append("</table>");
            return table.ToString();
        }

        private static string GetRowGroupingCombi(Report report, List<string[]> rgroups, int colCount, List<string[]> ungroups)
        {
            var table = new StringBuilder();
            if (rgroups.Count <= 0) return table.ToString();
            if (rgroups.Count() == 1)
                table.Append(CombineDataWithRow(report, rgroups, colCount, ungroups));
            else
            {
                foreach (var r in rgroups.ElementAt(0))
                {
                    table.Append("<tr>\n");
                    table.Append("<th align='left' colspan=" + colCount + ">\n");
                    table.Append(r);
                    table.Append("\n</th>\n");
                    table.Append("</tr>\n");

                    var rg = new List<string[]>(rgroups);
                    if (!rg.Any()) continue;
                    rg.RemoveAt(0);
                    table.Append(GetRowGroupingCombi(report, rg, colCount, ungroups));
                }
            }
            return table.ToString();
        }

        private static string CombineDataWithRow(Report report, List<string[]> rgroups, int colCount, List<string[]> ungroups)
        {
            var table = new StringBuilder();
            foreach (var ts in rgroups)
            {
                foreach (var t in ts)
                {
                    table.Append("<tr>\n");
                    table.Append("<th align='left' colspan=" + colCount + ">\n");
                    table.Append(t);
                    table.Append("\n</th>\n");
                    table.Append("</tr>\n");

                    GenerateColumns(report, colCount, ungroups, table);
                }
            }
            return table.ToString();
        }

        private static void GenerateColumns(Report report, int colCount, List<string[]> ungroups, StringBuilder table)
        {
            if (ungroups.Any())
            {
                foreach (var line in Combinations(ungroups))
                {
                    GenerateDataWithMoreThanOneColumnUnGrouped(colCount, table, line);
                }
            }
            else
            {
                for (var rowIndex = 0; rowIndex < ungroups[0].Length; rowIndex++)
                {
                    GenerateDataWithOneColumnUngroup(report, colCount, table, rowIndex);
                }
            }
        }

        private static void GenerateDataWithOneColumnUngroup(Report report, int colCount, StringBuilder table, int rowIndex)
        {
            var numCols = colCount;
            table.Append("<tr>\n");
            foreach (var col in report.Data)
            {
                if (!col.Group)
                {
                    table.Append("<td>\n");
                    table.Append(col.Dimension.Values.Length > rowIndex ? col.Dimension.Values[rowIndex] : "");
                    table.Append("\n</td>\n");
                }
                else
                {
                    for (var colIndex = 0; colIndex < numCols; colIndex++)
                    {
                        table.Append("<td>\n");
                        table.Append("1");
                        table.Append("\n</td>\n");
                    }
                }
            }
        }

        private static void GenerateDataWithMoreThanOneColumnUnGrouped(int colCount, StringBuilder table, string[] line)
        {
            var numCols = colCount;
            var ct = 0;
            table.Append("<tr>\n");
            foreach (var c in line)
            {
                table.Append("<td>\n");
                table.Append(c);
                table.Append("\n</td>\n");
                ct++;
            }

            for (var colIndex = 0; colIndex < numCols; colIndex++)
            {
                table.Append("<td>\n");
                table.Append("1");
                table.Append("\n</td>\n");
                ct++;
                if (ct == numCols)
                    break;
            }
            table.Append("</tr>\n");
        }
        
        private static IEnumerable<T[]> Combinations<T>(IReadOnlyList<IReadOnlyList<T>> lists)
        {
            var result = new int[lists.Count];
            for (;;)
            {
                yield return result.Select((n, i) => lists[i][n]).ToArray(); //returns the result of the Select or any expression one at a time; expression is returned and current location in code is retain

                for (var i = 0; i < lists.Count; i++)
                {
                    result[i]++;
                    if (result[i] < lists[i].Count) break;
                    result[i] = 0;

                    if (i == lists.Count - 1) yield break;//ends  the iteration
                }
            }
        }
    }
}
