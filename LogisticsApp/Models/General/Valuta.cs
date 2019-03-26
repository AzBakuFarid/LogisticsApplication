using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LogisticsApp.Models.General
{
    public class Valuta
    {
        public int Id { get; set; }
        public string Name  { get; set; }
        public double Value { get; set; }
        public bool isActive { get; set; }

        public virtual ICollection<Order> orders { get; set; }
        public virtual ICollection<Bundle> bundles { get; set; }

        public double getPriceInManat(double d) {
            return Math.Round(d * Value, 2);
        }

    }
}