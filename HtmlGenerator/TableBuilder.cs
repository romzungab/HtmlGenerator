using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

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
            var ungroup = report.Columns.Where(c => c.Group == false);
            var ungroups = ungroup.Select(g => g.Dimension.Values).ToList();

            var rgroup = report.Rows.Where(r => r.Group);
            var rgroups = rgroup.Select(g => g.Dimension.Values).ToList();
            var finalGrouping = rgroups.Concat(ungroups).ToList();
            
            var grouped = false;
            table.Append("<tr>\n");
            foreach (var row in report.Rows)
            {
                
                    table.Append("<th>\n");
                    table.Append(row.Dimension.Name);
                    table.Append("\n</th>\n");
                    colCount++;
                
            }
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
                    table.Append(col.Dimension.Name);
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
            
            if (ungroups.Any())
            {
                foreach (var line in Combinations(finalGrouping))
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
                }
            }
            else
            {
                for (var rowIndex = 0; rowIndex < ungroups[0].Length; rowIndex++)
                {
                    var numCols = colCount;
                    table.Append("<tr>\n");
                    foreach (var col in report.Columns)
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
            }

            table.Append("</tr>\n");

            table.Append("</table>");
            return table.ToString();
        }

        private static IEnumerable<T[]> Combinations<T>(IReadOnlyList<IReadOnlyList<T>> lists)
        {
            var result = new int[lists.Count];
            for (;;)
            {
                yield return result.Select((n, i) => lists[i][n]).ToArray();

                for (var i = 0; i < lists.Count; i++)
                {
                    result[i]++;
                    if (result[i] < lists[i].Count) break;
                    result[i] = 0;

                    if (i == lists.Count - 1) yield break;
                }
            }
        }
    }
}
