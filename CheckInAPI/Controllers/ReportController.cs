using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CheckIn.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace CheckIn.API.Controllers
{
    [Route("[controller]/[action]")]
    public class ReportController : Controller
    {
        private CheckInContext context;

        public ReportController(CheckInContext context)
        {
            this.context = context;
        }
        public dynamic GetReport(int year, int month)
        {
            try
            {
                if (HttpContext.Session.TryGetValue("userid", out var value))
                {
                    var userid = (value[0] << 24) + (value[1] << 16) + (value[2] << 8) + value[3];
                    var dic = context.UserInfo.GroupJoin(context.UserCheckInInfo, x => x.UserID, x => x.UserID, (x, y) => new
                    {
                        UserID = x.UserID,
                        Days = y.Where(z => z.CheckInTime.Year == year && z.CheckInTime.Month == month && z.HasCheckOut && z.CheckOutTime - z.CheckInTime >= new TimeSpan(8, 0, 0)).Count()
                    }).ToDictionary(x => x.UserID, x => x.Days);

                    var attendanceday = dic[userid];
                    //var attendancerate = (double)attendanceday / DateTime.DaysInMonth(year, month);
                    var exceedingcount = dic.Values.OrderBy(x => x).ToList().IndexOf(attendanceday);
                    var exceedingrate = (double)exceedingcount / context.UserInfo.Count();
                    return new { result = 1, attendanceday = attendanceday, exceedingrate = exceedingrate };
                }
                return new { result = 0, message = "Session异常" };
            }
            catch
            {
                return new { result = -1, message = "内部错误" };
            }
        }

    }
}
