using LogisticsApp.Models.General;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LogisticsApp.Models.Page
{
    public class Taariff
    {
        public int Id { get; set; }
        public int CountryId { get; set; }
        public double Weight { get; set; }
        public double Price { get; set; }
        public bool isActive { get; set; }

        public virtual Country country { get; set; }

    }

    public class CalculatorModel {
        [Range(0.01, double.MaxValue)]
        public double Height { get; set; }

        [Range(0.01, double.MaxValue)]
        public double Width { get; set; }

        [Range(0.01, double.MaxValue)]
        public double Length { get; set; }

        [Range(0.01, double.MaxValue)]
        public double Weight { get; set; }

        [Range(1, int.MaxValue)]
        public int CountryId { get; set; }

        [Range(1, int.MaxValue)]
        public int BundleCount { get; set; }
        
    }
}