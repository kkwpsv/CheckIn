using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CheckIn.Common.Models;
using CheckIn.Common.Util;
using CheckIn.Manager.Models;
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
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Index(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var user = context.UserInfo.Where(x => x.EmployeeID == model.EmployeeID && x.Password == MD5Helper.PasswordMD5(model.Password)).FirstOrDefault();
                if (user != null && user.Right > 0)
                {
                    HttpContext.Session.Set("User", SessionHelper.ObjectToBytes(user));
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "用户名或密码错误。");
                return View(model);
            }
            return View();
        }
        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("User");
            return RedirectToAction("Index", "Login");
        }

    }
}