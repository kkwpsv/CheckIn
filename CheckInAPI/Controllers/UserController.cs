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
            if (String.IsNullOrEmpty(username) || String.IsNullOrEmpty(password) || String.IsNullOrEmpty(deviceid))
            {
                return new { result = false, message = "内部错误" };
            }
            var userinfos = context.UserInfo.Where(x => (x.EmployeeID == username && x.Password == password) || (x.MobilephoneNumber == username && x.Password == password));
            var result = userinfos.Count() > 0;
            if (!result)
            {
                return new { result = result, message = "用户名或密码错误" };
            }
            else
            {
                var token = TokenHelper.BuildToken(deviceid);
                var user = userinfos.First();
                user.Token = token;
                context.SaveChanges();
                return new { result = result, token = token };
            }
        }

        [HttpGet]
        public FileStreamResult GetCheckCode()
        {
            var result = CheckCodeHelper.GetCheckCode();
            var image = result.image;
            image.Seek(0, SeekOrigin.Begin);
            HttpContext.Session.Set("checkcode", Encoding.ASCII.GetBytes(result.text.ToLower()));
            return new FileStreamResult(image, "image/jpeg");
        }

        [HttpGet]
        public dynamic UpdatePassword(string code, string newpass)
        {
            if (String.IsNullOrEmpty(code) || String.IsNullOrEmpty(newpass))
            {
                return new { result = false, message = "内部错误" };
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
                    return new { result = true };

                }
                else
                {
                    return new { result = false, message = "验证码超时" };
                }
            }
            else
            {
                return new { result = false, message = "验证码错误" };
            }
        }

        [HttpGet]
        public async Task<dynamic> SendSMS(string checkcode, string phonenumber)
        {
            if (String.IsNullOrEmpty(checkcode) || String.IsNullOrEmpty(phonenumber))
            {
                return new { result = false, message = "内部错误" };
            }
            if (HttpContext.Session.TryGetValue("checkcode", out var value) && Encoding.ASCII.GetString(value) == checkcode.ToLower())
            {
                var users = context.UserInfo.Where(x => (x.MobilephoneNumber == phonenumber));
                if (users.Count() == 0)
                {
                    return new { result = false, message = "不存在该用户" };
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
                    return new { result = false, message = "你今天发送的短信过多，请明天再试" };
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
                    return new { result = true };
                }
                else
                {
                    return new { result = false, message = "发送短信失败" };
                }

            }
            else
            {
                return new { result = false, message = "验证码错误" };
            }

        }




        //[HttpGet("{id}")]
        //public User Query(long id)
        //{
        //    return new User
        //    {
        //        JobNumber = 110,
        //        Name = "张三",
        //        Birthday = new DateTime()
        //    };
        //}
        //[HttpGet("{name}")]
        //public List<User> QueryList(string name)
        //{
        //    return new List<User>
        //    {
        //        new User
        //        {
        //            JobNumber = 110,
        //            Name = "张三",
        //            Birthday = new DateTime()
        //        },
        //        new User
        //        {
        //            JobNumber = 120,
        //            Name = "李四",
        //            Birthday = new DateTime()
        //        },
        //};
        //}


    }
}
