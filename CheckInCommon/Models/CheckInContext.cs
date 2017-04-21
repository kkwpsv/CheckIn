using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckIn.Common.Models
{
    public class CheckInContext : DbContext
    {
        public CheckInContext(DbContextOptions<CheckInContext> options) : base(options)
        {
        }
        public DbSet<DepartmentInfo> DepartmentInfo
        {
            get;
            set;
        }
        public DbSet<TelephoneInfo> TelephoneInfo
        {
            get;
            set;
        }
        public DbSet<UserCheckInInfo> UserCheckInInfo
        {
            get;
            set;
        }

        public DbSet<UserInfo> UserInfo
        {
            get;
            set;
        }
        public DbSet<LocationInfo> LocationInfo
        {
            get;
            set;
        }

    }
}
