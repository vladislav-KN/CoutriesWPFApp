using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Text.Json;

namespace CoutriesWPFApp.ConfigurationEF
{
    //Классы для файла AppSettings.json
    public class ConnectionStr
    {
        public string Countries { get; set; } 
    }
    public class AppSettings
    {
        public ConnectionStr ConnectionStrings { get; set; }
    }
    //Класс для настройки строки подключения.
    //Использует предыдущие 2 класса для записи новой строки.
    public static class ConnectionString
    {
        public static IConfiguration Configuration { get; private set; }

        //Функция для изменения строки подключения в файле AppSettings.json
        //На вход подаётся новая строка подключения
        public static void Set(string Constr)
        {
            ConnectionStr constr = new ConnectionStr() { Countries = Constr };
            AppSettings appSettings = new AppSettings() { ConnectionStrings = constr };
            var serializeOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
            string json = JsonSerializer.Serialize(appSettings, serializeOptions);
            File.WriteAllText(Directory.GetCurrentDirectory() + @"\AppSetting.json", json);


        }
        //получение строки подключения из файла AppSettings.json
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