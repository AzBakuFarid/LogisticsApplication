using LogisticsApp.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
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

        [HttpGet]
        public ActionResult AdminPaymentForOrder(int id)
        {
            OrderViewModel model = new OrderViewModel();
            try
            {
                Order order = db.orders.Single(w => w.Id == id);
                model.Id = order.Id;
                model.Customer = order.customer;
                model.Valuta = order.valuta;
                model.Quantity = order.Quantity;
                model.Price = order.Price;
            }
            catch (Exception) { }

            ViewBag.ServiceTax = 5; // bunlari bazaya elave etmek imkani yaranandan sonra deyisib dinamic elemek lazimdi
            ViewBag.UrgencyTax = 2;// bunlari bazaya elave etmek imkani yaranandan sonra deyisib dinamic elemek lazimdi
            return PartialView(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AdminPaymentForOrder(int[] selecteds, int _customer, int _order)
        {
            ApplicationUser user = null;
            Order order = null;

            try
            {
                user = db.Users.Single(s => s.CustomerNumber == _customer);
                order = db.orders.Single(s => s.Id == _order && s.isPaid == false && s.customer.CustomerNumber == _customer);
                double amount = order.valuta.getPriceInManat(order.Price * order.Quantity);
                double serviceTax = Math.Round(amount * 5 / 100, 2);  // general settingsde 5% xidmet haqqi ve 2% suretli sifaris hisselerni hazirladiqdan sonra 
                double urgencyTax = order.isUrgent == true ? Math.Round(amount * 2 / 100, 2) : 0;  // onlarin qiymetini alib burda duzelis elemek lazim olacaq
                double sum = amount + urgencyTax + serviceTax;
                if (sum < user.Balance)
                {
                    Transaction tr = new Transaction(user, sum, TransactionAction.Extract);
                    tr.TransactionInfo = "payment for order(s): " + _order;
                    db.transactions.Add(tr);
                    order.isPaid = true;
                    db.SaveChanges();
                }
            }
            catch (Exception) { }
            return RedirectToAction("List", "Order");
        }

        [HttpGet]
        public ActionResult AdminPaymentForBundle(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var model = db.bundles.Find(id);
            if (model == null)
            {
                return HttpNotFound();
            }

            return PartialView(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AdminPaymentForBundle(AdminBundlePaymentModel model)
        {
            Bundle bundle = null;
            string[] paymentMethods = new[] { "userbalance", "cash" };
            if (ModelState.IsValid && paymentMethods.Contains(model.paymentmethod))
            {
                bundle = db.bundles.FirstOrDefault(w => w.Id == model.bundle && w.isPaid == false);
                if (bundle == null)
                {
                    return HttpNotFound();
                }
                if (model.paymentmethod.Equals(paymentMethods[0]) && model.amount <= bundle.Customer.Balance)
                {
                    Transaction tr = new Transaction(bundle.Customer, model.amount, TransactionAction.Extract);
                    tr.TransactionInfo = "payment for bundle(s): " + model.bundle;
                    db.transactions.Add(tr);
                    bundle.isPaid = true;
                    db.SaveChanges();
                    TempData["AlertMessage"] = "Payment procceded succesfully";
                    return RedirectToAction("List", "Bundle");
                }
                if (model.paymentmethod.Equals(paymentMethods[1]))
                {
                    bundle.isPaid = true;
                    db.SaveChanges();
                    TempData["AlertMessage"] = "Payment procceded succesfully";
                    return RedirectToAction("List", "Bundle");
                }
            }
            TempData["AlertMessage"] = "Payment has not procceded";
            return RedirectToAction("AdminDetails", "Bundle", new { id = model.bundle });
        }
    }

    public class AdminBundlePaymentModel
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int bundle { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public double amount { get; set; }

        [Required]
        public string paymentmethod { get; set; }
    }

}