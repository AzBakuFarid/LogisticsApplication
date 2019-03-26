using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using LogisticsApp.Models;
using LogisticsApp.Models.General;
using Microsoft.AspNet.Identity;

namespace LogisticsApp.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Order
        public async Task<ActionResult> Index()
        {
            var UserId = User.Identity.GetUserId();
            var model = db.orders.Where(w => w.ApplicationUserId == UserId).ToList();
            var userInfo = new GeneralContentViewModel();
            ViewBag.Balance = await userInfo.getUserBalanceAsync(UserId);
            ViewBag.MessagesCounter = await userInfo.getUserUnreadMessagesAsync(UserId);
            ViewBag.InqueriesCounter = await userInfo.getUserUnAnsweredInqueriesAsync(UserId);
            ViewBag.CustomerNumber = await userInfo.getUserCustomerNumberAsync(UserId);
            ViewBag.userInfo = userInfo;
            return View(model);
        }

        // GET: Order/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var UserId = User.Identity.GetUserId();
            Order order = db.orders.Find(id);
            if (order == null||order.ApplicationUserId!=UserId)
            {
                return HttpNotFound();
            }
            return PartialView(order);
        }
        public PartialViewResult ordersPerSteps(string id) {
            List<Order> model = null;
            try
            {
                var userId = User.Identity.GetUserId();
                model = db.orders.Where(w => w.ApplicationUserId == userId && w.Statuses.FirstOrDefault(a => a.isCurrent == true && a.Name == id) != null).ToList();
            }
            catch (Exception)
            {
                model = null;
            }

            return PartialView(model);
        }

        // GET: Order/Create
        public async Task<ActionResult> Create()
        {
            var userId = User.Identity.GetUserId();
            var model = new OrderViewModel();
            model.Balance = await model.getUserBalanceAsync(userId);
            model.MessageCounter = await model.getUserUnreadMessagesAsync(userId);
            model.InqueryCounter = await model.getUserUnAnsweredInqueriesAsync(userId);
            model.CustomerID = await model.getUserCustomerNumberAsync(userId);

            TempData["Countries"] = db.countries.Where(w => w.isActive == true).ToList();
            TempData["Valutas"] = db.valutas.Where(w => w.isActive == true).ToList();
            TempData["Categories"] = db.categories.ToList();
            return PartialView(model);
        }

        // POST: Order/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(OrderCreateModel model)
        {
            Country country = null;
            try
            {
                country = db.countries.Single(s => s.Id == model.countryId);

                for (var i = 0; i < model.Link.Length; i++)
                {
                    if (String.IsNullOrWhiteSpace(model.Link[i]) ||
                        String.IsNullOrWhiteSpace(model.Description[i])||
                         model.Price[i]<=0||model.Quantity[i]<1)  { throw new Exception(); }
                    Order order = new Order
                    {
                        Link = model.Link[i],
                        Price = model.Price[i],
                        Description = model.Description[i],
                        Quantity = model.Quantity[i],
                        CategoryId = model.CategoryId[i],
                        CountryId = model.countryId,
                        ApplicationUserId = User.Identity.GetUserId(),
                        ValutaId = model.ValutaId[i],
                    };
                    if (model.isUrgent == null || model.isUrgent == false)
                    {
                        order.isUrgent = false;
                    }
                    else { order.isUrgent = true; }
                    db.orders.Add(order);
                    db.SaveChanges();
                }

            }
            catch (Exception)
            {
                return RedirectToAction("Create");
            }

            return RedirectToAction("Index");
        }

        // GET: Order/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Order/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Link,Price,Quantity,isPaid,isUrgent,Description")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(order);
        }

        // POST: Order/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var userId = User.Identity.GetUserId();
            Order order = null;
            try
            {
                order = db.orders.Single(s=>s.Id==id&&s.ApplicationUserId==userId&&s.Statuses.Count==0);
                db.orders.Remove(order);
                db.SaveChanges();
            }
            catch (Exception)
            {
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Payment(int[] selecteds) 
            //bunu yazmisam deye artiq, yerini deyismedim. 
            //gelecekde dusunurem ki PAYMENT controllere kecirmek lazimdi
        {
            var userId = User.Identity.GetUserId();
            ApplicationUser user = null; 
            IList<Order> orders = null;
            try
            {
                user = db.Users.Single(s => s.Id == userId);
                orders = db.orders.Where(w => selecteds.Any(a => a == w.Id) && w.ApplicationUserId == userId && w.isPaid == false).ToList();
                double amount = orders.Sum(s => s.valuta.getPriceInManat(s.Price * s.Quantity));
                // general settingsde 5% xidmet haqqi ve 2% suretli sifaris hisselerni hazirladiqdan sonra 
                // onlarin qiymetini alib burda duzelis elemek lazim olacaq
                if (amount <= user.Balance)
                {
                    foreach (var item in orders)
                    {
                        item.isPaid = true;
                        item.addStatus(new Status
                        {
                            CreatedDate = DateTime.Now,
                            isCurrent = true,
                            Name = "ordered"
                        });
                    }
                    
                    Transaction tr = new Transaction(user, amount, TransactionAction.Extract);
                    tr.TransactionInfo = "payment for order(s): "+String.Join(" , ", selecteds);
                    db.transactions.Add(tr);
                    db.SaveChanges();
                }
                else { return RedirectToAction("AddToBalance", "Payment"); }
            }
            catch { }
           
            return RedirectToAction("Index");
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
