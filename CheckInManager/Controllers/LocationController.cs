using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CheckIn.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace CheckIn.Manager.Controllers
{
    public class LocationController :ControllerWithAuthorize
    {
        public LocationController(CheckInContext context) : base(context)
        {
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
