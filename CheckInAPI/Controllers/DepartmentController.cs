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
    public class DepartmentController : Controller
    {
        private CheckInContext context;

        public DepartmentController(CheckInContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public dynamic GetAllDepartments()
        {
            try
            {
                return new { result = 1, data = context.DepartmentInfo };
            }
            catch
            {
                return new { result = -1, message = "内部错误" };
            }
        }
    }
}
