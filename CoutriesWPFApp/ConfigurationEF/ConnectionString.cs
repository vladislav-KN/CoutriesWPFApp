using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.IO;
using System.Text;

namespace CoutriesWPFApp.ConfigurationEF
{

    public static class ConnectionString
    {
        public static IServiceProvider ServiceProvider { get; private set; }

        public static IConfiguration Configuration { get; private set; }
        public static void Set(string Constr)
        {
            var builder = new ConfigurationBuilder()
              .SetBasePath(Directory.GetCurrentDirectory())
              .AddJsonFile("AppSetting.json", optional: false, reloadOnChange: true);

            Configuration = builder.Build();

            Configuration["Countries"] = Constr;
        }

        public static string Get()
        {
            var builder = new ConfigurationBuilder()
              .SetBasePath(Directory.GetCurrentDirectory())
              .AddJsonFile("AppSetting.json", optional: false, reloadOnChange: true);

            Configuration = builder.Build();

            return Configuration.GetConnectionString("Countries");
        }
    }

}