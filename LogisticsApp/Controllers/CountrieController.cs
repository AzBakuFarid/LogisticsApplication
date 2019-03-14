using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LogisticsApp.Models;
using LogisticsApp.Models.General;

namespace LogisticsApp.Controllers
{
    [Authorize]
    public class CountrieController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Countrie
        public ActionResult Index()
        {
            return View(db.countries.Where(w => w.isActive == true).ToList());
        }

        public ActionResult List()
        {

            return View(db.countries.ToList());
        }

        // GET: Countrie/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CountryInformation item = null;
            CountryViewModel model = new CountryViewModel();
            try
            {
                item = db.countryInformations.Single(s => s.Id == id);
                model.Id = item.Country.Id;
                model.Name = item.Country.Name;
                model.imagePath = item.Country.imagePath;
                model.isActive = item.Country.isActive;
                model.State = item.State;
                model.Addressheader = item.Addressheader;
                model.AddressLine1 = item.AddressLine1;
                model.AddressLine2 = item.AddressLine2;
                model.ZIPcode = item.ZIPcode;
                model.City = item.City;
                model.IDNumber = item.IDNumber;
                model.PhoneNumber = item.PhoneNumber;
                model.Semt = item.Semt;
                model.Ilce = item.Ilce;
                model.TaxIDNumber = item.TaxIDNumber;
                model.Area = item.Area;
            }
            catch (Exception)
            {
                model = null;
            }
            return PartialView(model);
        }

        // GET: Countrie/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Countrie/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(string name)
        {
            if (String.IsNullOrWhiteSpace(name))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Country country = new Country { Name = name };
            db.countries.Add(country);
            db.SaveChanges();

            CountryInformation info = new CountryInformation { Id = db.countries.First(f => f.Name == name).Id };
            db.countryInformations.Add(info);
            db.SaveChanges();
            TempData["AlertMessage"] = "country "+"\""+country.Name+"\""+" has succesfully created";
            return RedirectToAction("List");

        }

        // POST: Countrie/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CountryViewModel model)
        {
            if (ModelState.IsValid)
            {
                CountryInformation info = null;
                try
                {
                    info = db.countryInformations.Single(s => s.Id == model.Id);
                }
                catch (Exception)
                {
                    ViewBag.AlertMessage = "Something gone wrong. Try again please!";
                    return RedirectToAction("List");
                }
                info.IDNumber = model.IDNumber;
                info.State = model.State;
                info.Addressheader = model.Addressheader;
                info.AddressLine1 = model.AddressLine1;
                info.AddressLine2 = model.AddressLine2;
                info.ZIPcode = model.ZIPcode;
                info.City = model.City;
                info.PhoneNumber = model.PhoneNumber;
                info.Semt = model.Semt;
                info.Ilce = model.Ilce;
                info.TaxIDNumber = model.TaxIDNumber;
                info.Area = model.Area;
                info.Country.Name = model.Name;
                info.Country.isActive = model.isActive;
                if (model.File != null && model.File.ContentLength > 0)
                {
                    var fileExtension = Path.GetExtension(model.File.FileName);
                    var fileName = Path.GetFileNameWithoutExtension(model.File.FileName) + "-" +
                        DateTime.Now.ToString("yyyyMMddHHmmssfff") + fileExtension;
                    var path = Path.Combine(Server.MapPath("~/Img/country"), fileName);
                    model.File.SaveAs(path);
                    info.Country.imagePath = "/Img/country/" + fileName;
                }

                db.SaveChanges();
                TempData["AlertMessage"] = "informations of country " + "\"" + info.Country.Name + "\"" + " has succesfully changed";
                return RedirectToAction("List");
            }
            TempData["AlertMessage"] = "Something gone wrong. Try again please!";
            return RedirectToAction("List");
        }

        // POST: Countrie/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Country country = db.countries.Find(id);
            CountryInformation info = db.countryInformations.Find(id);
            if (country == null)
            {
                return HttpNotFound();
            }

            db.countryInformations.Remove(info);
            db.countries.Remove(country);
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
