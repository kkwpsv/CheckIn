using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CheckIn.Manager.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "用户名不能为空。")]
        public string EmployeeID
        {
            get;
            set;
        }

        [Required(ErrorMessage = "密码不能为空。")]
        [DataType(DataType.Password)]
        public string Password
        {
            get;
            set;
        }

    }
}
