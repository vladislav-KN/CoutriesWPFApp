using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Entity;

namespace CoutriesWPFApp.Models
{
    class CountryContext : DbContext
    {
        public DbSet<City> Cities { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Region> Regions { get; set; }
    }
}
