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
using Microsoft.AspNet.Identity;

namespace LogisticsApp.Controllers
{
    [Authorize]
    public class InqueryController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Inquerie
        public async Task<ActionResult> Index()
        {
            var UserId = User.Identity.GetUserId();
            var inqueries = db.inqueries.Where(w => w.sender.Id == UserId).OrderByDescending(o => o.CreatedDate).ToList();
            var model = new GeneralContentViewModel();
            model.Balance = await model.getUserBalanceAsync(UserId);
            model.MessageCounter = await model.getUserUnreadMessagesAsync(UserId);
            model.InqueryCounter = await model.getUserUnAnsweredInqueriesAsync(UserId);
            model.CustomerID = await model.getUserCustomerNumberAsync(UserId);
            ViewBag.inqueries = inqueries;
            ViewBag.messageTypes = db.messageTypes.ToList();
            ViewBag.MessageTypeId = new SelectList(db.messageTypes, "Id", "Name");
            return PartialView(model);
        }

        // GET: Inquerie/Details/5
        public ActionResult Details(int? id)
        {
            var UserId = User.Identity.GetUserId();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inquery inquery = db.inqueries.Find(id);
            if (inquery == null || inquery.sender.Id != UserId)
            {
                return HttpNotFound();
            }
            var DateView = inquery.CreatedDate.Day + "." +
                            inquery.CreatedDate.Month.ToString() + "." +
                            inquery.CreatedDate.Year.ToString();
            return Json(new { CreateDate = DateView, MessageType = inquery.messageType.Name, Text = inquery.Text }, JsonRequestBehavior.AllowGet);
        }

        // GET: Inquerie/Create
        public ActionResult Create()
        {
            ViewBag.MessageTypeId = new SelectList(db.messageTypes.ToList(), "Id", "Name");
            return PartialView();
        }

        // POST: Inquerie/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(InqueryViewModel model)
        {
            

            if (ModelState.IsValid)
            {
                string userId = User.Identity.GetUserId();
                var user = db.Users.Single(s => s.Id == userId);//await ApplicationUserManager.FindByIdAsync(User.Identity.GetUserId());
                
                var inquery = new Inquery();
                inquery.Text = model.Text;
                inquery.MessageTypeId = model.MessageTypeId;
                inquery.sender = user;
                db.inqueries.Add(inquery);
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        // GET: Inquerie/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inquery inquery = db.inqueries.Find(id);
            if (inquery == null)
            {
                return HttpNotFound();
            }
            ViewBag.MessageTypeId = new SelectList(db.messageTypes, "Id", "Name", inquery.MessageTypeId);
            return View(inquery);
        }

        // POST: Inquerie/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Text,CreatedDate,isAnswered,MessageTypeId")] Inquery inquery)
        {
            if (ModelState.IsValid)
            {
                db.Entry(inquery).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MessageTypeId = new SelectList(db.messageTypes, "Id", "Name", inquery.MessageTypeId);
            return View(inquery);
        }

        // GET: Inquerie/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inquery inquery = db.inqueries.Find(id);
            if (inquery == null)
            {
                return HttpNotFound();
            }
            return View(inquery);
        }

        // POST: Inquerie/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Inquery inquery = db.inqueries.Find(id);
            db.inqueries.Remove(inquery);
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
