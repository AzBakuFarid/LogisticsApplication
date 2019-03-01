using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LogisticsApp.Models.Page
{
    public class Contact
    {
        public int Id { get; set; }
        public string Address { get; set; }
        public string Phonenumber { get; set; }
        public string Mobilenumber { get; set; }
        [EmailAddress]
        public string Email { get; set; }



    }
}