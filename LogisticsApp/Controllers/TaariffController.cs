using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LogisticsApp.Models;
using LogisticsApp.Models.Page;

namespace LogisticsApp.Controllers
{
    [Authorize]
    public class TaariffController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Taariff
        [AllowAnonymous]
        public ActionResult Index()
        {
            var taariffs = db.taariffs.Where(w=>w.isActive==true).ToList();
            ViewBag.countries = db.countries.Where(w => w.isActive == true).ToList();
            return View(taariffs);
        }

        // GET: Taariff/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Taariff taariff = db.taariffs.Find(id);
            if (taariff == null)
            {
                return HttpNotFound();
            }
            return View(taariff);
        }

        // GET: Taariff/Create
        public ActionResult Create()
        {
            ViewBag.CountryId = new SelectList(db.countries, "Id", "Name");
            return View();
        }

        // POST: Taariff/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CountryId,Weight,Price,isActive")] Taariff taariff)
        {
            if (ModelState.IsValid)
            {
                db.taariffs.Add(taariff);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CountryId = new SelectList(db.countries, "Id", "Name", taariff.CountryId);
            return View(taariff);
        }

        // GET: Taariff/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Taariff taariff = db.taariffs.Find(id);
            if (taariff == null)
            {
                return HttpNotFound();
            }
            ViewBag.CountryId = new SelectList(db.countries, "Id", "Name", taariff.CountryId);
            return View(taariff);
        }

        // POST: Taariff/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CountryId,Weight,Price,isActive")] Taariff taariff)
        {
            if (ModelState.IsValid)
            {
                db.Entry(taariff).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CountryId = new SelectList(db.countries, "Id", "Name", taariff.CountryId);
            return View(taariff);
        }

        // GET: Taariff/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Taariff taariff = db.taariffs.Find(id);
            if (taariff == null)
            {
                return HttpNotFound();
            }
            return View(taariff);
        }

        // POST: Taariff/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Taariff taariff = db.taariffs.Find(id);
            db.taariffs.Remove(taariff);
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
