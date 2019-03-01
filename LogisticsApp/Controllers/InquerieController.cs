using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LogisticsApp.Models;

namespace LogisticsApp.Controllers
{
    [Authorize]
    public class InquerieController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Inquerie
        public ActionResult Index()
        {
            var inqueries = db.inqueries.Include(i => i.messageType);
            return View(inqueries.ToList());
        }

        // GET: Inquerie/Details/5
        public ActionResult Details(int? id)
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

        // GET: Inquerie/Create
        public ActionResult Create()
        {
            ViewBag.MessageTypeId = new SelectList(db.messageTypes, "Id", "Name");
            return View();
        }

        // POST: Inquerie/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Text,CreatedDate,isAnswered,MessageTypeId")] Inquery inquery)
        {
            if (ModelState.IsValid)
            {
                db.inqueries.Add(inquery);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MessageTypeId = new SelectList(db.messageTypes, "Id", "Name", inquery.MessageTypeId);
            return View(inquery);
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
