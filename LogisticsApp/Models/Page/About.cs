using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LogisticsApp.Models.Page
{
    public class About
    {
        public int Id { get; set; }
        [Required]
        public string Text { get; set; }
        public bool isActive { get; set; }
    }
}