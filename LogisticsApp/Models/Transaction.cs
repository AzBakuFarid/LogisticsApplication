using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LogisticsApp.Models
{
    public class Transaction
    {
        public Transaction(ApplicationUser _customer, double _amount, TransactionAction action)
        {
            CreatedDate = DateTime.Now;
            CurrentAmount = _customer.Balance;
            amount = _amount;
            customer = _customer;
           if (action == TransactionAction.Add) { customer.Balance += amount; }
            else if (action == TransactionAction.Extract) { customer.Balance -= amount;
                amount *= -1;
            }
        }

        public Transaction()
        {

        }
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public double CurrentAmount { get; set; }
        public double amount { get; set; }
        public string TransactionInfo { get; set; }

        public virtual ApplicationUser customer { get; set; }
    }

    public enum TransactionAction {
        Add=1,
        Extract=2,
        Block=3
    }

    public class TransactionViewModel : GeneralContentViewModel {

        public List<TransactionDetails> details { get; set; } = new List<TransactionDetails>();
    }

    public class TransactionDetails {
        public string CreatedDate { get; set; }
        public double Balance { get; set; }
        public double Amount { get; set; }
        public string TransactionInfo { get; set; }
    }
}