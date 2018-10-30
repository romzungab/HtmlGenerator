using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Reporting;

namespace Server.Controllers
{
    public static class FacttableDimensionAttributes
    {
        public static FactTable[] GetFactTables()
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

            var dExpenseStatus = new Dimension()
            {
                Name = "Status",
                Table = null
            };

            var dCurrency = new Dimension()
            {
                Name = "Currency",
                Table = null
            };

            var dDate = new Dimension
            {
                Name = "Date",
                Table = null
            };

            var dSubmittedBy = new Dimension
            {
                Name = "User",
                Table = "dUser"
            };

            var dChangedBy = new Dimension
            {
                Name = "User",
                Table = "dUser"
            };

            var dHost = new Dimension
            {
                Name = "Host",
                Table = null
            };
            var dProcessedAmount = new Dimension
            {
                Name = "ProcessedAmount",
                Table = null
            };

            var dReferenceNumber = new Dimension()
            {
                Name = "ReferenceNumber",
                Table = null
            };
            var dExpenseLog = new Dimension()
            {
                Name = "Expense",
                Table = "dExpenseLog"
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
                Dimension = dClassification,
                SortExpression = t => $"case {t}.Title " +
                    "\n\t\twhen \'Business\' then \'1\'" +
                    "\n\t\twhen \'Leave\' then \'2\'" +
                    "\n\t\twhen \'Break\' then \'4\'" +
                    "\n\t\telse \'3\'" +
                    "\n\tend "
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

            var cStartTime = new DimensionAttribute(t => $"cast(min({t}.StartTime) as Time)")
            {
                Name = "StartTime",
                Dimension = dDate,
            };

            var cFinishTime = new DimensionAttribute(t => $"cast(max({t}.FinishTime) as Time)")
            {
                Name = "FinishTime",
                Dimension = dDate
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

            var cDate = new DimensionAttribute(t => $"cast({t}.[Date] as Date)")
            {
                Name = "Date",
                Dimension = dDate,
            };

            var cDay = new DimensionAttribute(t => $"DATENAME(weekday,{t}.[Date])")
            {
                Name = "Day",
                Dimension = dDate,
                SortExpression = t => $"((@@datefirst - 2 + datepart(dw, {t}.[Date])) % 7)",

            };

            var cWeek = new DimensionAttribute(t => $"dateadd(dd, -((@@datefirst - 2 + datepart(dw, {t}.[Date])) % 7), cast({t}.[Date] as date))")
            {
                Name = "Week",
                Dimension = dDate
            };

            var cMonth = new DimensionAttribute(t => $"datename(month,dateadd(month, datediff(month, 0, {t}.[Date]), 0))")
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

            var cFromDate = new DimensionAttribute("FromDate")
            {
                Name = "FromDate",
                Dimension = dDate
            };

            var cToDate = new DimensionAttribute("ToDate")
            {
                Name = "ToDate",
                Dimension = dDate
            };

            var cCreatedDate = new DimensionAttribute("CreatedDate")
            {
                Name = "CreatedDate",
                Dimension = dDate
            };

            var cExpenseStatus = new DimensionAttribute("Status")
            {
                Name = "Status",
                Dimension = dExpenseStatus
            };

            var cCurrency = new DimensionAttribute("Currency")
            {
                Name = "Currency",
                Dimension = dCurrency
            };

            var mMileage = new Measure("Mileage")
            {
                Name = "Mileage",
                AggregationFunction = ""
            };

            var mNonChargeableExpense = new Measure("NonChargeableExpense")
            {
                Name = "Non Chargeable Expense",
                AggregationFunction = ""
            };

            var mChargeableExpense = new Measure("ChargeableExpense")
            {
                Name = "ChargeableExpense",
                AggregationFunction = ""
            };

            var cProcessedAmount = new DimensionAttribute("ProcessedAmount")
            {
                Name = "ProcessedAmount",
                Dimension = dProcessedAmount
            };

            var cSubmittedBy = new DimensionAttribute("FullName")
            {
                Name = "SubmittedBy",
                Dimension = dSubmittedBy
            };

            var cSubmittedDate = new DimensionAttribute("SubmittedDate")
            {
                Name = "SubmittedDate",
                Dimension = dDate
            };
            var cChangeType = new DimensionAttribute("Type")
            {
                Name = "Type",
                Dimension = dExpenseLog
            };

            var cChangedBy = new DimensionAttribute("FullName")
            {
                Name = "ChangedBy",
                Dimension = dExpenseLog
            };

            var cChangedDateTimeUtc = new DimensionAttribute("ChangedDate")
            {
                Name = "ChangedDate",
                Dimension = dExpenseLog
            };

            var mStartTime = new Measure(t => $"cast(min({t}.StartTime) as Time)")
            {
                Name = "StartTime",
            };

            var mFinishTime = new Measure(t => $"cast(max({t}.FinishTime) as Time)")
            {
                Name = "FinishTime",
            };

            var mTotal = new Measure(t => $"{t}.Mileage + {t}.NonChargeableExpense + {t}.ChargeableExpense")
            {
                Name = "TotalSubmitted",
                AggregationFunction = "sum",
            };

            var cColumnName = new DimensionAttribute("ColumnName")
            {
                Name = "ColumnName",
                Dimension = dExpenseLog
            };
            var cValue = new DimensionAttribute("Value")
            {
                Name = "Value",
                Dimension = dExpenseLog
            };

            var cEReferenceNumber = new DimensionAttribute("ReferenceNumber")
            {
                Name = "ReferenceNumber",
                Dimension = dReferenceNumber,
            };

            //add attributes to dimensions
            dActivity.Attributes = new[]
            {
                cATitle, cActivityType, cDescription, cObjectDescription, cUtcStartDateTime, cUtcFinishDateTime
            };
            dUser.Attributes = new[]
            {
                cUserName, cFullName, cDepartment, cRegionId
            };
            dClassification.Attributes = new[] { cIsArchived, cIsPersonal, cReferenceNumber, cCTitle, cFolder, cParent, cTaskPriority, cActualWork, cTotalWork, cTaskStatus, cDueDateTime, cCompletedDate, cTaskFinish, cTaskStart };
            dDate.Attributes = new[] { cStartTime, cFinishTime, cDate, cWeek, cMonth, cDay, cSubmittedDate, cCreatedDate, cChangedDateTimeUtc };
            dParticipant.Attributes = new[] { cParticipantRoleId, cRole, cRate, cOtherRate };
            dHost.Attributes = new[] { cHostName };

            dExpenseStatus.Attributes = new[] { cExpenseStatus };
            dCurrency.Attributes = new[] { cCurrency };
            dSubmittedBy.Attributes = new[] { cSubmittedBy };
            dChangedBy.Attributes = new[] { cChangedBy };
            dExpenseLog.Attributes = new[] { cColumnName, cValue, cChangedDateTimeUtc, cChangedBy };
            dReferenceNumber.Attributes = new[] { cEReferenceNumber };
            dProcessedAmount.Attributes = new[] { cProcessedAmount };

            var fExpense = new FactTable
            {
                Name = "Expense",
                Table = "fExpense",
                Dimensions = new[] { dExpenseStatus, dUser, dSubmittedBy, dChangedBy, dExpenseLog, dProcessedAmount }
            };

            var fActivity = new FactTable
            {
                Name = "Activity",
                Table = "fActivity",
                Dimensions = new[] { dActivity, dUser, dClassification, dParticipant, dDate, dHost },
            };

            fExpense.Measures = new[] { mTotal, mMileage, mNonChargeableExpense, mChargeableExpense };

            var mDuration = new Measure("Duration")
            {
                Name = "Duration",
                Table = fActivity,
                AggregationFunction = "sum",
            };

            fActivity.Measures = new[] { mActiveCount, mDuration }; //, mStart, mFinish };

            return new [] {fActivity, fExpense};
        }
    }
}
