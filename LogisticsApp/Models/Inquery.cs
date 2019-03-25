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
        public bool isAnswered { get; set; }
        public int MessageTypeId { get; set; }
        public string senderId { get; set; }

        public virtual ApplicationUser sender { get; set; }
        public virtual MessagType messageType { get; set; }

    }

    public class InqueryViewModel {
        public int MessageTypeId { get; set; }
        public string Text { get; set; }
        [Display(Name ="Message Type")]
        public string MessageType { get; set; }
        [Display(Name = "Date of creation")]
        public string CreatedDate { get; set; }
        [Display(Name = "Is query answered?")]
        public bool isAnswered { get; set; }
    }
}