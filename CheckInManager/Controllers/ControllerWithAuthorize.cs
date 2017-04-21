using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CheckIn.Common.Models;
using CheckIn.Common.Util;
using CheckIn.Manager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CheckIn.Manager.Controllers
{
    public class ControllerWithAuthorize :Controller
    {
        public int NeedUserRight
        {
            get;
            protected set;
        } = 1;

        public sealed override void OnActionExecuting(ActionExecutingContext context)
        {
            try
            {
                if (context.HttpContext.Session.TryGetValue("User", out byte[] data))
                {
                    var user = SessionHelper.BytesToObject<UserInfo>(data);
                    if (user.Right >= NeedUserRight)
                    {
                        base.OnActionExecuting(context);
                    }
                    else
                    {
                        context.Result = new ContentResult
                        {
                            Content = @"<script>alert(""你的权限不足"");window.location.href=document.referrer;</script>",
                            ContentType = "text/html; charset=utf-8"
                        };
                    }
                }
                else
                {
                    context.Result = new RedirectResult("/Login");
                }
            }
            catch
            {
                context.Result = new RedirectResult("/Home/Error");
            }

        }
    }
}
