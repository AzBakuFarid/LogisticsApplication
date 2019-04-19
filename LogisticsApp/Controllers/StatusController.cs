using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LogisticsApp.Models;
using LogisticsApp.Models.General;

namespace LogisticsApp.Controllers
{
    public class StatusController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Status
        public ActionResult Index()
        {
            return View(db.statuses.ToList());
        }

        // GET: Status/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Status status = db.statuses.Find(id);
            if (status == null)
            {
                return HttpNotFound();
            }
            return View(status);
        }

        // GET: Status/Create
        public ActionResult Create(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
           
            var bundle = db.bundles.Find(id);
            if (bundle == null)
            {
                return HttpNotFound();
            }
            return PartialView(id);
        }

        // POST: Status/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(StatusCreateModel model)
        {
            Bundle bundle = null;
            // bilirem bu duzgun deyil.... amma indi bununla bagli duzelis elemek istesem cox yerde deyisiklik elemeliyem
            // ona gore helelik bununla yola verirem...
            string[] statuses = { "inforeignstock", "ontheway", "arrived", "delivered" };
            if (ModelState.IsValid&&statuses.Contains(model.Name))
            {
                
                bundle = db.bundles.FirstOrDefault(f => f.Id == model.Bundle);
                bundle.addStatus(new Status { Name = model.Name, CreatedDate = model.CreateDate });
                db.SaveChanges();
            }

            return RedirectToAction("AdminDetails", "Bundle", new { id = model.Bundle });

        }

        // GET: Status/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var bundle = db.bundles.Find(id);
            if (bundle == null)
            {
                return HttpNotFound();
            }
            var model = new StatusViewModel();
            model.Bundle = bundle.Id;
            model.statuses = bundle.Statuses;
            return PartialView(model);
        }

        // POST: Status/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(StatusCreateModel model)
        {
            Bundle bundle = null;
            string[] statuses = { "ordered","inforeignstock", "ontheway", "arrived", "delivered" };
            if (ModelState.IsValid && statuses.Contains(model.Name))
            {
                bundle = db.bundles.FirstOrDefault(f => f.Id == model.Bundle);
                bundle.editStatus(new Status { Name = model.Name, CreatedDate = model.CreateDate });
                db.SaveChanges();
            }
            return RedirectToAction("AdminDetails", "Bundle", new { id = model.Bundle });
        }

        // GET: Status/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var bundle = db.bundles.Find(id);
            if (bundle == null)
            {
                return HttpNotFound();
            }
            var model = new StatusViewModel();
            model.Bundle = bundle.Id;
            model.statuses = bundle.Statuses;
            return PartialView(model);
        }

        // POST: Status/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(StatusCreateModel model)
        {
            Bundle bundle = null;
            string[] statuses = { "ordered","inforeignstock", "ontheway", "arrived", "delivered" };
            if (ModelState.IsValid && statuses.Contains(model.Name))
            {
                bundle = db.bundles.FirstOrDefault(f => f.Id == model.Bundle);
                bundle.deleteStatus(new Status { Name = model.Name, CreatedDate = model.CreateDate });
                db.SaveChanges();
            }
            return RedirectToAction("AdminDetails", "Bundle", new { id = model.Bundle });
        }

        public JsonResult GetStatusDate(string _name, int? _bundle) {
            string[] statuses = { "ordered","inforeignstock", "ontheway", "arrived", "delivered" };

            if (_bundle == null|| !statuses.Contains(_name))
            {
                return null;
            }
            var bundle = db.bundles.Find(_bundle);
            if (bundle == null)
            {
                return null;
            }

            var data = bundle.Statuses.First(w => w.Name.Equals(_name)).CreatedDate;
            return Json(new { date=data.ToString("yyyy-MM-dd") }, JsonRequestBehavior.AllowGet);
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
