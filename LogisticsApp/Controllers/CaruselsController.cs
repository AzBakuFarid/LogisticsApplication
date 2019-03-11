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
using LogisticsApp.Models.Page;

namespace LogisticsApp.Controllers
{
    public class CaruselsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        //GET: Carusels
        public ActionResult List()
        {
            return View(db.carusel.ToList());
        }

        //GET: Carusels/Details/5
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
            return Json(new { Text = carusel.Text, Id = carusel.Id, Title = carusel.Title, Image = carusel.ImgPath }, JsonRequestBehavior.AllowGet);
        }

        // GET: Carusels/Create
        public ActionResult Create()
        {
            return PartialView(new CaruselViewModel());
        }

        // POST: Carusels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Title,Text,File")] CaruselViewModel model)
        {

            if (ModelState.IsValid)
            {
                Carusel carusel = new Carusel();
                carusel.Title = model.Title;
                carusel.Text = model.Text;
                if (model.File != null && model.File.ContentLength > 0)
                {
                    var fileExtension = Path.GetExtension(model.File.FileName);
                    var fileName = Path.GetFileNameWithoutExtension(model.File.FileName) + "-" +
                        DateTime.Now.ToString("yyyyMMddHHmmssfff") + fileExtension;
                    var path = Path.Combine(Server.MapPath("~/Img/carusel"), fileName);
                    model.File.SaveAs(path);
                    carusel.ImgPath = "/Img/carusel/" + fileName;
                }

                db.carusel.Add(carusel);
                db.SaveChanges();
                return RedirectToAction("List");
            }

            return RedirectToAction("List");
        }

        // GET: Carusels/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Carusel carusel = db.carusel.Find(id);
        //    if (carusel == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(carusel);
        //}

        // POST: Carusels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Text,File")] CaruselViewModel model)
        {
            if (ModelState.IsValid)
            {
                Carusel carusel = db.carusel.Single(s => s.Id == model.Id);
                if (carusel == null)
                {
                    return HttpNotFound();
                }
                carusel.Title = model.Title;
                carusel.Text = model.Text;
                if (model.File != null && model.File.ContentLength > 0)
                {
                    var fileExtension = Path.GetExtension(model.File.FileName);
                    var fileName = Path.GetFileNameWithoutExtension(model.File.FileName) + "-" +
                        DateTime.Now.ToString("yyyyMMddHHmmssfff") + fileExtension;
                    var path = Path.Combine(Server.MapPath("~/Img/carusel"), fileName);
                    model.File.SaveAs(path);
                    carusel.ImgPath = "/Img/carusel/" + fileName;
                }
                db.SaveChanges();
                ViewBag.AlertMessage = "changes has ended succesfully";
                return RedirectToAction("List");
            }
            ViewBag.AlertMessage = "Something gone wrong. Try again please!";
            return RedirectToAction("List");
        }

        // GET: Carusels/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Carusel carusel = db.carusel.Find(id);
        //    if (carusel == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(carusel);
        //}

        // POST: Carusels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
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
            db.carusel.Remove(carusel);
            db.SaveChanges();
            return RedirectToAction("List");
        }

        [HttpPost]
        public ActionResult ChangeActivityStatus(string[] id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var carusels = db.carusel.ToList();
            try
            {
                foreach (var item in carusels)
                {
                    if (id.Any(a => a == item.Id.ToString())) { item.isActive = true; }
                    else { item.isActive = false; }
                    if (String.IsNullOrEmpty(item.ImgPath)) { item.isActive = false; }
                }
            }
            catch (Exception)
            {
                return Json(Url.Action("List", "Carusels"));
            }
            db.SaveChanges();
            return Json(Url.Action("List", "Carusels"));
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
