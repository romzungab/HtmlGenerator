using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace HtmlGenerator
{
    class ReportGenerator
    {
        public static void Main(string[] args)
        {
            var possibleDimensions = new[]
            {
                new Dimension
                {
                    Name = "Classification.Type",
                    Values = new[] { "Task", "Topic", "Project" },
                },
                new Dimension
                {
                    Name = "Activity.Application",
                    Values = new[] { "MS Word", "Visual Studio", "World of Warcraft", "Chrome" },
                }
            };

            var grouping = new[]
            {
                new Grouping
                {
                    Dimension = possibleDimensions[0],
                    Group = false,
                },

                new Grouping
                {
                    Dimension = possibleDimensions[1],
                    Group = false,
                },
            };

            var report = new Report
            {
                Columns = grouping
            };

            var html = TableBuilder.BuildHtml(report);
            var fileName = @"C:\Users\romelyn.ungab\Documents\report.html";
            File.WriteAllText(fileName, html);
            Process.Start(fileName);
        }
    }
}
