using LogisticsApp.Models.General;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LogisticsApp.Models
{
    public class Bundle
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public int BundleQuantity { get; set; }
        public string MarketName { get; set; }
        public string TrackNumber { get; set; }
        public double Price { get; set; }
        public string InvoiceFilePath { get; set; }
        public string Description { get; set; }
        public bool isPaid { get; set; }


        public int CategoryId { get; set; }
        public int CountryId { get; set; }
        public int ValutaId { get; set; }
        public string CustomerId { get; set; }

        public virtual Category Category { get; set; }
        public virtual Country Country { get; set; }
        public virtual Valuta Valuta { get; set; }
        public virtual ApplicationUser Customer { get; set; }
        public virtual ICollection<Status> Statuses { get; set; }

        public void addStatus(Status _status)
        {
            if (_status != null)
            {
                if (Statuses == null)
                {
                    Statuses = new List<Status>();
                    _status.isCurrent = true;
                    Statuses.Add(_status);
                    return;
                }
                if (!Statuses.Any(a => a.Name.Equals(_status.Name)))
                {
                    foreach (var item in Statuses)
                    {
                        item.isCurrent = false;
                    }
                    _status.isCurrent = true;
                    Statuses.Add(_status);
                }
            }
        }
        public void editStatus(Status _status) {
            if (Statuses != null&& _status != null) {
                Status st = Statuses.FirstOrDefault(f => f.Name.Equals(_status.Name));
                if (st != null) {
                    st.CreatedDate = _status.CreatedDate;
                }
            }
        }
        public void deleteStatus(Status _status)
        {
            if (Statuses != null && _status != null)
            {
                Status st = Statuses.FirstOrDefault(f => f.Name.Equals(_status.Name));
                if (st != null)
                {
                    Statuses.Remove(st);
                    ICollection<Status> temp = new List<Status>();
                    foreach (var item in Statuses)
                    {
                        if (item != null) {
                            temp.Add(item);
                        }
                    }
                    Statuses = temp;
                }
            }
        }
    }
    public class BundleCreateModel {

        public IEnumerable<int> Orders { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime OrderDate { get; set; }

        [Required]
        [Range(1,100)]
        public int BundleQuantity { get; set; }

        [Required]
        [DataType(DataType.Url)]
        public string MarketName { get; set; }

        [Required]
        public string TrackNumber { get; set; }

        [Required]
        [Range(0.1,double.MaxValue)]
        public double Price { get; set; }

        [Required]
        [DataType(DataType.Upload)]
        public HttpPostedFileBase Invoice { get; set; }

        public string Description { get; set; }

        [Required]
        public int Category { get; set; }

        [Required]
        public int Country { get; set; }

        [Required]
        public int Valuta { get; set; }
    }

    public class BundleViewModel {

        public int Id { get; set; }

        public List<Order> Orders { get; set; }

        public DateTime OrderDate { get; set; }

        public int BundleQuantity { get; set; }

        public string MarketName { get; set; }

        public string TrackNumber { get; set; }

        public double Price { get; set; }

        public string Invoice { get; set; }

        public string Description { get; set; }

        public bool isPaid { get; set; }

        public List<Status> Statuses { get; set; }
        public  Category Category { get; set; }
        public  Country Country { get; set; }
        public  Valuta Valuta { get; set; }
        public  ApplicationUser Customer { get; set; }

    }
}