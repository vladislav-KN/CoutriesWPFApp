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
using System.Data.SqlClient;

namespace CoutriesWPFApp
{
 
    public partial class MainWindow : Window
    {
        CountryContext DataBase;//база данных
        List<Country> SearchResults = new List<Country>();//найденные страны
        string urlApi = "https://restcountries.eu/rest/v2/name/";//ссылка на API

        public MainWindow()
        {
            ChangeConString();
            InitializeComponent();
            Add.IsEnabled = false;
        }

        #region ChangeEntity
        //Функции для добавления регионов и столиц 
        //На вход подаём добавляемую страну.
        //Данные заносятся в бд в том случае если они не существуют.
        City AddCity(Country SearchResult)
        {
            var city = DataBase.Cities.Where(x => x.CityName == SearchResult.City.CityName)
                    .FirstOrDefault();
            if (city is null)
            {
                city = SearchResult.City;
                DataBase.Cities.Add(city);
                DataBase.SaveChanges();
            }
 
            return city;
        }
        Region AddRegion(Country SearchResult)
        {
            var region = DataBase.Regions.Where(x => x.RegionName == SearchResult.Region.RegionName)
                             .FirstOrDefault();
            if (region is null)
            {
                region = SearchResult.Region;
                DataBase.Regions.Add(region);
                DataBase.SaveChanges();
            }
            return region;
        }
        //Функции для изменения столиц 
        //На вход подаём новые и старые данные о стране.
        //Данные изменяются только если названия стран отличается
        City EditCity(Country SearchResult, Country country)
        {
            var city = DataBase.Cities.Where(x => x.Id == country.CapitalID)
                    .FirstOrDefault();
            if (city != null)
            {
                if (city.CityName!= SearchResult.City.CityName)
                {
                    city.CityName = SearchResult.City.CityName;
                    DataBase.Entry(city).State = EntityState.Modified;
                    DataBase.SaveChanges();
                }
            }

            return city;
        }

