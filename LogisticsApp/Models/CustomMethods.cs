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
        public static async Task<double> getUserBalanceAsync<T>(this T model, string userId)  where T : GeneralContentViewModel 
        {
            ApplicationDbContext db = new ApplicationDbContext();
                return await Task<double>.Run(() => { return db.Users.First(w => w.Id == userId).Balance; });
           
        }
        public static async Task<int> getUserUnreadMessagesAsync<T>(this T model, string userId) where T : GeneralContentViewModel
        {
            ApplicationDbContext db = new ApplicationDbContext();
                return await Task<int>.Run(() => { return db.messages.Where(w => w.receiver.Id == userId && w.isRead == false).Count(); });
           
        }
        public static async Task<int> getUserUnAnsweredInqueriesAsync<T>(this T model, string userId) where T : GeneralContentViewModel
        {
            ApplicationDbContext db = new ApplicationDbContext();
                return await Task<int>.Run(() => { return db.inqueries.Where(w => w.sender.Id == userId && w.isAnswered == false).Count(); });
           
        }
        public static async Task<int> getUserCustomerNumberAsync<T>(this T model, string userId) where T : GeneralContentViewModel
        {
            ApplicationDbContext db = new ApplicationDbContext();
                return await Task<int>.Run(() => { return db.Users.First(w => w.Id == userId).CustomerNumber; });
           
        }
        public static async Task<IList<Country>> getCountriesAsync()
        {
            ApplicationDbContext db = new ApplicationDbContext();
                return await Task<int>.Run(() => { return db.countries.ToList(); });
            }
        public static async Task<IList<CountryInformation>> getCountryInformationsAsync()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            return await Task<int>.Run(() => { return db.countryInformations.ToList(); });
        }
        public static async Task<string> getUserNameAsync<T>(this T model, string userId) where T : GeneralContentViewModel
        {
            ApplicationDbContext db = new ApplicationDbContext();
            return await Task<string>.Run(() => { return db.Users.First(w => w.Id == userId).Name; });

        }
        public static async Task<string> getUserSurnameAsync<T>(this T model, string userId) where T : GeneralContentViewModel
        {
            ApplicationDbContext db = new ApplicationDbContext();
            return await Task<string>.Run(() => { return db.Users.First(w => w.Id == userId).Surname; });

        }




    }
}