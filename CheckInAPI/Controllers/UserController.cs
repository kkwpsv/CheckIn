using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CheckIn.Common.Models;

namespace CheckIn.API.Controllers
{
    [Route("[controller]/[action]")]
    public class UserController : Controller
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
                return new { result = false };
            }
            var userinfos = context.UserInfo.Where(x => (x.EmployeeID == username && x.Password == password) || (x.MobilephoneNumber == username && x.Password == password));
            var result = userinfos.Count() > 0;
            if (!result)
            {
                return new { result = result };
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
