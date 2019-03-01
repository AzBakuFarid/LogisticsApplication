using LogisticsApp.Models.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LogisticsApp.Models.Page
{
    public class Taariff
    {
        public int Id { get; set; }
        public int CountryId { get; set; }
        public string Weight { get; set; }
        public string Price { get; set; }
        public bool isActive { get; set; }

        public virtual Country country { get; set; }

    }
}