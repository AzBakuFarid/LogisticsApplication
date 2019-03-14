using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LogisticsApp.Models.Page
{
    public class FAQ
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public bool isActive { get; set; }
    }

    public class ForumViewModel {
        public int Id { get; set; }
        [Required]
        [Display(Name ="Sual")]
        public string Question { get; set; }
        [Required]
        [Display(Name = "Cavab")]
        public string Answer { get; set; }
        public bool isActive { get; set; }
    }
}