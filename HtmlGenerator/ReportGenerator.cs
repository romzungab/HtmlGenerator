using System.Diagnostics;
using System.IO;

namespace HtmlGenerator
{
    class ReportGenerator
    {
        public static void Main(string[] args)
        {
            //dimensions
            var dClassification = new Dimension
            {
                Table = "dClassification",
                PrimaryKey = "ObjectId",
            };

            var dActivity = new Dimension
            {
                Table = "dActivity",
                PrimaryKey = "GlobalId",
            };

            var dUser = new Dimension
            {
                Table = "dUser",
                PrimaryKey = "ResourceId",
            };

            //dClassification columns
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

            //dActivity columns
            var cATitle = new Column
            {
                Dimension = dActivity,
                Name = "Title",
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

           //dUser columns
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

            //fact table
            var fActivity = new FactTable
            {
                Table = "fActivity",
                Dimensions = new Dimension[] { dActivity, dUser, dClassification },
                Columns = new[] { "GlobalId", "ObjectId", "ResourceId", "UTCStart", "UTCFinish", "HostName"}
            };

            //fActivity columns

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
                Name = "Duration",
                Expression = "datediff(s, fActivity.UTCStart, fActivity.UTCFinish)/ 60.0"
            };

            var cfDay = new Column
            {
                Dimension = fActivity,
                Name = "Day",
            };

            var cfDate = new Column
            {
                Dimension = fActivity,
                Name = "Date",
                Expression = "cast(fActivity.UTCStart) as date)"
            };

            var cfWeek = new Column
            {
                Dimension = fActivity,
                Name = "Week",
                Expression = "dateadd(dd, -((@@datefirst - 2 + datepart(dw, fActivity.UTCStart)) % 7), cast(fActivity.UTCStart as date))",
            };
            var cfMonth = new Column
            {
                Dimension = fActivity,
                Name = "Month",
                Expression = "dateadd(dd, -((@@datefirst - 2 + datepart(dw, fActivity.UTCStart)) % 7), cast(fActivity.UTCStart as date))",
            };

            //reports
            var timesheetReport = new Report
            {
                FactTable = fActivity,
                Columns = new[]
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
                },
               
            };

            var activityListReport = new Report
            {
                FactTable = fActivity,
                Columns = new[]
                {
                    new Grouping()
                    {
                        Column = cfUtcStart,
                        Group = false
                    },
                    new Grouping()
                    {
                        Column = cFullName,
                        Group = false
                    },
                    new Grouping()
                    {
                        Column = cType,
                        Group = false,
                    },
                    new Grouping()
                    {
                        Column = cDescription,
                        Group = false
                    },
                    new Grouping()
                    {
                        Column = cFolder,
                        Group = false
                    }
                }
            };

            var adHocBillable = new Report
            {
                FactTable = fActivity,
                Columns = new[]
                {
                    new Grouping()
                    {
                        Column = cfUtcStart,
                        Group = false
                    },
                    new Grouping()
                    {
                        Column = cFolder,
                        Group = false
                    },
                    new Grouping()
                    {
                        Column = cFullName,
                        Group = false
                    },
                    new Grouping()
                    {
                        Column = cCTitle,
                        Group = false
                    },
                    new Grouping()
                    {
                        Column = cfMinutes,
                        Group = false
                    },

                    new Grouping()
                    {
                        Column = cFolder,
                        Group = false
                    },
                },
                Rows = new[]
                {
                    new Grouping()
                    {
                        Column = cFolder,
                        Group = true
                    }
                }
            };

            var topicAllocation = new Report()
            {
                FactTable = fActivity,
                Columns = new[]
                {
                    new Grouping()
                    {
                        Column = cFolder,
                        Group = false
                    },
                    new Grouping()
                    {
                        Column = cCTitle,
                        Group = false
                    },
                    new Grouping()
                    {
                        Column = cType,
                        Group = false
                    },
                    new Grouping()
                    {
                        Column = cFullName,
                        Group = false
                    },
                    new Grouping()
                    {
                        Column = cfDuration,
                        Group = false
                    }
                },
            };

            var classificationAllocation = new Report()
            {
                FactTable = fActivity,
                Columns = new[]
                {
                    new Grouping()
                    {
                        Column = cFolder,
                        Group = false
                    },
                    new Grouping()
                    {
                        Column = cCTitle,
                        Group = false
                    },
                    new Grouping()
                    {
                        Column = cActivityType,
                        Group = false
                    },
                    new Grouping()
                    {
                        Column = cFullName,
                        Group = false
                    },
                    new Grouping()
                    {
                        Column = cfDuration,
                        Group = false
                    },

                }
            };

            var applicationSummary = new Report()
            {
                FactTable = fActivity,
                Columns = new[]
                {
                    new Grouping()
                    {
                        Column = cFullName,
                        Group = false
                    },

                    new Grouping()
                    {
                        Column = cDepartment,
                        Group = false
                    },
                    new Grouping()
                    {
                        Column = cFolder,
                        Group = false
                    },
                    new Grouping()
                    {
                        Column = cATitle,
                        Group = false
                    },
                    new Grouping()
                    {
                        Column = cfDuration,
                        Group = false
                    },
                }
            };

            var weeklyTimesheet = new Report
            {
                FactTable = fActivity,
                Columns = new[]
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
                        Column = cfDuration,
                        Group = false
                    },
                    new Grouping()
                    {
                        Column = cfDay,
                        Group = true
                    },
                },
                Rows = new[]
                {
                    new Grouping()
                    {
                        Column = cFullName,
                        Group = true
                    },
                    new Grouping()
                    {
                        Column = cfWeek,
                        Group = true
                    },
                    new Grouping()
                    {
                        Column = cfObjectId,
                        Group = true
                    },
                    new Grouping()
                    {
                        Column = cCTitle,
                        Group = true
                    },
                },
            };
            CreateSQLFile(timesheetReport, "TimesheetReport");
            CreateSQLFile(activityListReport, "ActivityListReport");
            CreateSQLFile(topicAllocation, "TopicAllocationReport");
            CreateSQLFile(classificationAllocation, "ClassificationAllocationReport");
            CreateSQLFile(applicationSummary, "ApplicationSummary"); //sum and count --measure
            CreateSQLFile(adHocBillable, "AdHocBillable"); //pivot days
            CreateSQLFile(weeklyTimesheet, "WeeklyTimesheet"); //sum for total hours, pivot days

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










