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
using Microsoft.AspNet.Identity;

namespace LogisticsApp.Controllers
{
    public class BundleController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Bundle
        public ActionResult Index()
        {
            var bundles = db.bundles.Include(b => b.Category).Include(b => b.Country).Include(b => b.Customer).Include(b => b.Valuta);
            return View(bundles.ToList());
        }

        // GET: Bundle/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bundle bundle = db.bundles.Find(id);
            if (bundle == null)
            {
                return HttpNotFound();
            }
            return View(bundle);
        }

        // GET: Bundle/Create
        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.Category = db.categories.ToList();
            ViewBag.Country = db.countries.Where(w => w.isActive == true).ToList();
            ViewBag.Valuta = db.valutas.Where(w => w.isActive == true).ToList();
            return PartialView();
        }

        [HttpGet]
        public ActionResult AdminCreate(int _order)
        {
            OrderViewModel model = null;
            try
            {
                Order item = db.orders.Single(s => s.Id == _order && s.isPaid == true);
                model = new OrderViewModel {
                    Id = item.Id,
                    Price = item.Price,
                    Quantity = item.Quantity,
                    CreatedDate = item.CreatedDate.ToString("yyyy.MM.dd HH:mm:ss"),
                    Customer = item.customer,
                    Country = item.country,
                    Valuta = item.valuta,
                    Category = item.category,
                };
            }
            catch (Exception){ return new HttpStatusCodeResult(HttpStatusCode.BadRequest); }
            return PartialView(model);
        }

        // POST: Bundle/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OrderDate,BundleQuantity,MarketName,TrackNumber,Price,Invoice,Description,Category,Country,Valuta")] BundleCreateModel model)
        {
            Bundle bundle = null;
            string userId = null;
            if (ModelState.IsValid)
            {
                if (model.OrderDate > DateTime.Now || model.Invoice == null || model.Invoice.ContentLength < 0) {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                try
                {
                    userId = User.Identity.GetUserId();
                    bundle = new Bundle();

                    bundle.Customer = db.Users.Single(s => s.Id == userId);
                    bundle.MarketName = model.MarketName;
                    bundle.Price = model.Price;
                    bundle.TrackNumber = model.TrackNumber;
                    bundle.Description = model.Description;
                    bundle.BundleQuantity = model.BundleQuantity;
                    bundle.Category = db.categories.Single(s => s.Id == model.Category);
                    bundle.Country = db.countries.Single(s => s.Id == model.Country && s.isActive == true);
                    bundle.Valuta = db.valutas.Single(s => s.Id == model.Valuta && s.isActive == true);
                    bundle.CreatedDate = DateTime.Now;
                    bundle.OrderDate = model.OrderDate;
                   
                    bundle.addStatus(new Status
                    {
                        CreatedDate = DateTime.Now,
                        Name = "ordered"
                    });
                    var fileExtension = Path.GetExtension(model.Invoice.FileName);
                    var fileName = Path.GetFileNameWithoutExtension(model.Invoice.FileName) + "-" +
                        DateTime.Now.ToString("yyyyMMddHHmmssfff") + fileExtension;
                    var fullpath = Path.GetFullPath(Server.MapPath("/Img/Invoice/"));
                    var directoryName = fullpath + bundle.Customer.CustomerNumber;
                    Directory.CreateDirectory(directoryName);
                    var path = Path.Combine(Server.MapPath("/Img/Invoice/"+ bundle.Customer.CustomerNumber), fileName);
                    model.Invoice.SaveAs(path);
                    bundle.InvoiceFilePath = "/Img/Invoice/"+ bundle.Customer.CustomerNumber + "/" + fileName;
                    db.bundles.Add(bundle);
                    db.SaveChanges();

                }
                catch (Exception)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError); 
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AdminCreate([Bind(Include = "Order,OrderDate,BundleQuantity,MarketName,TrackNumber,Price,Invoice,Description,Category,Country,Valuta")] BundleCreateModel model)
        {
            Bundle bundle = null;
            
            if (ModelState.IsValid)
            {
                if (model.OrderDate > DateTime.Now || model.Invoice == null || model.Invoice.ContentLength < 0)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                try
                {
                    bundle = new Bundle();

                    bundle.Customer = db.orders.Single(s => s.Id == model.Order).customer;
                    bundle.MarketName = model.MarketName;
                    bundle.Price = model.Price;
                    bundle.TrackNumber = model.TrackNumber;
                    bundle.Description = model.Description;
                    bundle.BundleQuantity = model.BundleQuantity;
                    bundle.Category = db.categories.Single(s => s.Id == model.Category);
                    bundle.Country = db.countries.Single(s => s.Id == model.Country && s.isActive == true);
                    bundle.Valuta = db.valutas.Single(s => s.Id == model.Valuta && s.isActive == true);
                    bundle.CreatedDate = DateTime.Now;
                    bundle.OrderDate = model.OrderDate;
                    
                    bundle.addStatus(new Status
                    {
                        CreatedDate = DateTime.Now,
                        Name = "ordered"
                    });
                    var fileExtension = Path.GetExtension(model.Invoice.FileName);
                    var fileName = Path.GetFileNameWithoutExtension(model.Invoice.FileName) + "-" +
                        DateTime.Now.ToString("yyyyMMddHHmmssfff") + fileExtension;
                    var fullpath = Path.GetFullPath(Server.MapPath("/Img/Invoice/"));
                    var directoryName = fullpath + bundle.Customer.CustomerNumber;
                    Directory.CreateDirectory(directoryName);
                    var path = Path.Combine(Server.MapPath("/Img/Invoice/" + bundle.Customer.CustomerNumber), fileName);
                    model.Invoice.SaveAs(path);
                    bundle.InvoiceFilePath = "/Img/Invoice/" + bundle.Customer.CustomerNumber + "/" + fileName;
                    db.bundles.Add(bundle);
                    db.SaveChanges();
                    var order = db.orders.Single(s => s.Id == model.Order);
                    var readybundle = db.bundles.Single(s => s.TrackNumber == model.TrackNumber&&s.OrderDate==model.OrderDate&&s.MarketName==model.MarketName);
                    order.BundleId = readybundle.Id;
                    db.SaveChanges();
                }
                catch (Exception)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
                }
            }
            return RedirectToAction("List","Order");
        }


        [HttpPost]
        public ActionResult Search(string filter)
        {
            List<Bundle> bundles = null;
            if (filter != null)
            {
                if (filter.regexControl(@"^\w+$"))
                {
                    bundles = db.bundles.Where(w => w.Id.ToString().IndexOf(filter.ToString()) >= 0 ||
                    w.TrackNumber.ToUpper().IndexOf(filter.ToUpper()) >= 0).ToList();
                }
            }
            return PartialView(bundles);
        }



        // GET: Bundle/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bundle bundle = db.bundles.Find(id);
            if (bundle == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(db.categories, "Id", "Name", bundle.CategoryId);
            ViewBag.CountryId = new SelectList(db.countries, "Id", "Name", bundle.CountryId);
            ViewBag.ValutaId = new SelectList(db.valutas, "Id", "Name", bundle.ValutaId);
            return View(bundle);
        }

        // POST: Bundle/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CreatedDate,OrderDate,BundleQuantity,MarketName,TrackNumber,Price,InvoiceFilePath,Description,CategoryId,CountryId,ValutaId,CustomerId")] Bundle bundle)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bundle).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(db.categories, "Id", "Name", bundle.CategoryId);
            ViewBag.CountryId = new SelectList(db.countries, "Id", "Name", bundle.CountryId);
            ViewBag.ValutaId = new SelectList(db.valutas, "Id", "Name", bundle.ValutaId);
            return View(bundle);
        }

        // GET: Bundle/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bundle bundle = db.bundles.Find(id);
            if (bundle == null)
            {
                return HttpNotFound();
            }
            return View(bundle);
        }

        // POST: Bundle/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Bundle bundle = db.bundles.Find(id);
            db.bundles.Remove(bundle);
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
