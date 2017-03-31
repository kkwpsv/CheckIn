using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CheckIn.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace CheckInManager.Controllers
{
    [AllowAnonymous]
    public class LoginController :Controller
    {
        private CheckInContext context;
        public LoginController(CheckInContext context)
        {
            this.context = context;
        }
        [HttpGet]
        public IActionResult Index(string returnurl = null)
        {
            ViewData["ReturnUrl"] = returnurl;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password, string returnurl)
        {
            var users = context.UserInfo.Where(x => x.EmployeeID == username && x.Password == password);
            if (users.Count() > 0)
            {
                var user = users.First();
                if (user.Right > 0)
                {
                    List<Claim> claims = new List<Claim>
                    {
                        new Claim("Right", user.Right.ToString(), ClaimValueTypes.Integer32)
                    };
                    var identity = new ClaimsIdentity(claims);


                    await HttpContext.Authentication.SignInAsync("MyCookieMiddlewareInstance", new ClaimsPrincipal(identity),
                    new AuthenticationProperties
                    {
                        ExpiresUtc = DateTime.UtcNow.AddMinutes(20),
                    });
                    return LocalRedirect(returnurl);
                }
            }
            return Content(@"<script language=""javascript""> alert(""登录失败"");document.location.href=""/Index?returnurl=" + returnurl + @""";</script>");

        }
        [HttpGet]
        public IActionResult Denied(string returnurl = null)
        {
            return Content(@"<script language=""javascript""> alert(""你没有权限进行此操作"");window.location.href=document.referrer;"";</script>");
        }
    }
}