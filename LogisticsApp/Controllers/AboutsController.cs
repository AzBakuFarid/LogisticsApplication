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
    public class AboutsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Abouts
        [AllowAnonymous]
        public ActionResult Index()
        {
            var about = db.abouts.FirstOrDefault(f => f.isActive == true);
            if (about == null)
            {
                return View(new About { Text = " " });
            }
            return View(about);
        }

        public ActionResult List()
        {

            return View(db.abouts.ToList());
        }

        // GET: Abouts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            About about = db.abouts.Find(id);
            if (about == null)
            {
                return HttpNotFound();
            }
            return Json(new { Text = about.Text, Id = about.Id }, JsonRequestBehavior.AllowGet);
        }

        // GET: Abouts/Create
        public ActionResult Create()
        {
            return PartialView(new About());
        }

        // POST: Abouts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Text")] About about)
        {
            if (ModelState.IsValid)
            {
                db.abouts.Add(about);
                db.SaveChanges();
            }
            return RedirectToAction("List");
        }
        // POST: Abouts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(About model)
        {
            if (ModelState.IsValid)
            {

                var about = db.abouts.FirstOrDefault(f => f.Id == model.Id);
                about.Text = model.Text;
                db.SaveChanges();
                ViewBag.AlertMessage = "The text has succesfully changed";
                return RedirectToAction("List");
            }
            ViewBag.AlertMessage = "Something gone wrong. Try again please!";
            return RedirectToAction("List");
        }
        // POST: Abouts/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            About about = db.abouts.Find(id);
            if (about == null)
            {
                return HttpNotFound();
            }

            db.abouts.Remove(about);
            db.SaveChanges();
            return RedirectToAction("List");
        }

        [HttpPost]
        public ActionResult ChangeActivityStatus(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var abouts = db.abouts.ToList();
            if (!abouts.Any(a => a.Id == id)) {
                return HttpNotFound();
            }
            foreach (var item in abouts)
            {
                if (item.Id == id) { item.isActive = true; }
                else { item.isActive = false; }
            }

            db.SaveChanges();
            return RedirectToAction("List");
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
