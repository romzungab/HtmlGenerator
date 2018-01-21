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
                Table = "dClassification",
                PrimaryKey = "ObjectId",
                ColName = "Type",
                Values = new[] { "Task", "Topic", "Project" },
            };

           var classificationKind = new Dimension
            {
                Table = "dClassification",
                PrimaryKey = "ObjectId",
                ColName = "Kind",
                Values = new[] { "Business", "Leave", "Unclassified", "Break" },
            };
            var classificationFolder = new Dimension
            {
                Table = "dClassification",
                PrimaryKey = "ObjectId",
                ColName = "Folder",
                Values = new[] { "Unclassified", "Public", "Business", "AdHoc" },
            };

            var day = new Dimension
            {
                Table = "dDate",
                PrimaryKey = "DateId",
                ColName = "Day",
                Values = new[] { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday" },
            };

            var activityApplication = new Dimension
            {
                Table = "dActivity",
                PrimaryKey = "GlobalId",
                ColName = "Application",
                Values = new[] { "MS Word", "Visual Studio", "World of Warcraft", "Chrome" },
            };

            var resource = new Dimension
            {
                Table ="dResource",
                PrimaryKey = "ResourceId",
                ColName = "Fullname",
                Values = new[] { "Roms", "Vic", "Hope" },
            };

            var department = new Dimension
            {
                Table = "dResource",
                PrimaryKey = "ResourceId",
                ColName = "Department",
                Values = new[] { "Special Project", "Development", "Support" }
            };

            var date = new Dimension
            {
                Table = "dDate",
                PrimaryKey = "DateId",
                ColName = "Date",
                Values = new[] { "1 Nov 2017", "02 Nov 2017", "03 Nov 2017" },
            };

            var month = new Dimension
            {
                Table = "dDate",
                PrimaryKey = "DateId",
                ColName = "Month",
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
                    Group = false,
                },
                new Grouping
                {
                    Dimension = resource,
                    Group = false,
                },
                new Grouping()
                {
                    Dimension = date,
                    Group = false,
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

            var sampleReportColumnGrouping = new []
            {
                new Grouping
                {
                    Dimension = resource,
                    Group = false,
                },
                new Grouping
                {
                    Dimension = classificationFolder,
                    Group = false,
                },
                new Grouping
                {
                    Dimension = classificationKind,
                    Group = false,
                }
            };

            var sampleReport = new Report
            {
                BaseTable = "fTable",
                Columns = sampleReportColumnGrouping,
            };

         //   CreateSQLFile(sampleReport, "SampleReport");
            CreateSQLFile(weeklyTimesheet, "weeklyTimesheet");
            //CreateSQLFile(timesheet, "timesheet");
           // CreateSQLFile(classificationAllocation, "classificationAllocation");
            //CreateSQLFile(topicAllocation, "topicAllocation");
           // CreateSQLFile(activityList, "activityList");

            //CreateReportFile(weeklyTimesheet, "weeklyTimesheet");
            //CreateReportFile(timesheet, "timesheet");
            //CreateReportFile(classificationAllocation, "classificationAllocation");
            //CreateReportFile(topicAllocation, "topicAllocation");
            //CreateReportFile(activityList, "activityList");
        }

        private static void CreateReportFile(Report report, string reportName)
        {
           var html = TableBuilder.BuildHtml(report);
            var reportFilename = @"C:\Users\romelyn.ungab\Documents\reports\" + reportName + ".html";
            File.WriteAllText(reportFilename, html);
            Process.Start(reportFilename);
        }

        private static void CreateSQLFile(Report report, string reportName)
        {
            var sql = SQLGenerator.BuildSQL(report);
            var reportFilename = @"C:\Users\romelyn.ungab\Documents\sql\" + reportName + ".sql";
            File.WriteAllText(reportFilename, sql);
            Process.Start(reportFilename);
        }
    }
}
