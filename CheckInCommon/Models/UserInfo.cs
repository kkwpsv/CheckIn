﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CheckIn.Common.Models
{
    public class UserInfo
    {
        [MaxLength(11)]
        [Key]
        public int UserID
        {
            get;
            set;
        }
        [MaxLength(20)]
        public string Name
        {
            get;
            set;
        }
        [MaxLength(11)]
        public string EmployeeID
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
        [MaxLength(30)]
        public string Email
        {
            get;
            set;
        }
        [MaxLength(32)]
        public string Password
        {
            get;
            set;
        }
        [MaxLength(100)]

        public string Token
        {
            get;
            set;
        }
        [MaxLength(11)]
        public string MobilephoneNumber
        {
            get;
            set;
        }

        public DateTime SendSMSDate
        {
            get;
            set;
        }
        public int SendSMSTimes
        {
            get;
            set;
        }
        public int Right
        {
            get;
            set;
        }


    }
}
