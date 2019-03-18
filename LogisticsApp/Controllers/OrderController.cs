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
        public ActionResult Index()
        {
            return View(db.orders.ToList());
        }

        // GET: Order/Details/5
        public ActionResult Details(int? id)
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
        public ActionResult Create(/*[Bind(Include = "Link,Price,Quantity,isPaid,isUrgent,ValutaId,CategoryId, Description")]*/ IList<OrderViewModel> model, int countryId)
        {
            if (ModelState.IsValid)
            {
                Country country = null;
                try
                {
                    country = db.countries.Single(s=>s.Id==countryId);
                    foreach (var item in model)
                    {
                        Order order = new Order
                        {
                            Link = item.Link,
                            Price = item.Price,
                            isUrgent = item.isUrgent,
                            Description = item.Description,
                            Quantity = item.Quantity,
                            CategoryId = item.CategoryId,
                            CountryId = countryId,
                            ApplicationUserId = User.Identity.GetUserId(),
                            ValutaId = item.ValutaId,
                        };
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

            return RedirectToAction("Create", model);
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

        // GET: Order/Delete/5
        public ActionResult Delete(int? id)
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

        // POST: Order/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Order order = db.orders.Find(id);
            db.orders.Remove(order);
            db.SaveChanges();
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
