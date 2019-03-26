using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LogisticsApp.Models.General
{
    public class Status
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool isCurrent { get; set; }

        public virtual ICollection<Order> orders { get; set; } // bunu silecem
        public virtual ICollection<Bundle> bundles { get; set; }
    }
}