using System.Diagnostics;
using System.IO;

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

            var date = new Dimension
            {
                Name = "Date",
                Values = new[] { "1 Nov 2017", "02 Nov 2017", "03 Nov 2017" },
            };

            var month = new Dimension
            {
                Name = "Month",
                Values = new[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sept", "Oct", "Nov", "Dec" },
            };
            var weeklyTimesheetGrouping = new[]
            {
              new Grouping
                {
                    Dimension = classificationKind,
                    Group = false,
                },
                new Grouping()
                {
                    Dimension = day,
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
                new Grouping()
                {
                    Dimension = date,
                    Group = true,
                },
            };

            var weeklyTimesheet = new Report
            {
                Columns = weeklyTimesheetGrouping,
                Rows = weeklyTimesheetRowGrouping
            };

            var timesheetColumnGrouping = new[]
            {
              
               new Grouping()
                {
                    Dimension = date,
                    Group = false,
                },
                new Grouping
                {
                    Dimension = classificationKind,
                    Group = true,
                }
            };

            var timesheetRowGrouping = new[]
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

            var timesheet = new Report
            {
                Columns = timesheetColumnGrouping,
                Rows = timesheetRowGrouping,
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
                    Dimension = classificationType,
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
                    Dimension =   classificationFolder,
                    Group = false,
                },
               new Grouping
                {
                Dimension = activityApplication,
                Group = false,
                },
                new Grouping
                {
                    Dimension =   date,
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
                Columns = activityListColumnGrouping,
                Rows = activityListRowGrouping,
            };

            CreateFile(weeklyTimesheet, "weeklyTimesheet");
            CreateFile(timesheet, "timesheet");
            CreateFile(classificationAllocation, "classificationAllocation");
            CreateFile(topicAllocation, "topicAllocation");
            CreateFile(activityList, "activityList");
        }

        private static void CreateFile(Report report, string reportName)
        {
            var html = TableBuilder.BuildHtml(report);
            var reportFilename = @"C:\Users\romelyn.ungab\Documents\reports\" + reportName + ".html";
            File.WriteAllText(reportFilename, html);
            Process.Start(reportFilename);
        }
    }
}
