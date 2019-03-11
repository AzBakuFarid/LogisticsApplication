using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LogisticsApp.Models.Page
{
    public class Carusel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public string ImgPath { get; set; }
        public bool isActive { get; set; }
    }

    public class CaruselViewModel {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Text { get; set; }
     
        //[FileExtensions(Extensions = "jpeg, jpg, png",ErrorMessage = "Specify a jpeg, jpg or a png file")]
        public HttpPostedFileBase File { get; set; }
        public bool isActive { get; set; }
    }
}