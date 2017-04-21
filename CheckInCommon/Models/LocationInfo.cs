using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CheckIn.Common.Models
{
    public class LocationInfo
    {
        [Key]
        public int LocationID
        {
            get;
            set;
        }
        public string LocationName
        {
            get;
            set;
        }
        public string Location
        {
            get;
            set;
        }


    }
}
