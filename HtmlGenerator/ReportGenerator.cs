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

            var classificationKind = new Dimension
            {
                Name = "Classification.Kind",
                Values = new[] { "Business", "Leave", "Unclassified", "Break" },
            };

            var day = new Dimension
            {
                Name = "Day",
                Values = new[] { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday" },
            };

            var resource = new Dimension
            {
                Name = "Resource",
                Values = new[] { "Roms", "Vic", "Hope" },
            };

            var department = new Dimension
            {
                Name = "Department",
                Values = new[] { "Special Project", "Development", "Support" }
            };

            var classificationFolder = new Dimension
            {
                Name = "Classification.Folder",
                Values = new[] { "Unclassified", "Public", "Business", "AdHoc" },
            };

            var weeklyTimesheetGrouping = new[]
            {
                new Grouping()
                {
                    Dimension = day,
                    Group = false,
                },
                new Grouping
                {
                    Dimension = classificationKind,
                    Group = true,
                },
            };

            var weeklyTimesheetRowGrouping = new[]
            {
                new Grouping
                {
                    Dimension = department,
                    Group = true,
                },
                new Grouping
                {
                    Dimension = resource,
                    Group = true,
                },
            };

            var weeklyTimesheet = new Report
            {
                Columns = weeklyTimesheetGrouping,
                Rows = weeklyTimesheetRowGrouping
            };

            var classificationAllocationColumnGrouping = new[]
            {
                new Grouping
                {
                    Dimension = classificationFolder,
                    Group = false,
                },
                new Grouping
                {
                    Dimension = classificationType,
                    Group = false,
                },
                new Grouping
                {
                    Dimension = classificationKind,
                    Group = false,
                },

            };

            var classificationAllocationRowGrouping = new[]
            {
                new Grouping
                {
                  Dimension = department,
                  Group = true
                },
                new Grouping
                {
                    Dimension = resource,
                    Group = true
                },
            };

            var classificationAllocation = new Report
            {
                Columns = classificationAllocationColumnGrouping,
                Rows = classificationAllocationRowGrouping
            };

            var topicAllocationColumnGrouping = new[]
            {
                new Grouping
                {
                  Dimension = classificationFolder,
                  Group = false,
                },
                new Grouping
                {
                    Dimension = classificationKind,
                    Group = false,
                },
                new Grouping
                {
                    Dimension = resource,
                    Group = false,
                },
            };
            var topicAllocationRowGrouping = new[]
            {
                new Grouping
                {
                    Dimension = classificationFolder,
                    Group = false,
                },
            };
            var topicAllocation = new Report
            {
                Columns = topicAllocationColumnGrouping,
                Rows = topicAllocationRowGrouping
            };

            var activityListColumnGrouping = new[]
            {
                new Grouping
                {
                    Dimension = resource,
                    Group = false,
                },
               new Grouping
                {
                    Dimension =   classificationFolder,
                    Group = false,
                },
               new Grouping
                {
                Dimension = activityApplication,
                Group = false,
                },
            };

            var activityListRowGrouping = new[]
            {
                new Grouping
                {
                  Dimension = classificationFolder,
                  Group = true,
                },
              
                new Grouping
                {
                    Dimension = resource,
                    Group = true,
                }
            };

            var activityList = new Report
            {
                Columns = activityListColumnGrouping
             //   Rows = activityListRowGrouping,
            };

            //var weeklyTimesheethtml = TableBuilder.BuildHtml(weeklyTimesheet);
            //const string weeklyTimesheetFileName = @"C:\Users\romelyn.ungab\Documents\weeklyTimesheetReport.html";
            //File.WriteAllText(weeklyTimesheetFileName, weeklyTimesheethtml);
            //Process.Start(weeklyTimesheetFileName);

            //var classificationAllocationthtml = TableBuilder.BuildHtml(classificationAllocation);
            //const string classificationAllocationFileName = @"C:\Users\romelyn.ungab\Documents\classificationAllocationReport.html";
            //File.WriteAllText(classificationAllocationFileName, classificationAllocationthtml);
            //Process.Start(classificationAllocationFileName);

            //var topicAllocationHtml = TableBuilder.BuildHtml(topicAllocation);
            //const string topicAllocationFileName = @"C:\Users\romelyn.ungab\Documents\topicAllocationReport.html";
            //File.WriteAllText(topicAllocationFileName, topicAllocationHtml);
            //Process.Start(topicAllocationFileName);

            var activityListHtml = TableBuilder.BuildHtml(activityList);
            const string activityListFileName = @"C:\Users\romelyn.ungab\Documents\topicAllocationReport.html";
            File.WriteAllText(activityListFileName, activityListHtml);
            Process.Start(activityListFileName);
        }
    }
}
