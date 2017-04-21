using System;
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
            var list = RecordConfirmHelper.GetAbnormalRecords(context).AsQueryable().ToPagedList(page > 0 ? page : 1, 20);
            return View(list);
        }
        public IActionResult Confirm(int CheckInID = 0)
        {
            var item = context.UserCheckInInfo.Where(x => x.CheckInID == CheckInID).FirstOrDefault();
            if (item != null)
            {
                context.UserCheckInInfo.Remove(item);
                context.SaveChanges();
                return Content("ok");
            }
            else
            {
                return Content("error");
            }
        }
    }
}
