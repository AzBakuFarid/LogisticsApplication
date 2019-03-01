using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LogisticsApp.Models
{
    public enum BundleStatus
    {
        Ordered = 1,
        InForeignWarehouses,
        OnTheWay,
        Arrived,
        Delivered
    }
}