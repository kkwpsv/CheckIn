using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CheckIn.Common.Models;
using CheckIn.Manager.Models;
using Microsoft.AspNetCore.Mvc;
using PagedList.Core;

namespace CheckIn.Manager.Controllers
{
    public class NoticeController :ControllerWithAuthorize
    {
        public NoticeController(CheckInContext context) : base(context)
        {
        }
        [HttpGet]
        public IActionResult Index(int page = 1)
        {
            var list = context.NoticeInfo.Select(x => new NoticeModel
            {
                NoticeID = x.NoticeID,
                Author = x.Author,
                Title = x.Title,
                Content = x.Content,
                Time = x.Time
            }).ToPagedList(page > 0 ? page : 1, 20);
            return View(list);
        }
        [HttpGet]
        public IActionResult Delete(int? NoticeID)
        {
            if (NoticeID.HasValue)
            {
                var item = context.NoticeInfo.Where(x => x.NoticeID == NoticeID).FirstOrDefault();
                if (item != null)
                {
                    context.NoticeInfo.Remove(item);
                    context.SaveChanges();
                    return Content("ok");
                }
            }
            return Content("error");
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Add(NoticeModel model)
        {
            if (ModelState.IsValid)
            {

                context.NoticeInfo.Add(new NoticeInfo
                {
                    Author = this.user.Name,
                    Time = DateTime.Now,
                    Title = model.Title,
                    Content = model.Content,
                });
                context.SaveChanges();
                return Content("<script>alert('添加成功。');window.location.href='/Notice'</script>", "text/html; charset=utf-8");
            }
            return View();
        }
    }
}
