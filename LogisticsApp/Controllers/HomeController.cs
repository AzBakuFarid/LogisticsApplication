using LogisticsApp.Models;
using Microsoft.AspNet.Identity;
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
            ViewBag.steps = db.steps.Where(w => w.isActive == true).OrderBy(o => o.StepCounter).ToList();
            return View();
        }

        public ActionResult RootTo(int? id)
        {

            return RedirectToAction("Details", "News", new { id = id });
        }

        
       
        public ActionResult AdminPanel()
        {
            return View("AdminEmpty"); // helelik bunu yaziram sonra pozmaq lazimdi
            if (Request.IsAuthenticated)
            {

                if (!User.IsInRole("Admin"))
                {
                    return HttpNotFound();
                }
                return RedirectToAction("AdminEmpty");
            }

            return HttpNotFound();
        }

        public ActionResult AdminEmpty() {

            return View();
        }


    }
}