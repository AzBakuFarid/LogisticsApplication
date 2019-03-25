using LogisticsApp.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace LogisticsApp.Controllers
{
    [Authorize]
    public class PaymentController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: Payment
        [HttpGet]
        public async Task<ActionResult> AddToBalance()
        {
            var userId = User.Identity.GetUserId();
            var userInfo = new GeneralContentViewModel();
            ViewBag.Balance = await userInfo.getUserBalanceAsync(userId);
            ViewBag.MessagesCounter = await userInfo.getUserUnreadMessagesAsync(userId);
            ViewBag.InqueriesCounter = await userInfo.getUserUnAnsweredInqueriesAsync(userId);
            ViewBag.CustomerNumber = await userInfo.getUserCustomerNumberAsync(userId);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddToBalance(double amount)
        {
            if (amount > 0)
            {
                var userId = User.Identity.GetUserId();
                try
                {
                    var user = db.Users.Single(s => s.Id == userId);

                    Transaction tr = new Transaction(user, amount, TransactionAction.Add);
                    tr.TransactionInfo = "Balance adding:";
                    db.transactions.Add(tr);
                    db.SaveChanges();
                    return RedirectToAction("Countries", "Manage");
                }
                catch (Exception) { }
            }
            return RedirectToAction("AddToBalance");
        }
       
        public async Task<ActionResult> ShowTransactions()
        {
            var model = new TransactionViewModel();
            try
            {

                var userId = User.Identity.GetUserId();
                ViewBag.Balance = await model.getUserBalanceAsync(userId);
                ViewBag.MessagesCounter = await model.getUserUnreadMessagesAsync(userId);
                ViewBag.InqueriesCounter = await model.getUserUnAnsweredInqueriesAsync(userId);
                ViewBag.CustomerNumber = await model.getUserCustomerNumberAsync(userId);
                var transactions = db.transactions.Where(w => w.customer.Id == userId).OrderByDescending(o => o.CreatedDate).ToList();
                foreach (var item in transactions)
                {
                    model.details.Add(new TransactionDetails
                    {
                        CreatedDate = item.CreatedDate.ToString("dd.MM.yyyy HH:mm:ss"),
                        Balance = item.CurrentAmount,
                        Amount = item.amount,
                        TransactionInfo = item.TransactionInfo.Substring(0, item.TransactionInfo.IndexOf(":"))
                    });
                }
            }
            catch (Exception)
            {
                return HttpNotFound();
            }
            return PartialView(model);
        }
    }
}