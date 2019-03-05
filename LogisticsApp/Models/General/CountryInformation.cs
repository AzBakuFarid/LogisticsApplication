using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LogisticsApp.Models.General
{
    public class CountryInformation
    {
         [ForeignKey("Country")]
        public int Id { get; set; }
       

        public string State { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string ZIPcode { get; set; }
        public string City { get; set; }
        public string PhoneNumber { get; set; }
        public string Semt { get; set; }
        public string Ilce { get; set; }
        public string IDNumber { get; set; }
        public string Addressheader { get; set; }
        public string TaxIDNumber { get; set; }
        public string Area { get; set; }

        public virtual Country Country { get; set; }
    }
}