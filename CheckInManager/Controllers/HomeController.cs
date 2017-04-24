using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CheckIn.Common.Models;
using CheckIn.Common.Util;
using CheckIn.Manager.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CheckIn.Manager.Controllers
{
    public class HomeController :ControllerWithAuthorize
    {
        public HomeController(CheckInContext context) : base(context)
        {
        }

        public IActionResult Index()
        {
            ViewData["AbnormalRecordCount"] = RecordHelper.GetAbnormalRecords(context).Count();
            return View();
        }

        //public IActionResult About()
        //{
        //    ViewData["Message"] = "Your application description page.";

        //    return View();
        //}

        //public IActionResult Contact()
        //{
        //    ViewData["Message"] = "Your contact page.";

        //    return View();
        //}

        public IActionResult Error()
        {
            return View();
        }
    }
}
