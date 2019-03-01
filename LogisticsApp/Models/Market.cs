using LogisticsApp.Models.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LogisticsApp.Models
{
    public class Market
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        public string ImgPath { get; set; }
        public bool isActive { get; set; }
        

        public virtual ICollection<Country> countries { get; set; }
        public virtual ICollection<Category> categories { get; set; }


    }
}