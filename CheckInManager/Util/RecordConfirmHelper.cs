using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CheckIn.Common.Models;
using CheckIn.Manager.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CheckIn.Manager.Util
{
    public static class RecordConfirmHelper
    {
        public static List<AbnormalRecordModel> GetAbnormalRecords(CheckInContext context)
        {
            AutoConfirm(context);
            var records = context.UserCheckInInfo.Where(x => x.HasConfirmed == false).Join(context.UserInfo, x => x.UserID, x => x.UserID, (x, y) => (new AbnormalRecordModel
            {
                CheckInID = x.CheckInID,
                EmployeeID = y.EmployeeID,
                Name = y.Name,
                CheckInTime = x.CheckInTime,
                CheckOutTime = x.CheckOutTime,
                OriCheckInTime = x.OriCheckInTime,
                OriCheckOutTime = x.OriCheckOutTime,
                Reason1 = x.Reason1,
                Reason2 = x.Reason2,
                HasCheckOut = x.HasCheckOut,
            })).ToList();
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
            return records.OrderBy(x => x.CheckInID).Where(x => x.AbnormalCause != null).ToList();
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
