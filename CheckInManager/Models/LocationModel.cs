using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CheckIn.Manager.Models
{
    public class LocationModel
    {
        public int LocationID
        {
            get;
            set;
        }
        [Required(ErrorMessage = "分公司名称不能为空。")]
        public string LocationName
        {
            get;
            set;
        }

        [Required(ErrorMessage = "坐标1不能为空。")]
        public string Location1
        {
            get;
            set;
        }
        [Required(ErrorMessage = "坐标2不能为空。")]
        public string Location2
        {
            get;
            set;
        }
        [Required(ErrorMessage = "坐标3不能为空。")]
        public string Location3
        {
            get;
            set;
        }
        [Required(ErrorMessage = "坐标4不能为空。")]
        public string Location4
        {
            get;
            set;
        }

    }
}
