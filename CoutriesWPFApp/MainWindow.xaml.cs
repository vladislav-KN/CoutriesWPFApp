using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CoutriesWPFApp.Models;
using CoutriesWPFApp.Api;
using CoutriesWPFApp.ConfigurationEF;
using System.Configuration;
using System.Data.Entity;

namespace CoutriesWPFApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        CountryContext DataBase;
        Country SearchResult;
        string urlApi = "https://restcountries.eu/rest/v2/name/";
        public MainWindow()
        {
            DataBase = new CountryContext(ConnectionString.Get());
            try
            {
                DataBase.Database.Initialize(false);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            InitializeComponent();
        }
        Country AddCity(Country country)
        {
            var city = DataBase.Cities.Where(x => x.Id == country.CapitalID)
                    .FirstOrDefault();
            if (city is null)
            {
                DataBase.Cities.Add(SearchResult.City);
                country.City = SearchResult.City;
                DataBase.SaveChanges();
            }
            else
            {
                DataBase.Entry(
                    city.CityName = SearchResult.City.CityName
                    ).State = EntityState.Modified;
                DataBase.SaveChanges();
            }
            return country;
        }
        Country AddRegion(Country country)
        {
            var region = DataBase.Regions.Where(x => x.Id == country.RegionID)
                             .FirstOrDefault();
            if (region is null)
            {
                DataBase.Regions.Add(SearchResult.Region);
                country.Region = SearchResult.Region;
                DataBase.SaveChanges();
            }
            else
            {
                DataBase.Entry(
                    region.RegionName = SearchResult.Region.RegionName
                    ).State = EntityState.Modified;
                DataBase.SaveChanges();
            }
            return country;
        }
        void AddCountry(out bool IsSucceed)
        {
            try
            {
                var country = DataBase.Countries.Where(x => x.CountryName == SearchResult.CountryName)
                                                .FirstOrDefault();
                if (country is null)
                {
                    country = new Country()
                    {
                        CountryName = SearchResult.CountryName,
                        CountryID = SearchResult.CountryID,
                        City = SearchResult.City,
                        Region = SearchResult.Region,
                        Squere = SearchResult.Squere,
                        Population = SearchResult.Population
                    };
                    DataBase.Countries.Add(country);
                    DataBase.SaveChanges();
                    IsSucceed = true;
                    return;
                }
                else
                {
                    country = DataBase.Countries.Where(x => x.Id == country.Id)
                                                .Include(x => x.Region)
                                                .Include(x => x.City)
                                                .FirstOrDefault();
                }
                if (SearchResult.City != null)
                    if (SearchResult.City.CityName != country.City.CityName)
                        AddCity(country);
                if (SearchResult.Region!=null)
                    if (SearchResult.Region.RegionName != country.Region.RegionName)
                        AddRegion(country);
                country.Squere = SearchResult.Squere;
                country.CountryName = SearchResult.CountryName;
                country.Population = SearchResult.Population;
                DataBase.SaveChanges();
                IsSucceed = true;
            }
            catch(Exception ex)
            {
                IsSucceed = false;
                //error message
            }
        }
        void Search(string search, out bool isSuccess)
        {
            List<CountriesApi> result = ApiRequest.CallWebAPi(
                       new Uri(
                           urlApi + search,
                           UriKind.RelativeOrAbsolute),
                       out isSuccess);
            if (isSuccess && result.Count > 0)
            {
                SearchResult = new Country()
                {
                    CountryName = result[0].name,
                    CountryID = result[0].alpha3Code,
                    City = new City() { CityName = result[0].capital },
                    Region = new Region() { RegionName = result[0].region },
                    Squere = result[0].area,
                    Population = result[0].population

                };
                AddCountry(out isSuccess);
                if (!isSuccess)
                {
                    //error message
                }
            }
            else
            {
                //errormessage
            }
        }
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            bool isSuccess;
            if (SearchResult != null)
            {
                AddCountry(out isSuccess);
                if (!isSuccess) 
                { 
                    //error message
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(SearchBox.Text))
                {
                    Search(SearchBox.Text, out isSuccess);
                    if (!isSuccess)
                    { 
                        //error message
                    }
                }
            }
        }

    }
}
