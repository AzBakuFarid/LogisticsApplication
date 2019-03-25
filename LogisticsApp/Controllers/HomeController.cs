using LogisticsApp.Models;
using LogisticsApp.Models.Page;
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
            ViewBag.countries=db.countries.Where(w => w.isActive == true).ToList();
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


      
        public ActionResult Search(string text)
        {
            if (!String.IsNullOrWhiteSpace(text))
            {
                IList<Search> model = new List<Search>();
                try
                {
                    var news = db.news.Where(w => w.isActive == true && (w.Title.Contains(text) || w.Text.Contains(text))).
                                    Select(s => new { Text = s.Text, Link = "/News/Details/" + s.Id }).ToList();
                    var faqs = db.forum.Where(w => w.isActive == true && (w.Question.Contains(text) || w.Answer.Contains(text))).
                         Select(s => new { Text = s.Question, Link = "/FAQ/Index/" }).ToList();
                    foreach (var item in news.Concat(faqs).ToList())
                    {
                        model.Add(new Search { Text = item.Text, Link = item.Link });
                    }

                }
                catch (Exception)
                {
                }
                return View(model);
            }
            return RedirectToAction("Index","Home");
        }
    }
}