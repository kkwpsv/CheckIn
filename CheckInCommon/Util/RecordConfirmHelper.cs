using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CheckIn.Common.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CheckIn.Common.Util
{
    public static class RecordConfirmHelper
    {
        public static List<UserCheckInInfo> GetAbnormalRecords(CheckInContext context)
        {
            var records = context.UserCheckInInfo.Where(x => x.HasConfirmed == false);
            var result = new List<UserCheckInInfo>();
            foreach (var record in records)
            {
                if (record.HasCheckOut == false && record.CheckInTime.Date == DateTime.Now.Date)
                {
                    record.AbnormalCause = "没有签出";
                }
                else if (record.CheckInTime != record.OriCheckInTime || record.CheckOutTime != record.OriCheckOutTime)
                {
                    record.AbnormalCause = "记录经过修正";
                }
                else if (record.CheckOutTime - record.CheckInTime > new TimeSpan(12, 0, 0))
                {
                    record.AbnormalCause = "时间过长";
                }
                else if (record.CheckOutTime - record.CheckInTime < new TimeSpan(8, 0, 0))
                {
                    record.AbnormalCause = "时间过短";
                }
                else
                {
                    record.HasConfirmed = true;
                    continue;
                }
                result.Add(record);
            }
            context.SaveChanges();
            return result;
        }
    }
}
