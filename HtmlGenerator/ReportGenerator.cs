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

            var dParticipant = new Dimension
            {
                Name = "Participant",
                Table = "dParticipant"
            };

            var dLeave = new Dimension
            {
                Name = "Leave",
                Table = "dLeave"
            };

            var dDate = new Dimension
            {
                Name = "Date",
                Table = null
            };

            var dHost = new Dimension
            {
                Name = "Host",
                Table = null
            };

            //dClassification columns
            var cTaskStart = new DimensionAttribute("StartDateTime")
            {
                Name = "TaskStart",
                Dimension = dClassification
            };
            var cTaskFinish = new DimensionAttribute("FinishDateTime")
            {
                Name = "TaskFinish",
                Dimension = dClassification
            };
            var cCreatedDateTime = new DimensionAttribute("CreatedDateTime")
            {
                Name = "CreatedDateTime",
                Dimension = dClassification
            };
            var cDueDateTime = new DimensionAttribute("DueDateTime")
            {
                Name = "DueDateTime",
                Dimension = dClassification
            };
            var cCompletedDate = new DimensionAttribute("ActualCompletionDateTime")
            {
                Name = "CompletedDate",
                Dimension = dClassification
            };
            var cNotes = new DimensionAttribute("Notes")
            {
                Name = "Notes",
                Dimension = dClassification
            };

            var cReferenceNumber = new DimensionAttribute("ReferenceNumber")
            {
                Name = "ReferenceNumber",
                Dimension = dClassification
            };

            var cType = new DimensionAttribute("Type")
            {
                Name = "Type",
                Dimension = dClassification
            };

            var cFolder = new DimensionAttribute("Folder")
            {
                Name = "Folder",
                Dimension = dClassification
            };

            var cAssignedResource = new DimensionAttribute("AssignedResource")
            {
                Name = "AssignedResource",
                Dimension = dClassification
            };

            var cTaskStatus = new DimensionAttribute("TaskStatus")
            {
                Name = "TaskStatus",
                Dimension = dClassification
            };

            var cTaskPriority = new DimensionAttribute("TaskPriority")
            {
                Name = "TaskPriority",
                Dimension = dClassification
            };

            var cParent = new DimensionAttribute("Parent")
            {
                Name = "Parent",
                Dimension = dClassification
            };

            var cIsPersonal = new DimensionAttribute("IsPersonal")
            {
                Name = "IsPersonal",
                Dimension = dClassification
            };

            var cIsPrivate = new DimensionAttribute("IsPrivate")
            {
                Name = "IsPrivate",
                Dimension = dClassification
            };

            var cIsArchived = new DimensionAttribute("IsArchived")
            {
                Name = "IsArchived",
                Dimension = dClassification
            };

            var cActualWork = new DimensionAttribute("Actualwork")
            {
                Name = "ActualWork",
                Dimension = dClassification
            };

            var cTotalWork = new DimensionAttribute("TotalWork")
            {
                Name = "TotalWork",
                Dimension = dClassification
            };

            var cCTitle = new DimensionAttribute(t => "case" +
                $"\n\twhen {t}.Id is null then \'[Unclassified]\'" +
                $"\n\telse {t}.Title" +
                $"\n\tend")
            {
                Name = "Title",
                Dimension = dClassification
            };

            //dActivity columns
            var cATitle = new DimensionAttribute("Title")
            {
                Name = "ApplicationName",
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

            var cObjectDescription = new DimensionAttribute("ObjectDescription")
            {
                Name = "ObjectDescription",
                Dimension = dActivity
            };

            var cUtcStartDateTime = new DimensionAttribute("UtcStartDateTime")
            {
                Name = "UtcStartDateTime",
                Dimension = dActivity
            };

            var cUtcFinishDateTime = new DimensionAttribute("UtcFinishDateTime")
            {
                Name = "UtcFinishDateTime",
                Dimension = dActivity
            };
            var cHostName = new DimensionAttribute("HostName")
            {
                Name = "HostName",
                Dimension = dHost
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

            var cLeaveStatus = new DimensionAttribute("Status")
            {
                Name = "LeaveStatus",
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
            var cLReferenceNumber = new DimensionAttribute(("ReferenceNumber"))
            {
                Name = "ReferenceNumber",
                Dimension = dLeave
            };
            var cLeaveType = new DimensionAttribute("LeaveType")
            {
                Name = "LeaveType",
                Dimension = dLeave
            };
            var cStartDateTime = new DimensionAttribute("StartDateTime")
            {
                Name = "First Day of Leave",
                Dimension = dLeave,
            };
            var cFinishDateTime = new DimensionAttribute("FinishDateTime")
            {
                Name = "Last Day of Leave",
                Dimension = dLeave,
            };
           
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

            var cRate = new DimensionAttribute("Rate")
            {
                Name = "Rate",
                Dimension = dParticipant
            };

            var cOtherRate = new DimensionAttribute("OtherRate")
            {
                Name = "OtherRate",
                Dimension = dParticipant
            };

            var cParticipantRoleId = new DimensionAttribute("ParticipantRoleId")
            {
                Name = "ParticipantRoleId",
                Dimension = dParticipant
            };


            var cRole = new DimensionAttribute("Role")
            {
                Name = "Role",
                Dimension = dParticipant
            };

            var mActiveCount = new Measure("ActiveCount")
            {
                AggregationFunction = "sum",
                Name = "ActiveCount",
            };


            //dimensions

            //add attributes to dimensions
            dActivity.Attributes = new[] { cATitle, cActivityType, cDescription, cObjectDescription, cUtcStartDateTime, cUtcFinishDateTime };
            dUser.Attributes = new[] { cUserName, cFullName, cDepartment, cRegionId };
            dClassification.Attributes = new[] { cIsArchived, cIsPersonal, cReferenceNumber, cCTitle, cFolder, cParent, cTaskPriority, cActualWork, cTotalWork, cTaskStatus, cDueDateTime, cCompletedDate, cTaskFinish, cTaskStart };
            dDate.Attributes = new[] { cDate, cWeek, cMonth, cDay };
            dParticipant.Attributes = new[] { cParticipantRoleId, cRate, cOtherRate };
            dHost.Attributes = new[] { cHostName };
            dLeave.Attributes = new[] { cStartDateTime, cFinishDateTime, cLeaveStatus, cManagerNote, cPayrollNote, cLeaveType, cLReferenceNumber };

            var fLeave = new FactTable
            {
                Name = "Leave",
                Table = "fLeave",
                Dimensions = new[] { dLeave, dUser },
            };


            var fActivity = new FactTable
            {
                Name = "Activity",
                Table = "fActivity",
                Dimensions = new[] { dActivity, dUser, dClassification, dDate, dHost },
            };
            var mLDuration = new Measure("Duration")
            {
                Name = "Duration",
                AggregationFunction = "sum",
                Table = fLeave,

            };
            fLeave.Measures = new[] { mLDuration };
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

                    new ReportColumn()
                    {
                        Attribute = cActivityType,
                        Grouped = true
                    },
                    new ReportColumn()
                    {
                        Attribute = cCTitle,
                        Grouped = true
                    },
                },
                Rows = new[]
                {
                   cDepartment,
                   cHostName,
                   cMonth,
                   cWeek,
                   cFullName
                },
                Measures = new[] { mStart, mFinish, mDuration },
                Filters = new[]
                {
                    new ReportFilter()
                    {
                        Filter = cFullName,
                        Operation = "=",
                        Value = "Roms Ungab"
                    },

                    new ReportFilter()
                    {
                        Filter = new DimensionAttribute("StartTime")
                        {
                             Name = "StartTime"
                        },
                        Operation = ">",
                        Value = "20180101"
                    }
                }
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
                        Attribute = cType,
                    },
                    new ReportColumn()
                    {
                        Attribute = cCTitle,
                    },
                    new ReportColumn()
                    {
                        Attribute = cObjectDescription,
                    },
                    new ReportColumn()
                    {
                        Attribute = cFolder,
                    },
                },
                Rows = new[] { cFullName },
                Measures = new[] { mActiveCount, mDuration },
                Filters = new[]
                {
                    new ReportFilter()
                    {
                        Filter = new DimensionAttribute("StartTime")
                        {
                            Name = "StartTime"
                        },
                        Operation = ">",
                        Value = "20180101"
                    }
                }
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
                    new ReportColumn()
                    {
                        Attribute = cRate,
                    },
                },
                Rows = new[] { cFolder },
                Measures = new[]
                {
                    new Measure(t => $"floor(ceiling({t}.Duration + 30 - 1) / 30) * 30")
                    {
                        AggregationFunction = "sum",
                        Name = "NearestThirtyMinutes",
                        Table = fActivity,
                    },
                },
                Filters = new[]
                {
                    new ReportFilter()
                    {
                        Filter = new DimensionAttribute("StartTime")
                        {
                            Name = "StartTime"
                        },
                        Operation = ">",
                        Value = "20180101"
                    }
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
                Measures = new[]
                {
                    new Measure(t => $"coalesce({t}.Duration, 0)")
                    {
                        AggregationFunction = "sum",
                        Name ="AllocatedWork",
                        Table = fActivity,
                    }
                },
                Filters = new[]
                {
                    new ReportFilter()
                    {
                        Filter = new DimensionAttribute("StartTime")
                        {
                            Name = "StartTime"
                        },
                        Operation = ">",
                        Value = "20180101"
                    }
                }
            };

            var classificationAllocation = new Report()
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
                        Attribute = cActivityType,
                    },
                    new ReportColumn()
                    {
                        Attribute = cFullName,
                    },
                },
                Measures = new[] { mDuration },
                Filters = new[]
                {
                    new ReportFilter()
                    {
                        Filter = new DimensionAttribute("StartTime")
                        {
                            Name = "StartTime"
                        },
                        Operation = ">",
                        Value = "20180101"
                    }
                }
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

                 },
                Rows = new[] { cATitle, cMonth, cWeek },
                Measures = new[] { mDuration, mActiveCount },
                Filters = new[]
                {
                    new ReportFilter()
                    {
                        Filter = new DimensionAttribute("StartTime")
                        {
                            Name = "StartTime"
                        },
                        Operation = ">",
                        Value = "20180101"
                    }
                }
            };

            var weeklyTimesheet = new Report
            {
                FactTable = fActivity,
                Columns = new[]
                {
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
                Rows = new[] { cDay },
                Measures = new[] { mDuration, mActiveCount, mStart, mFinish },
                Filters = new[]
                {
                    new ReportFilter()
                    {
                        Filter = new DimensionAttribute("StartTime")
                        {
                            Name = "StartTime"
                        },
                        Operation = ">",
                        Value = "20180101"
                    }
                }
            };

            var billableProject = new Report
            {
                FactTable = fActivity,
                Columns = new[]
                {
                    new ReportColumn()
                    {
                        Attribute = cCTitle,
                    },
                   },
                Rows = new[] { cDay, cFullName, cWeek },
                Measures = new[] { mDuration }
            };

            var allocatedWork = new Report
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
                        Attribute = cCTitle,
                    },
                    new ReportColumn()
                    {
                        Attribute = cType,
                    },
                    new ReportColumn()
                    {
                        Attribute = cCreatedDateTime
                    },
                },
                Rows = new[] { cFullName },
                Measures = new[] { mDuration }
            };

            var detailBillable = new Report()
            {
                FactTable = fActivity,
                Columns = new[]
                {
                    new ReportColumn()
                    {
                        Attribute = cRole,
                    },
                    new ReportColumn()
                    {
                        Attribute = cFullName,
                    },
                    new ReportColumn()
                    {
                        Attribute = cATitle,
                    },
                    new ReportColumn()
                    {
                        Attribute = cType,
                    },
                    new ReportColumn()
                    {
                        Attribute = cCTitle,
                    },

                },
                Measures = new[] { mDuration }
            };


            var taskList = new Report()
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
                        Attribute = cDueDateTime,
                    },
                    new ReportColumn()
                    {
                        Attribute = cAssignedResource,
                    },
                    new ReportColumn()
                    {
                        Attribute = cTaskStatus,
                    },
                    new ReportColumn()
                    {
                        Attribute = cTaskPriority,
                    },
                    new ReportColumn()
                    {
                        Attribute = cTotalWork,
                    },
                    new ReportColumn()
                    {
                        Attribute = cNotes,
                    },
                },
                Measures = new[] { mStart, mFinish, mDuration },
                Filters = new[]
                {
                    new ReportFilter()
                    {
                        Filter = cType,
                        Operation = "=",
                        Value = "Task"
                    },
                }

            };


            var projectList = new Report()
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
                        Attribute = cCTitle
                    },

                    new ReportColumn()
                    {
                        Attribute = cAssignedResource,
                    },
                    new ReportColumn()
                    {
                        Attribute = cTaskStatus,
                    },
                    new ReportColumn()
                    {
                        Attribute = cDueDateTime,
                    },
                    new ReportColumn()
                    {
                        Attribute = cCompletedDate
                    },
                    new ReportColumn()
                    {
                        Attribute = cTotalWork,
                    },
                },

                Measures = new[] { mStart, mFinish, mDuration },
                Filters = new[]
                {
                    new ReportFilter()
                    {
                        Filter = cType,
                        Operation = "=",
                        Value = "Project"
                    },
                }

            };
            var leave = new Report()
            {
                FactTable = fLeave,
                Columns = new[]
                {
                    new ReportColumn()
                    {
                        Attribute = cStartDateTime
                    },
                    new ReportColumn()
                    {
                        Attribute =  cFinishDateTime
                    },
                    new ReportColumn()
                    {
                        Attribute = cFullName,
                    },
                    new ReportColumn()
                    {
                        Attribute = cLeaveType,
                    },
                    new ReportColumn()
                    {
                        Attribute = cLeaveStatus,
                    }, 
                   
                    new ReportColumn()
                    {
                        Attribute = cDepartment
                    },
                    new ReportColumn()
                    {
                        Attribute = cManagerNote
                    },
                    new ReportColumn() 
                    {
                        Attribute = cPayrollNote
                    },
               },
                Measures = new[] { mLDuration },

            };

            CreateSQLFile(timesheetReport, "TimesheetReport");
            //CreateSQLFile(activityListReport, "ActivityListReport");
            //CreateSQLFile(topicAllocation, "TopicAllocationReport");
            //CreateSQLFile(classificationAllocation, "ClassificationAllocationReport");
            //CreateSQLFile(applicationSummary, "ApplicationSummary");
            //CreateSQLFile(adHocBillable, "AdHocBillable"); //rate of participant ok
            //CreateSQLFile(weeklyTimesheet, "WeeklyTimesheet"); //leave and public holidays ; pivot days
            //CreateSQLFile(billableProject, "BillableProject");
            //CreateSQLFile(allocatedWork, "AllocatedWork");
            //CreateSQLFile(detailBillable, "DetailBillable");
            //CreateSQLFile(taskList, "TaskList");
            //CreateSQLFile(projectList, "ProjectList");

            //CreateSQLFile(leave, "Leave");

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
