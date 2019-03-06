using LogisticsApp.Models.General;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LogisticsApp.Models
{
    public class Inquery
    {
        public Inquery()
        {
            CreatedDate = DateTime.Now;
        }
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime CreatedDate { get; set; }
        [DefaultValue(false)]
        public bool isAnswered { get; set; }
        public int MessageTypeId { get; set; }
        public string senderId { get; set; }

        public virtual ApplicationUser sender { get; set; }
        public virtual MessagType messageType { get; set; }

    }

    public class InqueryViewModel {

        [Required]
        public string Text { get; set; }
        [Required]
        public int MessageTypeId { get; set; }

    }
}