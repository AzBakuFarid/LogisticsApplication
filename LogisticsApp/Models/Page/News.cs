using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LogisticsApp.Models.Page
{
    public class News
    {

        public int Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Modified { get; set; }
        public string PicturePath { get; set; }
        public bool isActive { get; set; }
    }

    public class NewsCreateModel
    {
        [Required]
        [DataType(DataType.Text)]
        //[RegularExpression("^[a-zA-Z0-9 ?'])+$")]
        public string Title { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string Text { get; set; }

        [Required]
        [DataType(DataType.Upload)]
        public HttpPostedFileBase File { get; set; }
    }

    public class NewsEditModel
    {
        [Required]
        [Range(1,int.MaxValue)]
        [HiddenInput]
        public int Id { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Title { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Text { get; set; }

        [DataType(DataType.Upload)]
        public HttpPostedFileBase File { get; set; }

        public string ImagePath { get; set; }
    }


}