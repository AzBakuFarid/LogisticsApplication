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
    public class MessageController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Message
        public async Task<ActionResult> Index()
        {
            var UserId = User.Identity.GetUserId();
            var messages = db.messages.Where(w => w.receiver.Id == UserId).OrderByDescending(o => o.CreatedDate).ToList();
            var model = new GeneralContentViewModel();
            model.Balance = await model.getUserBalanceAsync(UserId);
            model.MessageCounter = await model.getUserUnreadMessagesAsync(UserId);
            model.InqueryCounter = await model.getUserUnAnsweredInqueriesAsync(UserId);
            model.CustomerID = await model.getUserCustomerNumberAsync(UserId);
            ViewBag.messages = messages;
            return PartialView(model);
        }

        // GET: Message/Details/5
        public ActionResult Details(int? id)
        {
            var UserId = User.Identity.GetUserId();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Message message = db.messages.Find(id);
            if (message == null || message.receiver.Id != UserId)
            {
                return HttpNotFound();
            }
            if (!message.isRead) {
                message.isRead = true;
                db.SaveChanges();
            }
            
            var DateView = message.CreatedDate.Day + "." +
                           message.CreatedDate.Month.ToString() + "." +
                           message.CreatedDate.Year.ToString();
            return Json (new { CreateDate= DateView, MessageType= message.messageType.Name, Text = message.Text }, JsonRequestBehavior.AllowGet);
        }

        // GET: Message/Create
        public ActionResult Create()
        {
            ViewBag.MessageTypeId = new SelectList(db.messageTypes, "Id", "Name");
            return View();
        }

        // POST: Message/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Text,CreatedDate,isRead,MessageTypeId")] Message message)
        {
            if (ModelState.IsValid)
            {
                db.messages.Add(message);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MessageTypeId = new SelectList(db.messageTypes, "Id", "Name", message.MessageTypeId);
            return View(message);
        }

        // GET: Message/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Message message = db.messages.Find(id);
            if (message == null)
            {
                return HttpNotFound();
            }
            ViewBag.MessageTypeId = new SelectList(db.messageTypes, "Id", "Name", message.MessageTypeId);
            return View(message);
        }

        // POST: Message/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Text,CreatedDate,isRead,MessageTypeId")] Message message)
        {
            if (ModelState.IsValid)
            {
                db.Entry(message).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MessageTypeId = new SelectList(db.messageTypes, "Id", "Name", message.MessageTypeId);
            return View(message);
        }

        // GET: Message/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Message message = db.messages.Find(id);
            if (message == null)
            {
                return HttpNotFound();
            }
            return View(message);
        }

        // POST: Message/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Message message = db.messages.Find(id);
            db.messages.Remove(message);
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
