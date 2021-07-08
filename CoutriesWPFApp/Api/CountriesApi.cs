using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Text.Json.Serialization;

namespace CoutriesWPFApp.Api
{
    //Класс для десериализации JSON файла из API
    public class CountriesApi
    {
        public string name { get; set; }
        public string alpha3Code { get; set; }
        public string capital { get; set; }
        public string region { get; set; }
        [DefaultValue(0)]
        public int population { get; set; }
        [DefaultValue(0)]
        public double area { get; set; }
        
    }
}
