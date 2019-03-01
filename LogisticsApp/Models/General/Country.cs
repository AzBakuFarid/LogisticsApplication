using LogisticsApp.Models.Page;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LogisticsApp.Models.General
{
    public class Country
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string imagePath { get; set; }
        public bool isActive { get; set; }


        public virtual ICollection<Order> orders { get; set; }
        public virtual ICollection<Taariff> Taariffs { get; set; }
        public virtual ICollection<Market> markets { get; set; }
        public virtual ICollection<CountryInformation> countryInformations { get; set; }
    }
}