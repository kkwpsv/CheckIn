using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CheckIn.Common.Models;
using CheckIn.Common.Util;
using CheckIn.Manager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PagedList.Core;

namespace CheckIn.Manager.Controllers
{
    public class UsersController :ControllerWithAuthorize
    {
        public UsersController(CheckInContext context) : base(context)
        {
        }
        [HttpGet]
        public IActionResult Index(int page = 1)
        {
            ViewBag.DepartmentName = context.DepartmentInfo.ToDictionary(x => x.DepartmentID, x => x.DepartmentName);
            ViewBag.RightName = new Dictionary<int, string>
            {
                {0,"员工" },
                {1,"管理员" }
            };
            var list = context.UserInfo.Select(x => new UserModel
            {
                UserID = x.UserID,
                Name = x.Name,
                EmployeeID = x.EmployeeID,
                MobilephoneNumber = x.MobilephoneNumber,
                Email = x.Email,
                DepartmentID = x.DepartmentID,
                Right = x.Right
            }).ToPagedList(page > 0 ? page : 1, 20);
            return View(list);
        }
        [HttpGet]
        public IActionResult Delete(int UserID = 0)
        {
            var item = context.UserInfo.Where(x => x.UserID == UserID).FirstOrDefault();
            if (item != null)
            {
                context.UserInfo.Remove(item);
                context.SaveChanges();
                return Content("ok");
            }
            else
            {
                return Content("error");
            }
        }

        [HttpGet]
        public IActionResult Add()
        {
            var selectlist = context.DepartmentInfo.Select(x => new SelectListItem
            {
                Text = x.DepartmentName,
                Value = x.DepartmentID.ToString()
            }).ToList();
            ViewBag.SelectList = selectlist;
            var selectlist2 = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Text="管理员",
                    Value="1",
                },
                new SelectListItem
                {
                    Text="员工",
                    Value="0",
                    Selected=true,
                },
            };
            ViewBag.SelectList2 = selectlist2;
            return View();
        }
        [HttpPost]
        public IActionResult Add(UserModel model)
        {
            var selectlist = context.DepartmentInfo.Select(x => new SelectListItem
            {
                Text = x.DepartmentName,
                Value = x.DepartmentID.ToString()
            }).ToList();
            ViewBag.SelectList = selectlist;
            var selectlist2 = new List<SelectListItem>
                    {
                        new SelectListItem
                        {
                            Text="管理员",
                            Value="1",
                        },
                        new SelectListItem
                        {
                            Text="员工",
                            Value="0",
                            Selected=true,
                        },
                    };
            ViewBag.SelectList2 = selectlist2;
            if (ModelState.IsValid)
            {
                if (model.Password == null || model.Password.Length < 6)
                {

                    ModelState.AddModelError("", "密码长度过短，最短为6字符。");
                    return View(model);
                }
                else if (context.UserInfo.Where(x => x.MobilephoneNumber == model.MobilephoneNumber).Count() > 0)
                {
                    ModelState.AddModelError("", "该手机号码已经被使用。");
                    return View(model);
                }
                else
                {
                    context.UserInfo.Add(new UserInfo
                    {
                        Name = model.Name,
                        Password = MD5Helper.PasswordMD5(model.Password),
                        EmployeeID = model.EmployeeID,
                        MobilephoneNumber = model.MobilephoneNumber,
                        Email = model.Email,
                        DepartmentID = model.DepartmentID,
                        Right = model.Right,
                    });
                    context.SaveChanges();
                    return Content("<script>alert('添加成功。');window.location.href='/Users'</script>", "text/html; charset=utf-8");
                }
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult Edit(int UserID = 0)
        {

            var item = context.UserInfo.Where(x => x.UserID == UserID).Select(x => new UserModel
            {
                UserID = x.UserID,
                Name = x.Name,
                EmployeeID = x.EmployeeID,
                MobilephoneNumber = x.MobilephoneNumber,
                Email = x.Email,
                DepartmentID = x.DepartmentID,
                Right = x.Right
            }).FirstOrDefault();
            if (item != null)
            {
                var selectlist = context.DepartmentInfo.Select(x => new SelectListItem
                {
                    Text = x.DepartmentName,
                    Value = x.DepartmentID.ToString()
                }).ToList();
                ViewBag.SelectList = selectlist;
                var selectlist2 = new List<SelectListItem>
                {
                    new SelectListItem
                    {
                        Text="管理员",
                        Value="1",
                    },
                    new SelectListItem
                    {
                        Text="员工",
                        Value="0",
                    },
                };
                ViewBag.SelectList2 = selectlist2;
                return View(item);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult Edit(UserModel model)
        {
            var selectlist = context.DepartmentInfo.Select(x => new SelectListItem
            {
                Text = x.DepartmentName,
                Value = x.DepartmentID.ToString()
            }).ToList();
            ViewBag.SelectList = selectlist;
            var selectlist2 = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Text="管理员",
                    Value="1",
                },
                new SelectListItem
                {
                    Text="员工",
                    Value="0",
                },
            };
            ViewBag.SelectList2 = selectlist2;
            if (ModelState.IsValid)
            {
                {
                    if (model.Password != null && model.Password.Length < 6)
                    {

                        ModelState.AddModelError("", "密码长度过短，最短为6字符。");
                        return View(model);
                    }
                    else if (context.UserInfo.Where(x => x.MobilephoneNumber == model.MobilephoneNumber && x.UserID != model.UserID).Count() > 0)
                    {
                        ModelState.AddModelError("", "该手机号码已经被使用。");
                        return View(model);
                    }
                    else
                    {
                        var item = context.UserInfo.Where(a => a.UserID == model.UserID).FirstOrDefault();
                        if (item != null)
                        {
                            item.Name = model.Name;
                            item.EmployeeID = model.EmployeeID;
                            item.MobilephoneNumber = model.MobilephoneNumber;
                            item.Email = model.Email;
                            item.DepartmentID = model.DepartmentID;
                            item.Right = model.Right;
                            if (model.Password != null)
                            {
                                item.Password = MD5Helper.PasswordMD5(model.Password);
                            }
                            context.SaveChanges();
                            return Content("<script>alert('编辑成功。');window.location.href='/Users'</script>", "text/html; charset=utf-8");
                        }
                        else
                        {
                            return Content("<script>alert('内部错误。');window.location.href='/Users'</script>", "text/html; charset=utf-8");
                        }
                    }
                }
            }
            return View(model);
        }
    }
}
