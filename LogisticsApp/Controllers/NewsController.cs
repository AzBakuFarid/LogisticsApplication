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
    [Authorize]
    public class NewsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: News
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View(db.news.Where(w=>w.isActive == true).ToList());
        }

        //GET: News
        public ActionResult List()
        {
            return View(db.news.ToList());
        }

        // GET: News/Details/5
        [AllowAnonymous]
        public ActionResult Details(int? id)
        {
            try
            {
                News news = db.news.Single(s => s.Id == id && s.isActive == true);
                ViewBag.LatesNews = db.news.Where(w => w.isActive == true).OrderByDescending(o => o.Created).Take(5).ToList();
                return View(news);
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        // GET: News/Create
        public ActionResult Create()
        {
            return PartialView(new NewsCreateModel());
        }

        // POST: News/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Title,Text,File")] NewsCreateModel model)
        {

            if (ModelState.IsValid)
            {
                News news = new News {
                    Title = model.Title,
                    Text = model.Text,
                    Created=DateTime.Now,
                    Modified=DateTime.Now
                };
                if (model.File != null && model.File.ContentLength > 0)
                {
                    var fileExtension = Path.GetExtension(model.File.FileName);
                    var fileName = Path.GetFileNameWithoutExtension(model.File.FileName) + "-" +
                        DateTime.Now.ToString("yyyyMMdd_HHmmssfff") + fileExtension;
                    var path = Path.Combine(Server.MapPath("~/Img/news"), fileName);
                    model.File.SaveAs(path);
                    news.PicturePath = "/Img/news/" + fileName;
                }
                db.news.Add(news);
                db.SaveChanges();
                TempData["AlertMessage"] = "The News has created succesfully";
                return RedirectToAction("List");
            }
            TempData["AlertMessage"] = "Something gone wrong. Try again please!";
            return RedirectToAction("List");
        }

        // GET: News/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            News news = db.news.Find(id);
            if (news == null)
            {
                return HttpNotFound();
            }
            NewsEditModel model = new NewsEditModel {
                Title=news.Title,
                Text=news.Text,
                Id=news.Id,
                ImagePath=news.PicturePath
            };
            return PartialView(model);
        }

        // POST: News/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Text,File")] NewsEditModel model)
        {
            if (ModelState.IsValid)
            {
                News news = db.news.Single(s => s.Id == model.Id);
                if (news == null)
                {
                    return HttpNotFound();
                }
                news.Title = model.Title;
                news.Text = model.Text;
                if (model.File != null && model.File.ContentLength > 0)
                {
                    var fileExtension = Path.GetExtension(model.File.FileName);
                    var fileName = Path.GetFileNameWithoutExtension(model.File.FileName) + "-" +
                        DateTime.Now.ToString("yyyyMMdd_HHmmssfff") + fileExtension;
                    var path = Path.Combine(Server.MapPath("~/Img/news"), fileName);
                    model.File.SaveAs(path);
                    news.PicturePath = "/Img/news/" + fileName;
                }
                news.Modified = DateTime.Now;
                db.SaveChanges();
                TempData["AlertMessage"] = "changes has ended succesfully";
                return RedirectToAction("List");
            }
            TempData["AlertMessage"] = "Something gone wrong. Try again please!";
            return RedirectToAction("List");
        }

        // POST: News/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            News news = db.news.Find(id);
            if (news == null)
            {
                return HttpNotFound();
            }
            db.news.Remove(news);
            db.SaveChanges();
            TempData["AlertMessage"] = "The News has deleted";
            return RedirectToAction("List");
        }

        [HttpPost]
        public ActionResult ChangeActivityStatus(string[] id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var news = db.news.ToList();
            try
            {
                foreach (var item in news)
                {
                    if (id.Any(a => a == item.Id.ToString())) { item.isActive = true; }
                    else { item.isActive = false; }
                    if (String.IsNullOrEmpty(item.PicturePath)) { item.isActive = false; }
                }
            }
            catch (Exception)
            {
                return Json(Url.Action("List", "News"));
            }
            db.SaveChanges();
            return Json(Url.Action("List", "News"));
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
