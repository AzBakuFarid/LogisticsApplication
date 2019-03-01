using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LogisticsApp.Models
{
    public class Transaction
    {
        public Transaction(ApplicationUser _customer, double _amount)
        {
            CreatedDate = DateTime.Now;
            CurrentAmount = _customer.Balance;
            amount = _amount;

        }
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public double CurrentAmount { get; set; }
        public double amount { get; set; }

        public string ApplicationUserId { get; set; }

        public void Add(double amount) {
            customer.Balance += amount;
        }
        public void Extract(double amount)
        {
            customer.Balance -= amount;
        }
        public virtual ApplicationUser customer { get; set; }
    }
}