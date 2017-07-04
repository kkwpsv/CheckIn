using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CheckIn.Common.Models;
using System.IO;
using System.Text;
using CheckIn.Common.Util;
using Microsoft.AspNetCore.Http;
using System.Net;
using Lsj.Util.Text;

namespace CheckIn.API.Controllers
{
    [Route("[controller]/[action]")]
    public class UserController :Controller
    {
        private CheckInContext context;

        public UserController(CheckInContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public dynamic Login(string username, string password, string deviceid)
        {
            try
            {
                if (username.IsNullOrEmpty() || password.IsNullOrEmpty() || deviceid.IsNullOrEmpty())
                {
                    return new { result = -1, message = "内部错误" };
                }
                var user = context.UserInfo.Where(x => (x.EmployeeID == username && x.Password == password) || (x.MobilephoneNumber == username && x.Password == password)).FirstOrDefault();
                var result = user != null;
                if (!result)
                {
                    return new { result = -2, message = "用户名或密码错误" };
                }
                else
                {
                    var token = TokenHelper.BuildToken(deviceid);
                    user.Token = token;
                    context.SaveChanges();
                    return new { result = 1, token = token };
                }
            }
            catch
            {
                return new { result = -1, message = "内部错误" };
            }
        }

        [HttpGet]
        public dynamic GetCheckCode()
        {
            try
            {
                var result = CheckCodeHelper.GetCheckCode();
                var image = result.image;
                image.Seek(0, SeekOrigin.Begin);
                var bytes = new byte[image.Length];
                image.Read(bytes, 0, bytes.Length);
                HttpContext.Session.Set("checkcode", Encoding.ASCII.GetBytes(result.text.ToLower()));
                return new { result = 1, image = bytes };
            }
            catch
            {
                return new { result = -1, message = "内部错误" };
            }
        }
        [HttpGet]
        public dynamic GetHeadImage(string username)
        {
            try
            {
                if (username.IsNullOrEmpty())
                {
                    return new { result = -1, message = "内部错误" };
                }
                var user = context.UserInfo.Where(x => x.EmployeeID == username || x.MobilephoneNumber == username).FirstOrDefault();
                if (user != null)
                {
                    return new { result = 1, image = HeadImageHelper.GetHeadImage(user.UserID) };
                }
                return new { result = -2, message = "该用户不存在" };
            }
            catch
            {
                return new { result = -1, message = "内部错误" };
            }
        }


        [HttpGet]
        public async Task<dynamic> SendSMS(string checkcode, string phonenumber)
        {
            try
            {
                if (checkcode.IsNullOrEmpty() || phonenumber.IsNullOrEmpty())
                {
                    return new { result = -1, message = "内部错误" };
                }
                if (HttpContext.Session.TryGetValue("checkcode", out var value) && value.ConvertFromBytes(Encoding.ASCII) == checkcode.ToLower())
                {
                    var users = context.UserInfo.Where(x => (x.MobilephoneNumber == phonenumber));
                    if (users.Count() == 0)
                    {
                        return new { result = -3, message = "不存在该用户" };
                    }
                    var user = users.First();
                    var userid = user.UserID;
                    HttpContext.Session.Remove("checkcode");
                    if (user.SendSMSDate.Date != DateTime.Now.Date)
                    {
                        user.SendSMSTimes = 0;
                    }
                    if (user.SendSMSTimes >= 3)
                    {
                        return new { result = -4, message = "你今天发送的短信过多，请明天再试" };
                    }
                    user.SendSMSDate = DateTime.Now;
                    user.SendSMSTimes++;
                    context.SaveChanges();

                    var code = await SMSHelper.SendCheckCode(phonenumber);
                    if (code != 0)
                    {
                        var currenttime = DateTime.Now.Ticks;
                        var data = new byte[] { (byte)(code >> 8), (byte)code, (byte)(currenttime >> 56), (byte)(currenttime >> 48), (byte)(currenttime >> 40), (byte)(currenttime >> 32), (byte)(currenttime >> 24), (byte)(currenttime >> 16), (byte)(currenttime >> 8), (byte)currenttime, (byte)(userid >> 24), (byte)(userid >> 16), (byte)(userid >> 8), (byte)(userid) };
                        HttpContext.Session.Set("smscode", data);
                        return new { result = 1 };
                    }
                    else
                    {
                        return new { result = -5, message = "发送短信失败" };
                    }

                }
                else
                {
                    return new { result = -2, message = "验证码错误" };
                }
            }
            catch
            {
                return new { result = -1, message = "内部错误" };
            }
        }

        [HttpGet]
        public dynamic UpdatePassword(string code, string newpass)
        {
            try
            {
                if (code.IsNullOrEmpty() || newpass.IsNullOrEmpty())
                {
                    return new { result = -1, message = "内部错误" };
                }
                if (HttpContext.Session.TryGetValue("smscode", out var value) && ((value[0] << 8) + value[1]).ToString() == code)
                {
                    HttpContext.Session.Remove("smscode");
                    var oldtime = ((long)value[2] << 56) + ((long)value[3] << 48) + ((long)value[4] << 40) + ((long)value[5] << 32) + ((long)value[6] << 24) + ((long)value[7] << 16) + ((long)value[8] << 8) + ((long)value[9]);
                    if ((DateTime.Now.Ticks - oldtime) / 10000000 < 180)
                    {
                        var userid = (value[10] << 24) + (value[11] << 16) + (value[12] << 8) + value[13];
                        var user = context.UserInfo.Where(x => x.UserID == userid).First();
                        user.Password = newpass;
                        context.SaveChanges();
                        return new { result = 1 };

                    }
                    else
                    {
                        return new { result = -3, message = "验证码超时" };
                    }
                }
                else
                {
                    return new { result = -2, message = "验证码错误" };
                }
            }
            catch
            {
                return new { result = -1, message = "内部错误" };
            }
        }

        [HttpGet]
        public dynamic UpdateSession(string username, string deviceid, string token)
        {
            try
            {
                if (username.IsNullOrEmpty() || token.IsNullOrEmpty() || deviceid.IsNullOrEmpty())
                {
                    return new { result = -1, message = "内部错误" };
                }
                var user = context.UserInfo.Where(x => x.EmployeeID == username || x.MobilephoneNumber == username).FirstOrDefault();
                var result = user != null;
                if (!result)
                {
                    return new { result = 0, message = "状态异常" };
                }
                else
                {
                    result = user.Token == token;
                    if (!result)
                    {
                        return new { result = 0, message = "状态异常" };
                    }
                    else
                    {
                        var id = user.UserID;
                        HttpContext.Session.Set("userid", new byte[] { (byte)(id >> 24), (byte)(id >> 16), (byte)(id >> 8), (byte)id });
                        return new { result = 1 };
                    }
                }
            }
            catch
            {
                return new { result = -1, message = "内部错误" };
            }

        }

        [HttpGet]
        public dynamic GetUserInfo()
        {
            try
            {
                if (HttpContext.Session.TryGetValue("userid", out var value))
                {
                    var userid = (value[0] << 24) + (value[1] << 16) + (value[2] << 8) + value[3];
                    var user = context.UserInfo.Where(x => x.UserID == userid).FirstOrDefault();
                    if (user != null)
                    {
                        return new
                        {
                            result = 1,
                            employeeid = user.EmployeeID,
                            name = user.Name,
                            departmentname = context.DepartmentInfo.Where(x => x.DepartmentID == user.DepartmentID).FirstOrDefault()?.DepartmentName ?? "",
                            phonenumber = user.MobilephoneNumber,
                            email = user.Email,
                            headimage = HeadImageHelper.GetHeadImage(userid)
                        };
                    }
                }
                return new { result = 0, message = "Session异常" };

            }
            catch
            {
                return new { result = -1, message = "内部错误" };
            }
        }

        [HttpPost]
        public dynamic UpdateUserInfo(string employeeid, string name, int departmentid, string phonenumber, string email, IFormFile headimage)
        {
            try
            {
                if (employeeid.IsNullOrEmpty() || name.IsNullOrEmpty() || phonenumber.IsNullOrEmpty() || email.IsNullOrEmpty() || context.DepartmentInfo.Where(x => x.DepartmentID == departmentid).Count() == 0)
                {
                    return new { result = -1, message = "内部错误" };
                }
                if (HttpContext.Session.TryGetValue("userid", out var value))
                {
                    var userid = (value[0] << 24) + (value[1] << 16) + (value[2] << 8) + value[3];
                    byte[] buffer = null;
                    if (headimage != null)
                    {
                        var length = (int)headimage.Length;
                        buffer = new byte[length];
                        headimage.OpenReadStream().Read(buffer, 0, length);
                    }


                    var user = context.UserInfo.Where(x => x.UserID == userid).FirstOrDefault();
                    if (user != null)
                    {
                        user.EmployeeID = employeeid;
                        user.Name = name;
                        user.DepartmentID = departmentid;
                        user.MobilephoneNumber = phonenumber;
                        user.Email = email;
                        // user.HeadImage = buffer;
                        context.SaveChanges();
                        HeadImageHelper.SaveHeadImage(userid, buffer);
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
    }
}
