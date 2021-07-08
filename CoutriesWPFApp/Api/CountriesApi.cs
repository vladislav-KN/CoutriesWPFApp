using System;
using System.Collections.Generic;
using System.Text;

namespace CoutriesWPFApp.Api
{
    public class Currency
    {
        public string code { get; set; } = "";
        public string name { get; set; } = "";
        public string symbol { get; set; } = "";
    }

    public class Language
    {
        public string iso639_1 { get; set; } = "";
        public string iso639_2 { get; set; } = "";
        public string name { get; set; } = "";
        public string nativeName { get; set; } = "";
    }

    public class Translations
    {
        public string de { get; set; } = "";
        public string es { get; set; } = "";
        public string fr { get; set; } = "";
        public string ja { get; set; } = "";
        public string it { get; set; } = "";
        public string br { get; set; } = "";
        public string pt { get; set; } = "";
        public string nl { get; set; } = "";
        public string hr { get; set; } = "";
        public string fa { get; set; } = "";
    }

    public class RegionalBloc
    {
        public string acronym { get; set; } = "";
        public string name { get; set; } = "";
        public List<string> otherAcronyms { get; set; } = new List<string>();
        public List<string> otherNames { get; set; } = new List<string>();
    }

    public class CountriesApi
    {
        public string name { get; set; } = "";
        public List<string> topLevelDomain { get; set; } = new List<string>();
        public string alpha2Code { get; set; } = "";
        public string alpha3Code { get; set; } = "";
        public List<string> callingCodes { get; set; } = new List<string>();
        public string capital { get; set; } = "";
        public List<string> altSpellings { get; set; } = new List<string>();
        public string region { get; set; } = "";
        public string subregion { get; set; } = "";
        public int population { get; set; } = default(int);
        public List<double> latlng { get; set; } = new List<double>();
        public string demonym { get; set; } = "";
        public double area { get; set; } = default(double);
        public double gini { get; set; } = default(double);
        public List<string> timezones { get; set; } = new List<string>();
        public List<string> borders { get; set; } = new List<string>();
        public string nativeName { get; set; } = "";
        public string numericCode { get; set; } = "";
        public List<Currency> currencies { get; set; } = new List<Currency>();
        public List<Language> languages { get; set; } = new List<Language>();
        public Translations translations { get; set; } = new Translations();
        public string flag { get; set; } = "";
        public List<RegionalBloc> regionalBlocs { get; set; } = new List<RegionalBloc>();
        public string cioc { get; set; } = "";
    }
}
