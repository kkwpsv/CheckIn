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
    public class TelephoneController :Controller
    {
        private CheckInContext context;

        public TelephoneController(CheckInContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public dynamic GetAllTelephone()
        {
            try
            {
                var data = context.DepartmentInfo.ToDictionary(x => x.DepartmentID, x => x.DepartmentName);
                return new { result = 1, data = context.TelephoneInfo.Select(x => new { DepartmentName = data.ContainsKey(x.DepartmentID) ? data[x.DepartmentID] : "", TelephoneNumber = x.TelephoneNumber, TelephoneSubordination = x.TelephoneSubordination }) };
            }
            catch
            {
                return new { result = -1, message = "内部错误" };
            }
        }
    }
}
