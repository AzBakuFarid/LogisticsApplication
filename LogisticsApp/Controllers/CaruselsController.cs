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
    public class CaruselsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Carusels
        //public ActionResult Index()
        //{
        //    return PartialView(db.carusel.Where(w=>w.isActive==true).ToList());
        //}

        // GET: Carusels/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Carusel carusel = db.carusel.Find(id);
            if (carusel == null)
            {
                return HttpNotFound();
            }
            return View(carusel);
        }

        // GET: Carusels/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Carusels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Text,ImgPath,isActive")] Carusel carusel)
        {
            if (ModelState.IsValid)
            {
                db.carusel.Add(carusel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(carusel);
        }

        // GET: Carusels/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Carusel carusel = db.carusel.Find(id);
            if (carusel == null)
            {
                return HttpNotFound();
            }
            return View(carusel);
        }

        // POST: Carusels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Text,ImgPath,isActive")] Carusel carusel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(carusel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(carusel);
        }

        // GET: Carusels/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Carusel carusel = db.carusel.Find(id);
            if (carusel == null)
            {
                return HttpNotFound();
            }
            return View(carusel);
        }

        // POST: Carusels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Carusel carusel = db.carusel.Find(id);
            db.carusel.Remove(carusel);
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
