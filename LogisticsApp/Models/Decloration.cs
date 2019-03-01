using LogisticsApp.Models.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LogisticsApp.Models
{
    public class Decloration
    {
        public int Id { get; set; }
        public int ItemCount { get; set; }
        public DateTime CreatedDate { get; set; }
        public string OrderedFrom { get; set; }
        public int CategoryId { get; set; }
        public string ApplicationUserId { get; set; }
        public string TrackNumber { get; set; }
        public double Price { get; set; }
        public string Invoice { get; set; }
        public string Description { get; set; }


        public virtual Category Category { get; set; }
        public virtual ApplicationUser customer { get; set; }

    }
}