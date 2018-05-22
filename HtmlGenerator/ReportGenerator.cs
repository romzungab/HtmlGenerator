using System;
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
                Name = "Classification",
                Table = "dClassification",
            };

            var dUser = new Dimension
            {
                Name = "User",
                Table = "dUser"
            };

            var dActivity = new Dimension
            {
                Name = "Activity",
                Table = "dActivity"
            };

            var dLeave = new Dimension
            {
                Name = "Leave",
                Table = "dLeave"
            };

            var dDate = new Dimension
            {
                Name ="Date",
                Table = null
            };

            //dClassification columns
            var cReferenceNumber = new DimensionAttribute("ReferenceNumber")
            {
                Dimension = dClassification
            };

            var cType = new DimensionAttribute("dClassification.Type")
            {
                Name = "Type",
                Dimension = dClassification
            };

            var cFolder = new DimensionAttribute("dClassification.Folder")
            {
                Name = "Folder",
                Dimension = dClassification
            };

            var cParent = new DimensionAttribute("dClassification.Parent")
            {
                Name = "Parent",
                Dimension = dClassification
            };

            var cIsPersonal = new DimensionAttribute("dClassification.IsPersonal")
            {
                Name = "IsPersonal",
                Dimension = dClassification
            };

            var cIsPrivate = new DimensionAttribute("dClassification.IsPrivate")
            {
                Name = "IsPrivate",
                Dimension = dClassification
            };

            var cIsArchived = new DimensionAttribute("dClassification.IsArchived")
            {
                Name = "IsArchived",
                Dimension = dClassification
            };

            var cCTitle = new DimensionAttribute(t => "case" +
                $"\n\twhen {t}.dClassificationId is null then \'[Unclassified]\'" +
                $"\n\telse dClassification.Title" +
                $"\n\tend as Title")
            {
                Name = "Title",
                Dimension = dClassification
            };

            var cClassification = new DimensionAttribute(t => $"case " +
                $"\n\t\twhen {t}.IsArchived = 1 then \'[Archived]\'" +
                $"\n\t\telse coalesce(dClassification.Parent, \'-\')" +
                $"\n\tend Classification")
            {
                Name = "Classification",
                Dimension = dClassification
            };

            //dActivity columns
            var cATitle = new DimensionAttribute("Title")
            {
                Name = "Title",
                Dimension = dActivity
            };

            var cActivityType = new DimensionAttribute("ActivityType")
            {
                Name = "ActivityType",
                Dimension = dActivity
            };

            var cDescription = new DimensionAttribute("Description")
            {
                Name = "Description",
                Dimension = dActivity
            };
            var cHostName = new DimensionAttribute("HostName")
            {
                Name = "HostName",
                Dimension = dActivity
            };

            //dUser columns
            var cFullName = new DimensionAttribute("FullName")
            {
                Name = "FullName",
                Dimension = dUser
            };
            var cUserName = new DimensionAttribute("UserName")
            {
                Name = "UserName",
                Dimension = dUser
            };

            var cRegionId = new DimensionAttribute("RegionId")
            {
                Name = "RegionId",
                Dimension = dUser
            };
            var cDepartment = new DimensionAttribute("Department")
            {
                Name = "Department",
                Dimension = dUser
            };

            var cLeaveTime = new DimensionAttribute("LeaveTime")
            {
                Name = "LeaveTime",
                Dimension = dLeave
            };
            var cStatus = new DimensionAttribute("Status")
            {
                Name = "Status",
                Dimension = dLeave
            };
            var cManagerNote = new DimensionAttribute("ManagerNote")
            {
                Name = "ManagerNote",
                Dimension = dLeave
            };
            var cPayrollNote = new DimensionAttribute("PayrollNote")
            {
                Name = "PayrollNote",
                Dimension = dLeave
            };
            var cLeaveType = new DimensionAttribute("LeaveType")
            {
                Name = "LeaveType",
                Dimension = dLeave
            };
            var cStartTime = new DimensionAttribute("StartTime")
            {
                Name = "StartTime",
                Dimension = dLeave
            };
            var cFinishTime = new DimensionAttribute("FininshTime")
            {
                Name = "FinishTime",
                Dimension = dLeave
            };

            //var cMinutes = new DimensionAttribute(t => $"floor(ceiling({t}.Duration + 30 - 1) / 30) * 30 NearestThirtyMinutes")
            //{
            //    Name = "NeareasThirtyMinutes",
            //    Dimension = dActivity
            //};

            var cDate = new DimensionAttribute(t => $"cast({t}.[Date] as Date)")
            {
                Name = "Date",
                Dimension = dDate,
            };

            var cDay = new DimensionAttribute(t => $"DATENAME(weekday,{t}.[Date])")
            {
                Name = "Day",
                Dimension = dDate
            };

            var cWeek = new DimensionAttribute(t => $"dateadd(dd, -((@@datefirst - 2 + datepart(dw, {t}.[Date])) % 7), cast({t}.[Date] as date))")
            {
                Name = "Week",
                Dimension = dDate
            };

            var cMonth = new DimensionAttribute(t => $"dateadd(month, datediff(month, 0, {t}.[Date]), 0)")
            {
                Name = "Month",
                Dimension = dDate
            };

            var mActiveCount = new Measure("ActiveCount")
            {
                AggregationFunction = "sum",
                Name = "ActiveCount",
            };
            //var mAllocatedWork = new Measure()
            //{
            //    Name = "AllocatedWork",
            //};
            //var mActiveTime = new Measure
            //{
            //    Name = "ActiveTime"
            //};

            //dimensions
            var cLDuration = new Measure(t => $"datediff(ms, {t}.StartTime, fLeave.FinishTime) / 1000 Duration")
            {
                Name = "Duration",
            };

            //add attributes to dimensions
            dActivity.Attributes = new[] { cATitle, cActivityType, cDescription, cHostName };
            dUser.Attributes = new[] { cUserName, cFullName, cDepartment, cRegionId };
            dClassification.Attributes = new[] {cIsArchived,cIsPersonal,cReferenceNumber, cCTitle, cFolder, cParent };
            dDate.Attributes = new[] {cDate, cWeek, cMonth, cDay };

            var fLeave = new FactTable
            {
                Name = "Leave",
                Table = "fLeave",
                Dimensions = new[] { dLeave, dUser },
                Measures = new[] { cLDuration }
            };

            var fActivity = new FactTable
            {
                Name = "Activity",
                Table = "fActivity",
                Dimensions = new[] { dActivity, dUser, dClassification, dDate},
            };


            var mDuration = new Measure("Duration")
            {
                Name = "Duration",
                Table = fActivity,
                AggregationFunction = "sum",
            };

            var mStart = new Measure("StartTime")
            {
                Name = "Start",
                Table = fActivity,
                AggregationFunction = "min"
            };

            var mFinish = new Measure("FinishTime")
            {
                Name = "Finish",
                Table = fActivity,
                AggregationFunction = "max"
            };

            fActivity.Measures = new[] { mActiveCount, mDuration, mStart, mFinish };

            //reports
            var timesheetReport = new Report
            {
                FactTable = fActivity,
                Columns = new[]
                {
                    new ReportColumn()
                    {
                        Attribute = cDate,
                     },
                    //new ReportColumn()
                    //{
                    //    Attribute = cUtcStart,
                    //},
                    //new ReportColumn()
                    //{
                    //   Attribute = cUtcFinish,
                    //},
                    new ReportColumn()
                    {
                       Attribute = cFullName,
                    },
                },
                Rows = new[]
                {
                   cDepartment,
                   //cHostName,
                   cMonth,
                   cWeek,
                },
                Measures = new[] { mStart, mFinish, mDuration },
            };

            var activityListReport = new Report
            {
                FactTable = fActivity,
                Columns = new[]
                {
                    new ReportColumn()
                    {
                        Attribute = cDate,
                    },
                   
                    new ReportColumn()
                    {
                        Attribute = cFullName,
                    },
                    new ReportColumn()
                    {
                        Attribute = cType,
                    },
                    new ReportColumn()
                    {
                        Attribute = cCTitle,
                    },
                    new ReportColumn()
                    {
                        Attribute = cDescription,
                    },
                    new ReportColumn()
                    {
                        Attribute = cFolder,
                    },
                },
                Measures = new[] { mDuration },

            };

            var adHocBillable = new Report
            {
                FactTable = fActivity,
                Columns = new[]
                {
                    new ReportColumn()
                    {
                        Attribute = cDate,
                    },
                    new ReportColumn()
                    {
                        Attribute = cFullName,
                    },
                    new ReportColumn()
                    {
                        Attribute = cCTitle,
                    },
                },
                Rows = new[] { cFolder },
                Measures = new []
                {
                    new Measure(t => $"floor(ceiling({t}.Duration + 30 - 1) / 30) * 30")
                    {
                        AggregationFunction = "sum",
                        Name = "NearestThirtyMinutes",
                        Table = fActivity,
                    }, 
                }
            };

            var topicAllocation = new Report()
            {
                FactTable = fActivity,
                Columns = new[]
                {
                    new ReportColumn()
                    {
                        Attribute = cFolder,
                    },
                    new ReportColumn()
                    {
                        Attribute = cCTitle,
                    },
                    new ReportColumn()
                    {
                        Attribute = cType,
                    },
                    new ReportColumn()
                    {
                        Attribute = cFullName,
                    },
                 },
                Measures = new[] { mDuration },
            };

            var classificationAllocation = new Report()
            {
                FactTable = fActivity,
                Columns = new[]
                {
                    new ReportColumn()
                    {
                        Attribute = cClassification
                    },
                    new ReportColumn()
                    {
                        Attribute = cFolder,
                    },
                    new ReportColumn()
                    {
                        Attribute = cCTitle,
                    },
                    new ReportColumn()
                    {
                        Attribute = cType,
                    },
                    new ReportColumn()
                    {
                        Attribute = cActivityType,
                    },
                    new ReportColumn()
                    {
                        Attribute = cFullName,
                    },
                },
                Measures = new[] { mDuration },
            };

            var applicationSummary = new Report()
            {
                FactTable = fActivity,
                Columns = new[]
                {
                    new ReportColumn()
                    {
                        Attribute = cDate,
                    },
                    new ReportColumn()
                    {
                       Attribute = cFullName,
                    },

                    new ReportColumn()
                    {
                        Attribute = cDepartment,
                    },
                    new ReportColumn()
                    {
                        Attribute = cFolder,
                    },
                    new ReportColumn()
                    {
                        Attribute = cATitle,
                    },
                 },
                Rows = new[] { cMonth, cWeek },
                Measures = new[] { mDuration, mActiveCount }
            };

            var weeklyTimesheet = new Report
            {
                FactTable = fActivity,
                Columns = new[]
                {
                    //new ReportColumn()
                    //{
                    //    Attribute = cUtcStart,
                    //},
                    //new ReportColumn()
                    //{
                    //    Attribute = cUtcFinish,
                    //},
                    new ReportColumn()
                    {
                        Attribute = cDay,
                        Grouped = true
                    },
                    new ReportColumn()
                    {
                        Attribute = cFullName,
                        Grouped = true
                    },
                    new ReportColumn()
                    {
                        Attribute = cWeek,
                        Grouped = true
                    },
                    new ReportColumn()
                    {
                        Attribute = cCTitle,
                        Grouped = true
                    },
                },
                Measures = new[] { mDuration, mActiveCount }
            };
            CreateSQLFile(timesheetReport, "TimesheetReport");
            return;
            CreateSQLFile(activityListReport, "ActivityListReport");
            CreateSQLFile(topicAllocation, "TopicAllocationReport");
            CreateSQLFile(classificationAllocation, "ClassificationAllocationReport");
            CreateSQLFile(applicationSummary, "ApplicationSummary");
            CreateSQLFile(adHocBillable, "AdHocBillable"); //rate of participant
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
            var sql = new SQLGenerator(report).BuildSQL();
            var reportFilename = @"C:\Users\romelyn.ungab\Documents\sql\" + reportName + ".sql";
            File.WriteAllText(reportFilename, sql);
            Process.Start(reportFilename);
        }
    }
}
