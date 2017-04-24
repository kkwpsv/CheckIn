using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CheckIn.Common.Models;
using CheckIn.Common.Util;
using CheckIn.Manager.Models;
using CheckIn.Manager.Util;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Npoi.Core.HSSF.UserModel;
using Npoi.Core.HSSF.Util;
using Npoi.Core.XSSF.UserModel;
using PagedList.Core;

namespace CheckIn.Manager.Controllers
{
    public class RecordController :ControllerWithAuthorize
    {
        public RecordController(CheckInContext context) : base(context)
        {
        }
        public IActionResult Index(int page = 1, string EmployeeID = "", int DepartmentID = 0, DateTime? StartDate = null, DateTime? EndDate = null)
        {
            var list = RecordHelper.GetRecords(context).AsQueryable();
            var selectlist = context.DepartmentInfo.Select(x => new SelectListItem
            {
                Text = x.DepartmentName,
                Value = x.DepartmentID.ToString()
            }).ToList();
            selectlist.Insert(0, new SelectListItem
            {
                Text = "全部部门",
                Value = "0",
                Selected = true
            });
            ViewBag.SelectList = selectlist;
            var RouteData = new Dictionary<string, string>();


            if (EmployeeID != null && EmployeeID != "")
            {
                list = list.Where(x => x.EmployeeID == EmployeeID);
                RouteData.Add("EmployeeID", EmployeeID);
            }
            if (DepartmentID != 0)
            {
                list = list.Where(x => x.DepartmentID == DepartmentID);
                RouteData.Add("DepartmentID", DepartmentID.ToString());
            }
            if (StartDate != null)
            {
                list = list.Where(x => x.CheckInTime >= StartDate);
                RouteData.Add("StartDate", StartDate?.ToString("yyyy-MM-dd"));
            }
            if (EndDate != null)
            {
                list = list.Where(x => x.CheckInTime <= EndDate);
                RouteData.Add("EndDate", EndDate?.ToString("yyyy-MM-dd"));
            }
            ViewBag.RouteData = RouteData;
            return View(list.ToPagedList(page > 0 ? page : 1, 20).AddAbnormalCause());
        }
        public IActionResult Export(string EmployeeID = "", int DepartmentID = 0, DateTime? StartDate = null, DateTime? EndDate = null)
        {
            var list = RecordHelper.GetRecords(context).AsQueryable();
            var selectlist = context.DepartmentInfo.Select(x => new SelectListItem
            {
                Text = x.DepartmentName,
                Value = x.DepartmentID.ToString()
            }).ToList();
            selectlist.Insert(0, new SelectListItem
            {
                Text = "全部部门",
                Value = "0",
                Selected = true
            });
            ViewBag.SelectList = selectlist;

            if (EmployeeID != null && EmployeeID != "")
            {
                list = list.Where(x => x.EmployeeID == EmployeeID);
            }
            if (DepartmentID != 0)
            {
                list = list.Where(x => x.DepartmentID == DepartmentID);
            }
            if (StartDate != null)
            {
                list = list.Where(x => x.CheckInTime >= StartDate);
            }
            if (EndDate != null)
            {
                list = list.Where(x => x.CheckInTime <= EndDate);
            }
            var result = list.ToList().AddAbnormalCause();

            var book = new HSSFWorkbook();
            var table = book.CreateSheet("签到记录");


            string[] heads = { "签到记录编号", "工号", "姓名", "部门", "签到时间", "签出时间", "原始签到时间", "原始签出时间", "签到时间修改原因", "签出时间修改原因", "是否异常", "异常原因" };
            var columncount = heads.Length;
            var head = table.CreateRow(0);
            for (int i = 0; i < columncount; i++)
            {
                var cell = head.CreateCell(i);
                cell.SetCellValue(heads[i]);
            }
            int a = 1;
            foreach (var item in result)
            {
                var row = table.CreateRow(a++);

                var cell = row.CreateCell(0);
                cell.SetCellValue(item.CheckInID);
                cell = row.CreateCell(1);
                cell.SetCellValue(item.EmployeeID);
                cell = row.CreateCell(2);
                cell.SetCellValue(item.Name);
                cell = row.CreateCell(3);
                cell.SetCellValue(item.DepartmentName);
                cell = row.CreateCell(4);
                cell.SetCellValue(item.CheckInTime.ToString("yyyy-MM-dd HH:mm:ss"));
                if (item.HasCheckOut)
                {
                    cell = row.CreateCell(5);
                    cell.SetCellValue(item.CheckOutTime.ToString("yyyy-MM-dd HH:mm:ss"));
                }
                cell = row.CreateCell(6);
                cell.SetCellValue(item.OriCheckInTime.ToString("yyyy-MM-dd HH:mm:ss"));
                if (item.HasCheckOut)
                {
                    cell = row.CreateCell(7);
                    cell.SetCellValue(item.OriCheckOutTime.ToString("yyyy-MM-dd HH:mm:ss"));
                }
                cell = row.CreateCell(8);
                cell.SetCellValue(item.Reason1);
                cell = row.CreateCell(9);
                cell.SetCellValue(item.Reason2);
                if (item.AbnormalCause != null)
                {
                    cell = row.CreateCell(10);
                    cell.SetCellValue("是");
                    cell = row.CreateCell(11);
                    cell.SetCellValue(item.AbnormalCause);
                }
                else
                {
                    cell = row.CreateCell(10);
                    cell.SetCellValue("否");
                }

            }
            var stream = new MemoryStream();
            book.Write(stream);
            stream.Seek(0, SeekOrigin.Begin);
            return File(stream, "application/ms-excel", "record.xls");
        }

    }
}
