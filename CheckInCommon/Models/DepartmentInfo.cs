using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CheckIn.Common.Models
{
    public class DepartmentInfo
    {
        [MaxLength(11)]
        [Key]
        public int DepartmentID
        {
            get;
            set;
        }
        [MaxLength(40)]
        public string DepartmentName
        {
            get;
            set;
        }

    }
}
