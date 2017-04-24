using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CheckIn.Common.Models;
using CheckIn.Manager.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PagedList.Core;

namespace CheckIn.Manager.Util
{
    public static class RecordHelper
    {
        public static IQueryable<RecordModel> GetAbnormalRecords(CheckInContext context)
        {
            AutoConfirm(context);
            var records = context.UserCheckInInfo.Where(x => x.HasConfirmed == false).Join(context.UserInfo, x => x.UserID, x => x.UserID, (x, y) => new
            {
                x = x,
                y = y
            }).Join(context.DepartmentInfo, x => x.y.DepartmentID, x => x.DepartmentID, (x, y) => (new RecordModel
            {
                CheckInID = x.x.CheckInID,
                EmployeeID = x.y.EmployeeID,
                Name = x.y.Name,
                CheckInTime = x.x.CheckInTime,
                CheckOutTime = x.x.CheckOutTime,
                OriCheckInTime = x.x.OriCheckInTime,
                OriCheckOutTime = x.x.OriCheckOutTime,
                Reason1 = x.x.Reason1,
                Reason2 = x.x.Reason2,
                HasCheckOut = x.x.HasCheckOut,
                DepartmentName = y.DepartmentName
            })).Where(x => (x.HasCheckOut == false && x.CheckInTime.Date != DateTime.Now.Date) || x.CheckInTime != x.OriCheckInTime || x.CheckOutTime != x.OriCheckOutTime || (x.HasCheckOut && (x.CheckOutTime - x.CheckInTime > new TimeSpan(12, 0, 0) || x.CheckOutTime - x.CheckInTime < new TimeSpan(8, 0, 0)))).OrderBy(x => x.CheckInID);
            return records;
        }
        public static IQueryable<RecordModel> GetRecords(CheckInContext context)
        {
            AutoConfirm(context);
            var records = context.UserCheckInInfo.Join(context.UserInfo, x => x.UserID, x => x.UserID, (x, y) => new
            {
                x = x,
                y = y
            }).Join(context.DepartmentInfo, x => x.y.DepartmentID, x => x.DepartmentID, (x, y) => (new RecordModel
            {
                CheckInID = x.x.CheckInID,
                EmployeeID = x.y.EmployeeID,
                Name = x.y.Name,
                CheckInTime = x.x.CheckInTime,
                CheckOutTime = x.x.CheckOutTime,
                OriCheckInTime = x.x.OriCheckInTime,
                OriCheckOutTime = x.x.OriCheckOutTime,
                Reason1 = x.x.Reason1,
                Reason2 = x.x.Reason2,
                HasCheckOut = x.x.HasCheckOut,
                DepartmentID = x.y.DepartmentID,
                DepartmentName = y.DepartmentName,
                IsNormal = x.x.HasConfirmed
            }));
            return records;
        }

        public static IEnumerable<RecordModel> AddAbnormalCause(this IEnumerable<RecordModel> records)
        {
            foreach (var record in records)
            {
                if (record.HasCheckOut == false)
                {
                    if (record.CheckInTime.Date == DateTime.Now.Date)
                    {
                        continue;
                    }
                    record.AbnormalCause = "没有签出";
                }
                else if (record.CheckInTime != record.OriCheckInTime || record.CheckOutTime != record.OriCheckOutTime)
                {
                    record.AbnormalCause = "记录经过修正";
                }
                else if (record.HasCheckOut && record.CheckOutTime - record.CheckInTime > new TimeSpan(12, 0, 0))
                {
                    record.AbnormalCause = "时间过长";
                }
                else if (record.HasCheckOut && record.CheckOutTime - record.CheckInTime < new TimeSpan(8, 0, 0))
                {
                    record.AbnormalCause = "时间过短";
                }
            }
            return records;
        }

        private static void AutoConfirm(CheckInContext context)
        {
            var records = context.UserCheckInInfo.Where(x => x.HasConfirmed == false);
            foreach (var record in records)
            {
                if (record.HasCheckOut == true && record.CheckInTime == record.OriCheckInTime && record.CheckOutTime == record.OriCheckOutTime && record.CheckOutTime - record.CheckInTime < new TimeSpan(12, 0, 0) && record.CheckOutTime - record.CheckInTime > new TimeSpan(8, 0, 0))
                {
                    record.HasConfirmed = true;
                }
                context.SaveChanges();
            }
        }
    }
}
