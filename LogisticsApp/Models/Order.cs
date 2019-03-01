using LogisticsApp.Models.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LogisticsApp.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string Link { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool isPaid { get; set; }
        public bool isUrgent { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }

        public int CategoryId { get; set; }
        public string ApplicationUserId { get; set; }
        public int CountryId { get; set; }

        public virtual ApplicationUser customer { get; set; }
        public virtual Category category { get; set; }
        public virtual Country country { get; set; }
    }
}