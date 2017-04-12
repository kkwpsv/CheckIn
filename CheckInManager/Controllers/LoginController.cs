using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CheckIn.Common.Models;
using CheckIn.Common.Util;
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
        public async Task<IActionResult> Login(string EmployeeID, string Password, string ReturnUrl)
        {
            var users = context.UserInfo.Where(x => x.EmployeeID == EmployeeID && x.Password == MD5Helper.PasswordMD5(Password));
            if (users.Count() > 0)
            {
                var user = users.First();
                if (user.Right > 0)
                {



                    await HttpContext.Authentication.SignInAsync("MyCookieMiddlewareInstance", new ClaimsPrincipal(),
                    new AuthenticationProperties
                    {
                        ExpiresUtc = DateTime.UtcNow.AddMinutes(20),
                    });
                    return LocalRedirect(ReturnUrl);
                }
            }
            return Content(@"<script>alert(""µÇÂ¼Ê§°Ü"");window.location.href=document.referrer;</script>", "text/html; charset=utf-8");

        }

    }
}