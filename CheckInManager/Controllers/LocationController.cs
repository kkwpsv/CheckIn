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
    public class LocationController :ControllerWithAuthorize
    {
        public LocationController(CheckInContext context) : base(context)
        {
        }
        [HttpGet]
        public IActionResult Index(int page = 1)
        {
            var list = context.LocationInfo.Select(x => new LocationModel
            {
                LocationID = x.LocationID,
                LocationName = x.LocationName,
                Location1 = x.Location.Split('|')[0],
                Location2 = x.Location.Split('|')[1],
                Location3 = x.Location.Split('|')[2],
                Location4 = x.Location.Split('|')[3],
            }).ToPagedList(page > 0 ? page : 1, 20);
            return View(list);
        }
        [HttpGet]
        public IActionResult Delete(int? LocationID)
        {
            if (LocationID.HasValue)
            {
                var item = context.LocationInfo.Where(x => x.LocationID == LocationID).FirstOrDefault();
                if (item != null)
                {
                    context.LocationInfo.Remove(item);
                    context.SaveChanges();
                    return Content("ok");
                }
            }

            return Content("error");

        }
        [HttpGet]
        public IActionResult Edit(int LocationID = 0)
        {
            var item = context.LocationInfo.Where(x => x.LocationID == LocationID).Select(x => new LocationModel
            {
                LocationID = x.LocationID,
                LocationName = x.LocationName,
                Location1 = x.Location.Split('|')[0],
                Location2 = x.Location.Split('|')[1],
                Location3 = x.Location.Split('|')[2],
                Location4 = x.Location.Split('|')[3],
            }).FirstOrDefault();
            if (item != null)
            {
                return View(item);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        public IActionResult Edit(LocationModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Location1.Split(',').Count() != 2 || !decimal.TryParse(model.Location1.Split(',')[0], out var x) || !decimal.TryParse(model.Location1.Split(',')[1], out x))
                {
                    ModelState.AddModelError("", "位置1不合法。");
                }
                else if (model.Location2.Split(',').Count() != 2 || !decimal.TryParse(model.Location2.Split(',')[0], out x) || !decimal.TryParse(model.Location2.Split(',')[1], out x))
                {
                    ModelState.AddModelError("", "位置2不合法。");
                }
                else if (model.Location3.Split(',').Count() != 2 || !decimal.TryParse(model.Location3.Split(',')[0], out x) || !decimal.TryParse(model.Location3.Split(',')[1], out x))
                {
                    ModelState.AddModelError("", "位置3不合法。");
                }
                else if (model.Location4.Split(',').Count() != 2 || !decimal.TryParse(model.Location4.Split(',')[0], out x) || !decimal.TryParse(model.Location4.Split(',')[1], out x))
                {
                    ModelState.AddModelError("", "位置4不合法。");
                }
                else
                {
                    var item = context.LocationInfo.Where(a => a.LocationID == model.LocationID).FirstOrDefault();
                    if (item != null)
                    {
                        item.LocationName = model.LocationName;
                        item.Location = model.Location1 + "|" + model.Location2 + "|" + model.Location3 + "|" + model.Location4;
                        context.SaveChanges();
                        return Content("<script>alert('编辑成功。');window.location.href='/Location'</script>", "text/html; charset=utf-8");
                    }
                    else
                    {
                        return Content("<script>alert('内部错误。');window.location.href='/Location'</script>", "text/html; charset=utf-8");
                    }
                }

                return View(model);
            }
            return View();
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Add(LocationModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Location1.Split(',').Count() != 2 || !decimal.TryParse(model.Location1.Split(',')[0], out var x) || !decimal.TryParse(model.Location1.Split(',')[1], out x))
                {
                    ModelState.AddModelError("", "位置1不合法。");
                }
                else if (model.Location2.Split(',').Count() != 2 || !decimal.TryParse(model.Location2.Split(',')[0], out x) || !decimal.TryParse(model.Location2.Split(',')[1], out x))
                {
                    ModelState.AddModelError("", "位置2不合法。");
                }
                else if (model.Location3.Split(',').Count() != 2 || !decimal.TryParse(model.Location3.Split(',')[0], out x) || !decimal.TryParse(model.Location3.Split(',')[1], out x))
                {
                    ModelState.AddModelError("", "位置3不合法。");
                }
                else if (model.Location4.Split(',').Count() != 2 || !decimal.TryParse(model.Location4.Split(',')[0], out x) || !decimal.TryParse(model.Location4.Split(',')[1], out x))
                {
                    ModelState.AddModelError("", "位置4不合法。");
                }
                else
                {
                    context.LocationInfo.Add(new LocationInfo
                    {
                        LocationName = model.LocationName,
                        Location = model.Location1 + "|" + model.Location2 + "|" + model.Location3 + "|" + model.Location4
                    });
                    context.SaveChanges();
                    return Content("<script>alert('添加成功。');window.location.href='/Location'</script>", "text/html; charset=utf-8");
                }
                return View(model);
            }
            return View();
        }
    }
}
