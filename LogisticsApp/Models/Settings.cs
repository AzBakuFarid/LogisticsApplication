using LogisticsApp.Models.General;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

    public class SettingsEditModel {

        public double[] valuta { get; set; }

        [Required(ErrorMessage ="Title is required")]
        public string Title { get; set; }

        [DataType(DataType.Upload)]
        public HttpPostedFileBase Logo { get; set; }

        [Range(0,100)]
        [Required(ErrorMessage = "Service tax is required")]
        [Display(Name ="Tax % for service")]
        public int ServiceTax { get; set; }

        [Range(0, 100)]
        [Required(ErrorMessage = "Tax for urgency is required")]
        [Display(Name = "Tax % for urgent ordering")]
        public int UrgencyTax { get; set; }

    }

    public class SettingsViewModel {
        public List<Valuta> valutas { get; set; }


        public string Title { get; set; }

        [Url]
        [DataType(DataType.ImageUrl)]
        public string Logo { get; set; }

        [Range(0, 100)]
        [Display(Name = "Tax % for service")]
        public int ServiceTax { get; set; }

        [Range(0, 100)]
        [Display(Name = "Tax % for urgent ordering")]
        public int UrgencyTax { get; set; }
    }
}