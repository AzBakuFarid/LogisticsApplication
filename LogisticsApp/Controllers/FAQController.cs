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
    public class FAQController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult List()
        {
            return View(db.forum.ToList());
        }

        // GET: FAQ
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View(db.forum.Where(w => w.isActive == true).ToList());
        }

        // GET: FAQ/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FAQ model = db.forum.Find(id);
            if (model == null)
            {
                return HttpNotFound();
            }
            return Json(new { Question = model.Question, Id = model.Id, Answer = model.Answer }, JsonRequestBehavior.AllowGet);
        }

        // GET: FAQ/Create
        public ActionResult Create()
        {
            return PartialView(new ForumViewModel());
        }

        // POST: FAQ/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Question,Answer")] ForumViewModel model)
        {
            if (ModelState.IsValid)
            {
                FAQ forum = new FAQ();
                forum.Question = model.Question;
                forum.Answer = model.Answer;
                db.forum.Add(forum);
                db.SaveChanges();
                ViewBag.AlertMessage = "creation has ended succesfully";
                return RedirectToAction("List");
            }

            ViewBag.AlertMessage = "Something gone wrong. Try again please!";
            return RedirectToAction("List");
        }

        // GET: FAQ/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    FAQ fAQ = db.forum.Find(id);
        //    if (fAQ == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(fAQ);
        //}

        // POST: FAQ/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Question,Answer")] ForumViewModel model)
        {
            if (ModelState.IsValid)
            {
                FAQ forum;
                try
                {
                    forum = db.forum.Single(s => s.Id == model.Id);
                    if (forum == null)
                    {
                        return HttpNotFound();
                    }
                    forum.Answer = model.Answer;
                    forum.Question = model.Question;
                    db.SaveChanges();
                    ViewBag.AlertMessage = "changes has ended succesfully";
                    return RedirectToAction("List");
                }
                catch (Exception)
                {
                    ViewBag.AlertMessage = "Something gone wrong. Try again please!";
                    return RedirectToAction("List");
                }
            }
            ViewBag.AlertMessage = "Something gone wrong. Try again please!";
            return RedirectToAction("List");
        }

        // GET: FAQ/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    FAQ fAQ = db.forum.Find(id);
        //    if (fAQ == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(fAQ);
        //}

        // POST: FAQ/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FAQ model = db.forum.Find(id);
            if (model == null)
            {
                return HttpNotFound();
            }
            db.forum.Remove(model);
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
            var forums = db.forum.ToList();
            try
            {
                foreach (var item in forums)
                {
                    if (id.Any(a => a == item.Id.ToString())) { item.isActive = true; }
                    else { item.isActive = false; }
                }
            }
            catch (Exception)
            {
                return Json(Url.Action("List", "FAQ"));
            }
            db.SaveChanges();
            return Json(Url.Action("List", "FAQ"));
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
