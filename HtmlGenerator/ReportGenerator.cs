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
            var classificationType = new Dimension
            {
                Name = "Classification.Type",
                Values = new[] { "Task", "Topic", "Project" },
            };

            var activityApplication = new Dimension
            {
                Name = "Activity.Application",
                Values = new[] { "MS Word", "Visual Studio", "World of Warcraft", "Chrome" },
            };

            var columnGrouping = new[]
            {
                new Grouping
                {
                    Dimension = classificationType,
                    Group = false,
                },

                new Grouping
                {
                    Dimension = activityApplication,
                    Group = false,
                },
                new Grouping
                {
                    Dimension = activityApplication,
                    Group = false,
                },
                new Grouping
                {
                    Dimension = activityApplication,
                    Group = false,
                },

            };

            var rowGrouping = new[]
            {
                new Grouping
                {
                    Dimension = classificationType,
                    Group = true,
                }
            };

            var report = new Report
            {
                Columns = columnGrouping,
                Rows = rowGrouping
            };

            var html = TableBuilder.BuildHtml(report);
            var fileName = @"C:\Users\romelyn.ungab\Documents\report.html";
            File.WriteAllText(fileName, html);
            Process.Start(fileName);
        }
    }
}
