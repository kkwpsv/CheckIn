using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CheckIn.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace CheckIn.API.Controllers
{
    [Route("[controller]/[action]")]
    public class NoticeController :Controller
    {
        private CheckInContext context;

        public NoticeController(CheckInContext context)
        {
            this.context = context;
        }
        [HttpGet]
        public dynamic GetAllNotices()
        {
            try
            {
                return new { result = 1, data = context.NoticeInfo };
            }
            catch
            {
                return new { result = -1, message = "内部错误" };
            }
        }

    }
}
