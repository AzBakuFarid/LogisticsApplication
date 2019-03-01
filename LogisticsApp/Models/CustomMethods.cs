using LogisticsApp.Models;
using LogisticsApp.Models.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace LogisticsApp.Models
{
    public static class CustomMethods
    {
        public static async Task<double> getUserBalanceAsync(this IndexViewModel model, string userId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
                return await Task<double>.Run(() => { return db.Users.First(w => w.Id == userId).Balance; });
           
        }
        public static async Task<int> getUserUnreadMessagesAsync(this IndexViewModel model, string userId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
                return await Task<int>.Run(() => { return db.messages.Where(w => w.receiver.Id == userId && w.isRead == false).Count(); });
           
        }
        public static async Task<int> getUserUnAnsweredInqueriesAsync(this IndexViewModel model, string userId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
                return await Task<int>.Run(() => { return db.inqueries.Where(w => w.sender.Id == userId && w.isAnswered == false).Count(); });
           
        }
        public static async Task<int> getUserCustomerNumberAsync(this IndexViewModel model, string userId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
                return await Task<int>.Run(() => { return db.Users.First(w => w.Id == userId).CustomerNumber; });
           
        }
        public static async Task<IList<Country>> getCountryInformationsAsync()
        {
            ApplicationDbContext db = new ApplicationDbContext();
                return await Task<int>.Run(() => { return db.countries.ToList(); });
            }
       




    }
}