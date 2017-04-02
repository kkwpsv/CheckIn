using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CheckIn.Common.Models;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Net.Http;
using System.Net.Http.Headers;
using CheckIn.API;
using CheckIn.Common.Util;

namespace CheckIn.API.Controllers
{
    [Route("[controller]/[action]")]
    public class CheckInController : Controller
    {
        private CheckInContext context;

        public CheckInController(CheckInContext context)
        {
            this.context = context;
        }


        [HttpGet]
        public dynamic CheckIn()
        {
            try
            {
                if (HttpContext.Session.TryGetValue("userid", out var value))
                {
                    var userid = (value[0] << 24) + (value[1] << 16) + (value[2] << 8) + value[3];
                    var checkininfo = context.UserCheckInInfo;
                    if (checkininfo.Where(x => x.CheckInTime.Date == DateTime.Now.Date).Count() != 0)
                    {
                        return new { result = -2, message = "今天已经签到" };
                    }
                    else
                    {
                        checkininfo.Add(new UserCheckInInfo
                        {
                            UserID = userid,
                            CheckInTime = DateTime.Now,
                            OriCheckInTime = DateTime.Now,
                            HasCheckOut = false,
                        });
                        context.SaveChanges();
                        return new { result = 1 };
                    }
                }
                return new { result = 0, message = "Session异常" };
            }
            catch
            {
                return new { result = -1, message = "内部错误" };
            }
        }

        [HttpGet]
        public dynamic CheckOut()
        {
            try
            {
                if (HttpContext.Session.TryGetValue("userid", out var value))
                {
                    var userid = (value[0] << 24) + (value[1] << 16) + (value[2] << 8) + value[3];
                    var checkininfo = context.UserCheckInInfo;
                    var tocheckout = checkininfo.Where(x => x.UserID == userid && x.HasCheckOut == false && x.CheckInTime.Date == DateTime.Now.Date).FirstOrDefault();
                    if (tocheckout == null)
                    {
                        return new { result = -2, message = "尚未签到或已经签出" };
                    }
                    else
                    {
                        tocheckout.HasCheckOut = true;
                        tocheckout.CheckOutTime = DateTime.Now;
                        tocheckout.OriCheckOutTime = DateTime.Now;
                        context.SaveChanges();
                        return new { result = 1 };
                    }
                }
                return new { result = 0, message = "Session异常" };
            }
            catch
            {
                return new { result = -1, message = "内部错误" };
            }
        }

        [HttpGet]
        public dynamic AutoCheckOut(int checkinid, int hour, int minute, int second)
        {
            try
            {
                if (HttpContext.Session.TryGetValue("userid", out var value))
                {
                    var userid = (value[0] << 24) + (value[1] << 16) + (value[2] << 8) + value[3];
                    var checkininfo = context.UserCheckInInfo;
                    var tocheckout = checkininfo.Where(x => x.UserID == userid && x.HasCheckOut == false && x.CheckInID == checkinid).FirstOrDefault();
                    if (tocheckout == null)
                    {
                        return new { result = -2, message = "找不到该记录" };
                    }
                    else
                    {
                        tocheckout.HasCheckOut = true;
                        tocheckout.CheckOutTime = new DateTime(tocheckout.CheckInTime.Year, tocheckout.CheckInTime.Month, tocheckout.CheckInTime.Day, hour, minute, second);
                        tocheckout.OriCheckOutTime = new DateTime(tocheckout.CheckInTime.Year, tocheckout.CheckInTime.Month, tocheckout.CheckInTime.Day, hour, minute, second);
                        context.SaveChanges();
                        return new { result = 1 };
                    }
                }
                return new { result = 0, message = "Session异常" };
            }
            catch
            {
                return new { result = -1, message = "内部错误" };
            }
        }

        [HttpGet]
        public dynamic ModifyCheckIn(int checkinid, int hour, int minute, int second)
        {
            try
            {
                if (HttpContext.Session.TryGetValue("userid", out var value))
                {
                    var userid = (value[0] << 24) + (value[1] << 16) + (value[2] << 8) + value[3];
                    var checkininfo = context.UserCheckInInfo;
                    var tomodify = checkininfo.Where(x => x.UserID == userid && x.CheckInID == checkinid).FirstOrDefault();
                    if (tomodify == null)
                    {
                        return new { result = -2, message = "找不到该记录" };
                    }
                    else
                    {
                        tomodify.HasCheckOut = true;
                        tomodify.CheckInTime = new DateTime(tomodify.OriCheckInTime.Year, tomodify.OriCheckInTime.Month, tomodify.OriCheckInTime.Day, hour, minute, second);
                        context.SaveChanges();
                        return new { result = 1 };
                    }
                }
                return new { result = 0, message = "Session异常" };
            }
            catch
            {
                return new { result = -1, message = "内部错误" };
            }
        }

        [HttpGet]
        public dynamic ModifyCheckOut(int checkinid, int hour, int minute, int second)
        {
            try
            {
                if (HttpContext.Session.TryGetValue("userid", out var value))
                {
                    var userid = (value[0] << 24) + (value[1] << 16) + (value[2] << 8) + value[3];
                    var checkininfo = context.UserCheckInInfo;
                    var tomodify = checkininfo.Where(x => x.UserID == userid && x.CheckInID == checkinid).FirstOrDefault();
                    if (tomodify == null)
                    {
                        return new { result = -2, message = "找不到该记录" };
                    }
                    else
                    {
                        tomodify.HasCheckOut = true;
                        tomodify.CheckOutTime = new DateTime(tomodify.OriCheckOutTime.Year, tomodify.OriCheckOutTime.Month, tomodify.OriCheckOutTime.Day, hour, minute, second);
                        context.SaveChanges();
                        return new { result = 1 };
                    }
                }
                return new { result = 0, message = "Session异常" };
            }
            catch
            {
                return new { result = -1, message = "内部错误" };
            }
        }

        [HttpGet]
        public dynamic GetTodayStatus()
        {
            try
            {
                if (HttpContext.Session.TryGetValue("userid", out var value))
                {
                    var userid = (value[0] << 24) + (value[1] << 16) + (value[2] << 8) + value[3];
                    var info = context.UserCheckInInfo.Where(x => x.UserID == userid && x.CheckInTime.Date == DateTime.Now.Date).FirstOrDefault();
                    if (info == null)
                    {
                        return new { result = 1, hascheckin = false, hascheckout = false };
                    }
                    else
                    {
                        if (info.HasCheckOut)
                        {
                            return new { result = 1, hascheckin = true, hascheckout = true };
                        }
                        else
                        {
                            return new { result = 1, hascheckin = true, hascheckout = false };
                        }
                    }
                }
                return new { result = 0, message = "Session异常" };
            }
            catch
            {
                return new { result = -1, message = "内部错误" };
            }
        }

        [HttpGet]
        public dynamic GetMonthData(int year, int month)
        {
            try
            {
                if (HttpContext.Session.TryGetValue("userid", out var value))
                {
                    var userid = (value[0] << 24) + (value[1] << 16) + (value[2] << 8) + value[3];
                    var infos = context.UserCheckInInfo.Where(x => x.UserID == userid && x.CheckInTime.Date.Year == year && x.CheckInTime.Date.Month == month);
                    return new { result = 1, data = infos };
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
