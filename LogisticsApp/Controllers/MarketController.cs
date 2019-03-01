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
    public class MarketController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Market
        [AllowAnonymous]
        public ActionResult Index(int? id)
        {
            if (id == null)
                ViewBag.markets = db.markets.Where(w => w.isActive == true).ToList();
            else {
                ViewBag.markets = db.markets.Where(w => w.isActive == true && w.categories.Any(a => a.Id == id)).ToList();
            }
            ViewBag.selected = id;
            return View(db.categories.ToList());
            
        }

        [AllowAnonymous]
        public PartialViewResult RenderList()
        {
            var model = db.categories.ToList();
            return PartialView("PartialCategories", model);
        }

        // GET: Market/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Market market = db.markets.Find(id);
            if (market == null)
            {
                return HttpNotFound();
            }
            return View(market);
        }

        // GET: Market/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Market/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Link,ImgPath,isActive,CategoryId,CountryId")] Market market)
        {
            if (ModelState.IsValid)
            {
                db.markets.Add(market);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(market);
        }

        // GET: Market/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Market market = db.markets.Find(id);
            if (market == null)
            {
                return HttpNotFound();
            }
            return View(market);
        }

        // POST: Market/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Link,ImgPath,isActive,CategoryId,CountryId")] Market market)
        {
            if (ModelState.IsValid)
            {
                db.Entry(market).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(market);
        }

        // GET: Market/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Market market = db.markets.Find(id);
            if (market == null)
            {
                return HttpNotFound();
            }
            return View(market);
        }

        // POST: Market/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Market market = db.markets.Find(id);
            db.markets.Remove(market);
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
