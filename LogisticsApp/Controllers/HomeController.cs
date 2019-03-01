using LogisticsApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LogisticsApp.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            ViewBag.Carusel = db.carusel.Where(w => w.isActive == true).ToList();
            ViewBag.LatesNews = db.news.Where(w => w.isActive == true).OrderByDescending(o => o.Created).Take(3).ToList();
            ViewBag.steps = db.steps.Where(w => w.isActive == true).OrderBy(o=>o.StepCounter).ToList();
            return View();
        }

        public ActionResult RootTo(int? id)
        {
            
            return RedirectToAction("Details","News", new { id = id });
        }


    }
}