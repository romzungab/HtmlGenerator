using System.Diagnostics;
using System.IO;

namespace HtmlGenerator
{
    class ReportGenerator
    {
        public static void Main(string[] args)
        {
            //dClassification columns
            var cObjectId = new Column
            {
                Name = "ObjectId",
            };

            var cReferenceNumber = new Column
            {
                Name = "ReferenceNumber",
            };

            var cType = new Column
            {
                Name = "Type",
                Expression = "case"
                            + "\n\t\twhen dClassification.ObjectId is null then '[Unclassified]'"
                            + "\n\t\telse dClassification.Type"
                            + "\n\tend"
            };

            var cFolder = new Column
            {
                Name = "Folder",
            };

            var cParent = new Column
            {
                Name = "Parent",
            };

            var cIsPersonal = new Column
            {
                Name = "IsPersonal"
            };
            var cIsPrivate = new Column
            {
                Name = "IsPrivate"
            };
            var cIsArchived = new Column
            {
                Name = "IsArchived"
            };

            var cCTitle = new Column
            {
                Name = "Title",
                Expression = "case "
                             + "\n\t\twhen fActivity.ObjectId is null then '[Unclassified]'"
                             + "\n\t\twhen dClassification.IsArchived = 1 then '[Archived]'"
                            + "\n\t\telse dClassification.Title"
                            + "\n\tend",
                GroupBys = new[] { "fActivity.ObjectId,", "dClassification.IsArchived,", "dClassification.Title" }
            };

            var cClassification = new Column
            {
                Name = "Classification",
                Expression = "case "
                    + "\n\t\twhen dClassification.IsArchived = 1 then '[Archived]'"
                    + "\n\t\telse coalesce(dClassification.Parent, '-')"
                    + "\n\tend"
            };

            //dActivity columns
            var cATitle = new Column
            {
                Name = "Title",
            };
            var cGlobalId = new Column
            {
                Name = "GlobalId",
            };

            var cActivityType = new Column
            {
                Name = "ActivityType",
                Expression = "case"
                            + "\n\twhen dActivity.ActivityType in (3) then 'Untracked'"
                            + "\n\twhen dActivity.ActivityType in (4, 5) then 'Leave'"
                            + "\n\twhen dActivity.ActivityType in (6) then 'Break'"
                            + "\n\twhen dActivity.ActivityType in (1) then 'Idle'"
                            + "\n\twhen fActivity.ObjectId is null then 'Unclassified'"
                            + "\n\twhen dClassification.IsPersonal = 0 then 'Business'"
                            + "\n\telse 'Other'"
                            + "\n\tend ",
                GroupBys = new[] { "dActivity.ActivityType,", "dActivity.ObjectId,", "dClassification.IsPersonal" }
            };

            var cDescription = new Column
            {
                Name = "Description",
            };

            //dUser columns
            var cResourceId = new Column
            {
                Name = "ResourceId",
            };

            var cFullName = new Column
            {
                Name = "FullName",
            };
            var cUserName = new Column
            {
                Name = "UserName",
            };

            var cRegionId = new Column
            {
                Name = "RegionId",
            };
            var cDepartment = new Column
            {
                Name = "Department"
            };

            //dLeave Columns
            var cLeaveRequestId = new Column
            {
                Name = "LeaveRequestId"
            };

            var cLeaveTime = new Column
            {
                Name = "LeaveTime"
            };
            var cStatus = new Column
            {
                Name = "Status"
            };
            var cManagerNote = new Column
            {
                Name = "ManagerNote"
            };
            var cPayrollNote = new Column
            {
                Name = "PayrollNote"
            };
            var cLeaveType = new Column
            {
                Name = "LeaveType"
            };
            var cStartTime = new Column
            {
                Name = "StartTime"
            };
            var cFinishTime = new Column
            {
                Name = "LeaveTime"
            };

            //fActivity columns
                var cfUtcStart = new Column
            {
                Name = "UTCStart"
            };

            var cfUtcFinish = new Column
            {
                Name = "UTCFinish"
            };

            var cfHostName = new Column
            {
               Name = "HostName"
            };

            var cfMinutes = new Column
            {
              Name = "Minutes",
                Expression = "floor(ceiling((datediff(s, fActivity.UTCStart, fActivity.UTCFinish) / 60.0) + 30 - 1) / 30 * 30)"
            };

            var cfDuration = new Column
            {
               Name = "Duration",
                Expression = "datediff(ms, fActivity.UTCStart, fActivity.UTCFinish)/1000"
            };

            var cfDay = new Column
            {
                Name = "Day",
                Expression = "DATENAME(weekday,fActivity.UTCStart)",

            };

            var cfDate = new Column
            {
                Name = "Date",
                Expression = "cast(fActivity.UTCStart as date)"
            };
            var cfWeek = new Column
            {
                Name = "Week",
                Expression = "dateadd(dd, -((@@datefirst - 2 + datepart(dw, fActivity.UTCStart)) % 7), cast(fActivity.UTCStart as date))",
            };
            var cfMonth = new Column
            {
                Name = "Month",
                Expression = " dateadd(month, datediff(month, 0, fActivity.UTCStart), 0)",
            };
           
            var mActiveCount = new Measure
            {
                Aggregation = "sum",
                Expression = "fActivity.ActiveCount",
                Name = "ActiveCount",
            };
            var mAllocatedWork = new Measure
            {
                Name = "AllocatedWork",
            };
            var mActiveTime = new Measure
            {
                Name = "ActiveTime"
            };

            //dimensions
            var dClassification = new Dimension
            {
                Table = "dClassification",
                PrimaryKey = "ObjectId",
                Columns = new[] {cObjectId, cCTitle, cReferenceNumber, cType, cIsPersonal, cIsPrivate, cIsArchived, cFolder, cParent}
            };

            var dUser = new Dimension
            {
                Table = "dUser",
                PrimaryKey = "ResourceId",
                Columns = new[] {cResourceId, cFullName, cUserName, cRegionId, cDepartment}
            };

            var dActivity = new Dimension
            {
                Table = "dActivity",
                PrimaryKey = "GlobalId",
                Columns = new[] {cGlobalId, cActivityType,cATitle, cDescription}
            };

            var dLeave = new Dimension
            {
                Table = "dLeave",
                PrimaryKey = "LeaveRequestId",
                Columns = new[] {cLeaveRequestId, cResourceId, cLeaveTime, cReferenceNumber, cStatus, cManagerNote, cPayrollNote, cLeaveType, cStartTime, cFinishTime}
            };

            //fact tables
            var fActivity = new FactTable
            {
                Table = "fActivity",
                PrimaryKey = "GlobalId",
                Dimensions = new[] { dActivity, dUser, dClassification },
                Columns = new[] {cGlobalId, cObjectId, cResourceId, cfUtcStart, cfUtcFinish, cfHostName,cfDay,cfDate,cfWeek,cfMonth,cfMinutes}, 
                Measures = new[] { mActiveCount, }
            };

            var mLDuration  = new Measure()
            {
                Name = "Duration",
                Expression = "datediff(ms, fLeave.StartTime, fLeave.FinishTime) / 1000",
            };

            //fact table
            var fLeave = new FactTable
            {
                Table = "fLeave",
                Dimensions = new[] { dLeave, dUser },
                Measures = new[] {mLDuration }
            };

            
            //reports
            var timesheetReport = new Report
            {
                FactTable = fActivity,
                Columns = new[]
                {
                    new Grouping()
                    {
                        Source = fActivity,
                        Column = cfDate,
                    },
                    new Grouping()
                    {
                        Source = fActivity,
                        Column = cfUtcStart,
                    },
                    new Grouping()
                    {
                        Source = fActivity,
                        Column = cfUtcFinish,
                    },
                    new Grouping()
                    {
                        Source = fActivity,
                        Column = cfDuration,
                    },
                    new Grouping()
                    {
                        Source = dUser,
                        Column = cFullName,
                    },
                    new Grouping
                    {
                        Source = dActivity,
                        Column = cActivityType,
                        Group = true,
                    },
                },
                Rows = new[]
                {
                    new Grouping()
                    {
                        Source = dUser,
                        Column = cDepartment,
                        Group = true
                    },
                    new Grouping()
                    {
                        Source = fActivity,
                        Column = cfHostName,
                        Group = true
                    },
                    new Grouping()
                    {
                        Source = fActivity,
                        Column = cfMonth,
                        Group = true
                    },
                    new Grouping()
                    {
                        Source = fActivity,
                        Column = cfWeek,
                        Group = true
                    },
                },

            };

            var activityListReport = new Report
            {
                FactTable = fActivity,
                Columns = new[]
                {
                    new Grouping()
                    {
                        Source = fActivity,
                        Column = cfUtcStart,
                    },
                    new Grouping()
                    {
                        Source = fActivity,
                        Column = cfDuration,
                    },
                    new Grouping()
                    {
                        Source = dUser,
                        Column = cFullName,
                    },
                    new Grouping()
                    {
                        Source = dClassification,
                        Column = cType,
                    },
                    new Grouping()
                    {
                        Source = dClassification,
                        Column = cCTitle,
                    },
                    new Grouping()
                    {
                        Source = dActivity,
                        Column = cDescription,
                    },
                    new Grouping()
                    {
                        Source = dClassification,
                        Column = cFolder,
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
                        Source = fActivity,
                        Column = cfUtcStart,
                    },
                    new Grouping()
                    {
                        Source = dClassification,
                        Column = cFolder,
                    },
                    new Grouping()
                    {

                        Source = dUser,
                        Column = cFullName,
                    },
                    new Grouping()
                    {

                        Source = dClassification,
                        Column = cCTitle,
                    },
                    new Grouping()
                    {
                        Source = fActivity,
                        Column = cfMinutes,
                    },
                },
                Rows = new[]
                {
                    new Grouping()
                    {
                        Source = dClassification,
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
                        Source = dClassification,
                        Column = cFolder,
                    },
                    new Grouping()
                    {
                        Source = dClassification,
                        Column = cCTitle,
                    },
                    new Grouping()
                    {

                        Source = dClassification,
                        Column = cType,
                    },
                    new Grouping()
                    {
                        Source = dUser,
                        Column = cFullName,
                    },
                    new Grouping()
                    {
                        Source = fActivity,
                        Column = cfDuration,
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
                        Source = dClassification,
                        Column = cClassification
                    },
                    new Grouping()
                    {
                        Source = dClassification,
                        Column = cFolder,
                    },
                    new Grouping()
                    {
                        Source = dClassification,
                        Column = cCTitle,
                    },
                    new Grouping()
                    {
                        Source = dClassification,
                        Column = cType,
                    },
                    new Grouping()
                    {
                        Source = dActivity,
                        Column = cActivityType,
                    },
                    new Grouping()
                    {
                        Source = dUser,
                        Column = cFullName,
                    },
                    new Grouping()
                    {
                        Source = fActivity,
                        Column = cfDuration,
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
                        Source = fActivity,
                        Column = cfUtcStart,
                    },
                    new Grouping()
                    {
                        Source = dUser,
                        Column = cFullName,
                    },

                    new Grouping()
                    {
                        Source = dUser,
                        Column = cDepartment,
                    },
                    new Grouping()
                    {
                        Source = dClassification,
                        Column = cFolder,
                    },
                    new Grouping()
                    {
                        Source = dActivity,
                        Column = cATitle,
                    },
                    new Grouping()
                    {
                        Source = fActivity,
                        Column = cfDuration,
                    },
                  },
                Rows = new[]
                {
                    new Grouping()
                    {
                        Source = fActivity,
                        Column = cfMonth,
                        Group = true
                    },
                    new Grouping()
                    {
                        Source = fActivity,
                        Column = cfWeek,
                        Group = true
                    },
                },

                Measures = new[] { mActiveCount }
            };

            var weeklyTimesheet = new Report
            {
                FactTable = fActivity,
                Columns = new[]
                {
                    new Grouping()
                    {
                        Source = fActivity,
                        Column = cfUtcStart,
                    },
                    new Grouping()
                    {
                        Source = fActivity,
                        Column = cfUtcFinish,
                    },
                    new Grouping()
                    {
                        Source = fActivity,
                        Column = cfDuration,
                    },
                    new Grouping()
                    {
                        Source = fActivity,
                        Column = cfDay,
                        Group = true
                    },
                },
                Rows = new[]
                {
                    new Grouping()
                    {
                        Source = dUser,
                        Column = cFullName,
                        Group = true
                    },
                    new Grouping()
                    {
                        Source = fActivity,
                        Column = cfWeek,
                        Group = true
                    },
                   new Grouping()
                    {
                        Source = dClassification,
                        Column = cCTitle,
                        Group = true
                    },
                },

            };
            //CreateSQLFile(timesheetReport, "TimesheetReport");
            //CreateSQLFile(activityListReport, "ActivityListReport");
            CreateSQLFile(topicAllocation, "TopicAllocationReport");
            //CreateSQLFile(classificationAllocation, "ClassificationAllocationReport");
            CreateSQLFile(applicationSummary, "ApplicationSummary");
            //CreateSQLFile(adHocBillable, "AdHocBillable"); //rate of participant
            CreateSQLFile(weeklyTimesheet, "WeeklyTimesheet"); //leave and public holidays pivot days

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
