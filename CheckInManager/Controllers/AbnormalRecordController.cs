﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CheckIn.Common.Models;
using CheckIn.Common.Util;
using CheckIn.Manager.Models;
using CheckIn.Manager.Util;
using Microsoft.AspNetCore.Mvc;
using PagedList.Core;

namespace CheckIn.Manager.Controllers
{
    public class AbnormalRecordController :ControllerWithAuthorize
    {
        public AbnormalRecordController(CheckInContext context) : base(context)
        {
        }
        public IActionResult Index(int page = 1)
        {
            var list = RecordHelper.GetAbnormalRecords(context).ToPagedList(page > 0 ? page : 1, 20).AddAbnormalCause();
            return View(list);
        }
        public IActionResult Confirm(int? CheckInID)
        {
            if (CheckInID.HasValue)
            {
                var item = context.UserCheckInInfo.Where(x => x.CheckInID == CheckInID).FirstOrDefault();
                if (item != null)
                {
                    context.UserCheckInInfo.Remove(item);
                    context.SaveChanges();
                    return Content("ok");
                }
            }
            return Content("error");
        }
    }
}
