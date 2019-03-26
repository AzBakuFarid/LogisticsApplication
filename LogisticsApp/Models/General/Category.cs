using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LogisticsApp.Models.General
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        
        public virtual ICollection<Order> orders { get; set; }
        public virtual ICollection<Market> markets { get; set; }
        public virtual ICollection<Bundle> bundles { get; set; }

    }
}