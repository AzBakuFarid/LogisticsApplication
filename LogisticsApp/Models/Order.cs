using LogisticsApp.Models.General;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LogisticsApp.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string Link { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }

        
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
    public class OrderViewModel :GeneralContentViewModel {
        public int Id { get; set; }
        [Required]
        public string Link { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        public int Quantity { get; set; }
        public bool isPaid { get; set; }
        public bool isUrgent { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public int ValutaId { get; set; }
    }
}