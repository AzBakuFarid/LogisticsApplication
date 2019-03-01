using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LogisticsApp.Models.Page
{
    public class FAQ
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public bool isActive { get; set; }
    }
}