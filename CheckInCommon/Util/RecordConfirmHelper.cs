using System;
using System.Collections.Generic;
using System.Text;
using CheckIn.Common.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CheckIn.Common.Util
{
    public static class RecordConfirmHelper
    {
        public static List<UserCheckInInfo> GetAbnormalRecords(CheckInContext context)
        {
            var records = context.UserCheckInInfo;
        }
    }
}
