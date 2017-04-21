using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CheckIn.Common.Models;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Net.Http;
using System.Net.Http.Headers;
using CheckIn.API;
using CheckIn.Common.Util;
using Newtonsoft.Json;

namespace CheckIn.API.Controllers
{
    [Route("[controller]/[action]")]
    public class LocationController :Controller
    {
        private CheckInContext context;

        public LocationController(CheckInContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public dynamic GetAllLocation()
        {
            try
            {
                var data = context.LocationInfo;
                return new
                {
                    result = 1,
                    data = data.Select((x) =>
                    (
                        new
                        {
                            LocationID = x.LocationID,
                            LocationName = x.LocationName,
                            X1 = decimal.Parse(x.Location.Split('|')[0].Split(',')[0]),
                            X2 = decimal.Parse(x.Location.Split('|')[1].Split(',')[0]),
                            X3 = decimal.Parse(x.Location.Split('|')[2].Split(',')[0]),
                            X4 = decimal.Parse(x.Location.Split('|')[3].Split(',')[0]),
                            Y1 = decimal.Parse(x.Location.Split('|')[0].Split(',')[1]),
                            Y2 = decimal.Parse(x.Location.Split('|')[1].Split(',')[1]),
                            Y3 = decimal.Parse(x.Location.Split('|')[2].Split(',')[1]),
                            Y4 = decimal.Parse(x.Location.Split('|')[3].Split(',')[1]),
                        }
                    ))
                };
            }
            catch
            {
                return new { result = -1, message = "内部错误" };
            }
        }
    }
}
