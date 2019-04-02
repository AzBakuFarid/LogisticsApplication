using LogisticsApp.Models.General;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LogisticsApp.Models
{
    public class Order
    {
        public Order()
        {
            CreatedDate = DateTime.Now; 
        }
        public int Id { get; set; }
        public string Link { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public DateTime CreatedDate { get; set; }
        public int BundleId { get; set; }


        public bool isPaid { get; set; }
        public bool isUrgent { get; set; }
        public string Description { get; set; }
       

        public int CategoryId { get; set; }
        public string ApplicationUserId { get; set; }
        public int CountryId { get; set; }
        public int ValutaId { get; set; }

        public virtual ApplicationUser customer { get; set; }
        public virtual Category category { get; set; }
        public virtual Country country { get; set; }
        public virtual Valuta valuta { get; set; }
        public virtual ICollection<Status> Statuses { get; set; }

        public void addStatus(Status _status) {

            if (!Statuses.Any(a => a.Name.Equals(_status.Name))) {
                Statuses.Add(_status);
            }
        }
    }

    public class OrderViewModel {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public int Bundle { get; set; }
        public double Price { get; set; }
        public bool isPaid { get; set; }
        public bool isUrgent { get; set; }
        public string Link { get; set; }
        public string Description { get; set; }
        public string CreatedDate { get; set; }
        public Category Category { get; set; }
        public Valuta Valuta { get; set; }
        public ApplicationUser Customer { get; set; }
        public Country Country { get; set; }
    }

    public class OrderCreateModel : GeneralContentViewModel
    {
        [Required]
        public string[] Link { get; set; }
        [Required]
        public double[] Price { get; set; }
        [Required]
        public int[] Quantity { get; set; }
        [Required]
        public string[] Description { get; set; }
        [Required]
        public int[] CategoryId { get; set; }
        [Required]
        public int[] ValutaId { get; set; }
        [Required]
        public bool isUrgent { get; set; }
        [Required]
        public int countryId { get; set; }

    }

}