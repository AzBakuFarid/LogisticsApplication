using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
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

        public async Task<ActionResult> List()
        {
            List<BundleViewModel> model = new List<BundleViewModel>();
            try
            {
                foreach (var item in db.bundles.ToList())
                {
                    await Task.Run(() =>
                    {
                        model.Add(new BundleViewModel
                        {
                            Id = item.Id,
                            Orders = db.orders.Where(w => w.BundleId == item.Id).ToList(),
                            Price = item.Price,
                            BundleQuantity = item.BundleQuantity,
                            Description = item.Description,
                            OrderDate = item.CreatedDate,
                            Customer = item.Customer,
                            Country = item.Country,
                            Valuta = item.Valuta,
                            Category = item.Category,
                            Statuses = item.Statuses.ToList()
                        });
                    });
                }
                ViewBag.Bundles = db.bundles.ToList();
            }
            catch (Exception)
            {

            }

            return View(model);
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


        // GET: Bundle/Details/5
        public ActionResult AdminDetails(int? id)
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

            var model = new BundleViewModel {
                Id=bundle.Id,
                Country=bundle.Country,
                Category=bundle.Category,
                Valuta=bundle.Valuta,
                Customer=bundle.Customer,
                TrackNumber=bundle.TrackNumber,
                Description=bundle.Description,
                Price=bundle.Price,
                Orders=db.orders.Where(w=>w.BundleId==bundle.Id).ToList(),
                Statuses=bundle.Statuses.ToList(),
                isPaid=bundle.isPaid,
                Invoice=bundle.InvoiceFilePath,
                BundleQuantity=bundle.BundleQuantity,
                OrderDate=bundle.CreatedDate

            };
            return View(model);
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
                if (model.OrderDate > DateTime.Now || model.Invoice.ContentLength < 0) {
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
                    bundle.CreatedDate = model.OrderDate;
                   
                    bundle.addStatus(new Status
                    {
                        CreatedDate = bundle.CreatedDate,
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

        [HttpGet]
        public ActionResult AdminCreate(IEnumerable<int> selecteds)
        {
            try
            {
                var model = new BundleViewModel();
                List<Order> orders = db.orders.Where(w => w.BundleId == 0 && selecteds.Contains(w.Id)&&w.isPaid==true).ToList();
                model.Orders = orders;
                foreach (var item in orders)
                {
                    if (item.customer.CustomerNumber != orders[0].customer.CustomerNumber ||
                        item.category.Id != orders[0].category.Id ||
                        item.country.Id != orders[0].country.Id) { return HttpNotFound(); }
                }

                return PartialView(model);
            }
            catch (Exception) {  }
            return HttpNotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AdminCreate([Bind(Include = "Orders,OrderDate,BundleQuantity,MarketName,TrackNumber,Price,Invoice,Description,Category,Country,Valuta")] BundleCreateModel model)
        {
            Bundle bundle = null;

            if (ModelState.IsValid)
            {
                if (model.OrderDate > DateTime.Now || model.Invoice.ContentLength < 0)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                try
                {
                    bundle = new Bundle();
                    List<Order> orders = db.orders.Where(w => w.BundleId == 0 && model.Orders.Contains(w.Id)&&w.isPaid==true).ToList();
                    foreach (var item in orders)
                    {
                        if (item.customer.CustomerNumber != orders[0].customer.CustomerNumber ||
                            item.category.Id != orders[0].category.Id ||
                            item.country.Id != orders[0].country.Id)
                        {
                            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                        }
                    }

                    bundle.Customer = orders[0].customer;
                    bundle.MarketName = model.MarketName;
                    bundle.Price = model.Price;
                    bundle.TrackNumber = model.TrackNumber;
                    bundle.Description = model.Description;
                    bundle.BundleQuantity = model.BundleQuantity;
                    bundle.Category = db.categories.Single(s => s.Id == model.Category);
                    bundle.Country = db.countries.Single(s => s.Id == model.Country && s.isActive == true);
                    bundle.Valuta = db.valutas.Single(s => s.Id == model.Valuta && s.isActive == true);
                    bundle.CreatedDate = model.OrderDate;

                    bundle.addStatus(new Status
                    {
                        CreatedDate = bundle.CreatedDate,
                        Name = "ordered"
                    });
                    var fileExtension = Path.GetExtension(model.Invoice.FileName);
                    var fileName = Path.GetFileNameWithoutExtension(model.Invoice.FileName) + "-" +
                        DateTime.Now.ToString("yyyyMMdd_HHmmssfff") + fileExtension;
                    var fullpath = Path.GetFullPath(Server.MapPath("/Img/Invoice/"));
                    var directoryName = fullpath + bundle.Customer.CustomerNumber;
                    Directory.CreateDirectory(directoryName);
                    var path = Path.Combine(Server.MapPath("/Img/Invoice/" + bundle.Customer.CustomerNumber), fileName);
                    model.Invoice.SaveAs(path);
                    bundle.InvoiceFilePath = "/Img/Invoice/" + bundle.Customer.CustomerNumber + "/" + fileName;
                    db.bundles.Add(bundle);
                    db.SaveChanges();
                    var readybundle = db.bundles.Single(s => s.TrackNumber == model.TrackNumber && s.CreatedDate == model.OrderDate && s.MarketName == model.MarketName);
                    foreach (var item in orders)
                    {
                         item.BundleId = readybundle.Id;
                    }
                    db.SaveChanges();
                }
                catch (Exception)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
                }
            }
            return RedirectToAction("List", "Bundle");
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
