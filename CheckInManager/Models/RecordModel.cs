using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckIn.Manager.Models
{
    public class RecordModel
    {
        public int CheckInID
        {
            get;
            set;
        }
        public string EmployeeID
        {
            get;
            set;
        }
        public int DepartmentID
        {
            get;
            set;
        }
        public string DepartmentName
        {
            get;
            set;
        }
        public string Name
        {
            get;
            set;
        }
        public DateTime CheckInTime
        {
            get;
            set;
        }
        public DateTime CheckOutTime
        {
            get;
            set;
        }
        public DateTime OriCheckInTime
        {
            get;
            set;
        }
        public DateTime OriCheckOutTime
        {
            get;
            set;
        }
        public string Reason1
        {
            get;
            set;
        }
        public string Reason2
        {
            get;
            set;
        }
        public string AbnormalCause
        {
            get;
            set;
        }
        public bool HasCheckOut
        {
            get;
            set;
        }
        public bool IsNormal
        {
            get;
            set;
        }
    }
}
