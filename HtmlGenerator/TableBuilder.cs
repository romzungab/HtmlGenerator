using System;
using System.Collections.Generic;
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
            var table = new StringBuilder("<table borderColor = #000000 cellSpacing=1 cellPadding=1 border=1>\n");
            var colCount = 0;
            table.Append("<tr>\n");
            foreach (var col in report.Columns)
            {
                if (col.Group)
                {
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

            table.Append("</tr>\n");

            var ungroup = report.Columns.Where(c => c.Group == false);
            var rowCount = ungroup.Select(g => g.Dimension.Values.Length).Concat(new[] { 0 }).Max();

            for (var rowIndex = 0; rowIndex < rowCount; rowIndex++)
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
                        numCols--;
                    }
                    else
                    {
                        for (var colIndex = 0; colIndex < numCols; colIndex++)
                        {
                            table.Append("<td>\n");
                            table.Append("1");
                            table.Append("\n</td>\n");
                        }
                        break;
                    }

                }
                table.Append("</tr>\n");
            }
            table.Append("</table>");
            return table.ToString();
        }
    }
}
