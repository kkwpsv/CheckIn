using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CheckIn.Common.Models;

namespace CheckIn.Manager.Controllers
{
    public class AbnormalRecordController :ControllerWithAuthorize
    {
        public AbnormalRecordController(CheckInContext context) : base(context)
        {
        }
    }
}
