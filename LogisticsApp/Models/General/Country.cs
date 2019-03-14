using LogisticsApp.Models.Page;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LogisticsApp.Models.General
{
    public class Country
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string imagePath { get; set; }
        public bool isActive { get; set; }


        public virtual ICollection<Order> orders { get; set; }
        public virtual ICollection<Taariff> Taariffs { get; set; }
        public virtual ICollection<Market> markets { get; set; }
        public virtual CountryInformation countryInformation { get; set; }
    }

    public class CountryViewModel {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Display(Name="Country flag")]
        public HttpPostedFileBase File { get; set; }
        public string imagePath { get; set; }
        [Display(Name = "Is active")]
        public bool isActive { get; set; }
        public string State { get; set; }
        [Required]
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string ZIPcode { get; set; }
        [Required]
        public string City { get; set; }
        public string PhoneNumber { get; set; }
        public string Semt { get; set; }
        public string Ilce { get; set; }
        public string IDNumber { get; set; }
        public string Addressheader { get; set; }
        public string TaxIDNumber { get; set; }
        public string Area { get; set; }
    } 
}