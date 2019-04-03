using LogisticsApp.Models.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LogisticsApp.Models
{
    public class Settings
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Logo { get; set; }
        public int ServiceTax { get; set; }
        public int UrgencyTax { get; set; }
    }

    public class SettingsViewModel {

        public List<Valuta> valutas { get; set; }

    }
}