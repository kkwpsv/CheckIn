using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CheckIn.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace CheckIn.Manager.Controllers
{
    public class UsersController :ControllerWithAuthorize
    {
        public UsersController(CheckInContext context) : base(context)
        {
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
