using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CheckIn.Common.Models
{
    public class TelephoneInfo
    {
        [MaxLength(11)]
        [Key]
        public int TelephoneID
        {
            get;
            set;
        }
        [MaxLength(11)]
        public int DepartmentID
        {
            get;
            set;
        }
        [MaxLength(50)]
        public string TelephoneSubordination
        {
            get;
            set;
        }
        [MaxLength(20)]
        public string TelephoneNumber
        {
            get;
            set;
        }
    }
}