        //Функции для изменения регионов.
        //На вход подаём новые и старые данные о стране.
        //Название регионов не меняется.
        //На выходе либо получаем новый регион, либо меняем на существующий, либо не меняем вовсе.
        Region EditRegion(Country SearchResult, Country country)
        {
            var region = DataBase.Regions.Where(x => x.Id == country.RegionID)
                             .FirstOrDefault();
            if (region != null)
            {
                if (region.RegionName != SearchResult.Region.RegionName)
                {
                    region = DataBase.Regions.Where(x => x.RegionName == SearchResult.Region.RegionName)
                             .FirstOrDefault();
                    if(region is null)
                    {
                        region = SearchResult.Region;
                        DataBase.Regions.Add(region);
                        DataBase.SaveChanges();
                    }
                }
            }
            return region;
        }
        //Функция для добавления новых стран в бд.
        void AddCountry()
        {
            try
            {
                //Для каждой страны проверим её существование в базе данных
                //Если страны не существует то создаём новую иначе меняем данные.
                foreach (Country SearchResult in SearchResults) 
                {
                    var country = DataBase.Countries.Where(x => x.CountryName == SearchResult.CountryName)
                                                    .FirstOrDefault();
                    if (country is null)
                    {
                        City city = AddCity(SearchResult);
                        Region region = AddRegion(SearchResult);
                        country = new Country()
                        {
                            CountryName = SearchResult.CountryName,
                            CountryID = SearchResult.CountryID,
                            CapitalID = city.Id,
                            City = city,
                            Region = region,
                            RegionID = region.Id,
                            Squere = SearchResult.Squere,
                            Population = SearchResult.Population
                        };
                        DataBase.Countries.Add(country);  
                    }
                    else 
                    {
                        country = DataBase.Countries.Where(x => x.Id == country.Id)
                                                    .Include(x => x.Region)
                                                    .Include(x => x.City)
                                                    .FirstOrDefault();
                        if (SearchResult.City != null)
                        {
                            country.City = EditCity(SearchResult, country);
                            country.CapitalID = country.City.Id;
                        }
                        if (SearchResult.Region != null)
                        {
                            country.Region = EditRegion(SearchResult, country);
                            country.RegionID = country.Region.Id;
                        }
                        country.Squere = SearchResult.Squere;
                        country.CountryName = SearchResult.CountryName;
                        country.Population = SearchResult.Population;
                        country.CountryID = SearchResult.CountryID;
                        DataBase.Entry(country).State = EntityState.Modified;
                    }
                    DataBase.SaveChanges();
                }
           
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        //Функция для поиска страны
        //На вход подаётся поисковой запрос
        //На выход в переменную SearchResults заносятся данные, если такие есть
        void SearchData(string search, out bool isSuccess)
        {
            
            List<CountriesApi> results = ApiRequest.CallWebAPi<List<CountriesApi>>(
                       new Uri(
                           urlApi + search,
                           UriKind.RelativeOrAbsolute),
                       out isSuccess);
            if (isSuccess && results != null && results.Count > 0)
            {
                foreach (CountriesApi result in results) {
                    SearchResults.Add(new Country()
                    {
                        CountryName = result.name,
                        CountryID = result.alpha3Code,
                        City = new City() { CityName = result.capital },
                        Region = new Region() { RegionName = result.region },
                        Squere = result.area,
                        Population = result.population

                    }); 
                }
                Add.IsEnabled = true;
            }
            else 
            {
                MessageBox.Show($"По запросу {search} данных нет!");
            }
 
        }
        //Функция для изменения строки подключения для Entity framework
        void ChangeConString()
        {
            string old = "";
            //запоминаем старую строку
            if (DataBase != null)
                old = DataBase.Database.Connection.ConnectionString;
            //меняем
            DataBase = new CountryContext(ConnectionString.Get());
            //проверяем
            try
            {
                
                DataBase.Database.Initialize(false);
            }
            catch (Exception ex)
            {
               //если не получилось возвращаем обратно и выводим сообщение об ошибке
                MessageBox.Show(ex.Message);
                if (old != "")
                {
                    ConnectionString.Set(old);
                    DataBase = new CountryContext(old);
                }
                else
                {
                    MessageBox.Show("Ошибка в строке подключения файла AppSetting.json.");
                }
            }
        }
        #endregion

        #region ChangeInterface
        //Функция отображения данных в DataGrid
        void ShowData(List<Country> countries)
        {
            ShowDataGrid.Items.Clear();
            foreach (Country item in countries)
                ShowDataGrid.Items.Add(item);
        }
        #endregion

        #region Events
        //Обработки нажатий на кнопки
        //Добавление в базу данных
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            Add.IsEnabled = false;
            if (SearchResults != null)
            {
                AddCountry();
            }
            else
            {
                //если данные отсутствуют то осуществим поиск
                if (!string.IsNullOrEmpty(SearchBox.Text))
                {
                    bool isSuccess;
                    SearchData(SearchBox.Text, out isSuccess);
                    if (isSuccess)
                    {             
                        AddCountry();
                    }
                }
            }
        }
        //Поиск
        private void Search_Click(object sender, RoutedEventArgs e)
        {
            bool isSuccess;
            Add.IsEnabled = false;
            if (!string.IsNullOrEmpty(SearchBox.Text))
            {
                SearchResults = new List<Country>();
                SearchData(SearchBox.Text, out isSuccess);
                if (isSuccess)
                {
                    ShowData(SearchResults);
                }
            }
        }
        //Отобразить все данные
        private void ShowAll_Click(object sender, RoutedEventArgs e)
        {
            Add.IsEnabled = false;
            ShowData(DataBase.Countries.Include(x=>x.City).Include(x=>x.Region).ToList<Country>());
        }
        //Изменить строку подключения
        private void ChangeConStr_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(ConStr.Text))
            {
                //проверяем возможность подключением
                try
                {
                    var con = new SqlConnectionStringBuilder(ConStr.Text);
                    ConnectionString.Set(ConStr.Text);
                    ChangeConString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
       
    }
    #endregion
}
