using System.Diagnostics;
using System.IO;

namespace HtmlGenerator
{
    class ReportGenerator
    {
        public static void Main(string[] args)
        {

            var dClassification = new Dimension
            {
                Table = "dClassification",
                PrimaryKey = "ObjectId",
            };

            var cObjectId = new Column
            {
                Dimension = dClassification,
                Name = "ObjectId",
            };
            var cCTitle = new Column
            {
                Dimension = dClassification,
                Name = "Title",
            };
            var cReferenceNumber = new Column
            {
                Dimension = dClassification,
                Name = "ReferenceNumber",
            };
            var cType = new Column
            {
                Dimension = dClassification,
                Name = "Type",
            };
            var cIsPersonal = new Column
            {
                Dimension = dClassification,
                Name = "IsPersonal",
            };
            var cIsPrivate = new Column
            {
                Dimension = dClassification,
                Name = "IsPrivate",
            };
            var cIsArchived = new Column
            {
                Dimension = dClassification,
                Name = "IsArchived",
            };
            var cFolder = new Column
            {
                Dimension = dClassification,
                Name = "Folder",
            };

            var dActivity = new Dimension
            {
                Table = "dActivity",
                PrimaryKey = "GlobalId",
            };

            var cGlobalId = new Column
            {
                Dimension = dActivity,
                Name = "GlobalId",
            };

            var cActivityType = new Column
            {
                Dimension = dActivity,
                Name = "ActivityType",
            };

            var cDescription = new Column
            {
                Dimension = dActivity,
                Name = "Description",
            };

            var dUser = new Dimension
            {
                Table = "dUser",
                PrimaryKey = "ResourceId",
            };

            var cResourceId = new Column
            {
                Dimension = dUser,
                Name = "ResourceId"
            };

            var cFullName = new Column
            {
                Dimension = dUser,
                Name = "FullName"
            };
            var cDepartment = new Column
            {
                Dimension = dUser,
                Name = "Department"
            };

            var fActivity = new FactTable
            {
                Table = "fActivity",
                Dimensions = new Dimension[] { dActivity, dUser, dClassification },
                Columns = new[] { "GlobalId", "ObjectId", "ResourceId", "UTCStart", "UTCFinish", "HostName", "Minutes", "Duration", "Week","Day" }
            };

            var cfGlobalId = new Column
            {
                Dimension = fActivity,
                Name = "GlobalId"
            };

            var cfObjectId = new Column
            {
                Dimension = fActivity,
                Name = "ObjectId"
            };

            var cfUtcStart = new Column
            {
                Dimension = fActivity,
                Name = "UTCStart"
            };
            var cfUtcFinish = new Column
            {
                Dimension = fActivity,
                Name = "UTCFinish"
            };
            var cfHostName = new Column
            {
                Dimension = fActivity,
                Name = "HostName"
            };
            var cfMinutes = new Column
            {
                Dimension = fActivity,
                Name = "Minutes"
            };

            var cfDuration = new Column
            {
                Dimension = fActivity,
                Name = "Duration"
            };
            var timesheetReport = new Report
            {
                FactTable = fActivity,
                Data = new[]
                {
                    new Grouping()
                    {
                        Column = cfUtcStart,
                        Group = false
                    },
                    new Grouping()
                    {
                        Column = cfUtcFinish,
                        Group = false
                    },
                    new Grouping()
                    {
                        Column = cFullName,
                        Group = false
                    },
                    new Grouping
                    {
                        Column = cActivityType,
                        Group = true
                    },
                    new Grouping
                    {
                        Column = cType,
                        Group = true
                    },
                },
                Rows = new[]
                {
                    new Grouping()
                    {
                        Column = cDepartment,
                        Group = true
                    },
                    new Grouping()
                    {
                        Column = cfHostName,
                        Group = true
                    }
                }
            };

            var activityListReport = new Report
            {
                FactTable = fActivity,
                Data = new[] {
                        new Grouping(){
                            Column = cfUtcStart,
                            Group = false
                        },
                        new Grouping(){
                            Column = cFullName,
                            Group = false
                        },
                        new Grouping(){
                            Column = cType,
                            Group = false,
                        } ,
                        new Grouping(){
                            Column = cDescription,
                            Group =false
                        },
                        new Grouping(){
                            Column = cFolder,
                            Group = false
                        }
                }
            };

            var adHocBillable = new Report
            {
                FactTable = fActivity,
                Data = new[] {
                     new Grouping(){
                        Column = cfUtcStart,
                        Group = false
                    },
                    new Grouping(){
                        Column = cFolder,
                        Group = false
                    },
                    new Grouping(){
                        Column = cFullName,
                        Group = false
                    },
                    new Grouping(){
                        Column  = cCTitle,
                        Group = false
                    },
                    new Grouping(){
                        Column = cfMinutes,
                        Group = false
                    },

                    new Grouping(){
                        Column = cFolder,
                        Group = false
                    },
                },
                Rows = new[] {
                     new Grouping(){
                         Column = cFolder,
                         Group = true
                     }
                 }
            };

            var topicAllocation = new Report()
            {
                FactTable = fActivity,
                Data = new[] {
                    new Grouping(){
                        Column = cFolder,
                        Group = false
                    },
                    new Grouping(){
                         Column = cCTitle,
                         Group = false
                    },
                    new Grouping(){
                        Column = cType,
                        Group = false
                    },
                    new Grouping(){
                        Column = cFullName,
                        Group = false
                    },
                    new Grouping() {
                        Column = cfDuration,
                        Group = false
                    }
                },
            };

            var classificationAllocation = new Report()
            {
                FactTable = fActivity,
                Data = new[] {
                    new Grouping(){
                        Column = cFolder,
                        Group = false
                    },
                      new Grouping(){
                        Column = cCTitle,
                        Group = false
                    },
                        new Grouping(){
                        Column = cActivityType,
                        Group = false
                    },
                         new Grouping(){
                        Column = cFullName,
                        Group = false
                    },
                         new Grouping(){
                        Column = cfDuration,
                        Group = false
                    },

                }
            };

            CreateSQLFile(timesheetReport, "TimesheetReport");
            CreateSQLFile(activityListReport, "ActivityListReport");
            CreateSQLFile(topicAllocation, "TopicAllocationReport");
            CreateSQLFile(classificationAllocation, "ClassificationAllocationReport");

            // CreateSQLFile(weeklyTimesheet, "weeklyTimesheet");
            //CreateSQLFile(timesheet, "timesheet");
            // CreateSQLFile(classificationAllocation, "classificationAllocation");
            //CreateSQLFile(topicAllocation, "topicAllocation");


           // CreateReportFile(weeklyTimesheet, "weeklyTimesheet");
            //CreateReportFile(timesheet, "timesheet");
            //CreateReportFile(classificationAllocation, "classificationAllocation");
            //CreateReportFile(topicAllocation, "topicAllocation");
            //CreateReportFile(activityList, "activityList");
        }

        //private static void CreateReportFile(Report report, string reportName)
        //{
        //    var html = TableBuilder.BuildHtml(report);
        //    var reportFilename = @"C:\Users\romelyn.ungab\Documents\reports\" + reportName + ".html";
        //    File.WriteAllText(reportFilename, html);
        //    Process.Start(reportFilename);
        //}

        private static void CreateSQLFile(Report report, string reportName)
        {
            var sql = SQLGenerator.BuildSQL(report);
            var reportFilename = @"C:\Users\romelyn.ungab\Documents\sql\" + reportName + ".sql";
            File.WriteAllText(reportFilename, sql);
            Process.Start(reportFilename);
        }
    }
}