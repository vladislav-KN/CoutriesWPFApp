using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CoutriesWPFApp.Models
{
    class Country
    {
        [Key]
        public int Id { get; set; }
        public string CountryName { get; set; }
        public string CountryID { get; set; }
        public int CapitalID { get; set; }
        [ForeignKey("CapitalID")]
        public City City { get; set; }
        public float Squere { get; set; }
        public int RegionID { get; set; }
        [ForeignKey("RegionID")]
        public Region Region { get; set; }
    }
}
