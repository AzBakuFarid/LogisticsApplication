using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LogisticsApp.Models.General
{
    public class Status
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool isCurrent { get; set; }

        public virtual ICollection<Order> orders { get; set; } // bunu silecem
        public virtual ICollection<Bundle> bundles { get; set; }
    }



    public class StatusCreateModel {

        [Required]
        [Range(1, int.MaxValue)]
        public int Bundle { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime CreateDate { get; set; }

    }

    public class StatusViewModel
    {
        public int Bundle { get; set; }
        public IEnumerable<Status> statuses { get; set; }

}
}