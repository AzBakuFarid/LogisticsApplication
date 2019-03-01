using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LogisticsApp.Models.Page
{
    public class Step
    {
        public int Id { get; set; }
        public bool isActive { get; set; }
        public string ImgPath { get; set; }
        public string Text { get; set; }
        public int StepCounter { get; set; }


    }
}