using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CheckIn.Manager.Models
{
    public class UserModel
    {
        public int UserID
        {
            get;
            set;
        }
        [Required(ErrorMessage = "姓名不能为空。")]
        public string Name
        {
            get;
            set;
        }
        [Required(ErrorMessage = "工号不能为空。")]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "工号只能由数字组成。")]
        public string EmployeeID
        {
            get;
            set;
        }
        [Required(ErrorMessage = "手机号不能为空。")]
        [RegularExpression(@"^1[34578]\d{9}$", ErrorMessage = "手机号不合法。")]
        public string MobilephoneNumber
        {
            get;
            set;
        }
        [Required(ErrorMessage = "邮箱不能为空。")]
        public string Email
        {
            get;
            set;
        }
        [Required(ErrorMessage = "部门不能为空。")]
        public int DepartmentID
        {
            get;
            set;
        }
        [Required(ErrorMessage = "用户类型不能为空。")]
        public int Right
        {
            get;
            set;
        }
        public string Password
        {
            get;
            set;
        }

    }
}
