using LogisticsApp.Models.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LogisticsApp.Models
{
    public class Inquery
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool isAnswered { get; set; }
        public int MessageTypeId { get; set; }

        public virtual ApplicationUser sender { get; set; }
        public virtual MessagType messageType { get; set; }
    }
}