using CheckIn.Common.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using MySQL.Data.EntityFrameworkCore.Extensions;


namespace CheckIn.Common
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {

            //services.AddDbContext<CheckInContext>(options => options.UseMySQL("server=tellyouwhat.cn;userid=root;pwd=db499759;port=3306;database=checkin;sslmode=none;"));

        }
    }
}
