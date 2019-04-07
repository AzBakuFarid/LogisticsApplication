using LogisticsApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace LogisticsApp.Controllers
{
    public class SettingsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Settings
        [HttpGet]
        public ActionResult Edit()
        {
            SettingsViewModel model = null;
            try
            {
                Settings settings = db.settings.FirstOrDefault();
                if (settings == null)
                {
                    settings = new Settings();
                    db.settings.Add(settings);
                    db.SaveChanges();
                }
                model = new SettingsViewModel
                {
                    Title = settings.Title,
                    ServiceTax = settings.ServiceTax,
                    Logo = settings.Logo,
                    UrgencyTax = settings.UrgencyTax,
                    valutas = db.valutas.Where(w => w.isActive == true).ToList()
                };
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SettingsEditModel model)
        {
            SettingsViewModel viewModel = null;
            try
            {
                var valutas = db.valutas.Where(w => w.isActive == true).ToList();
                viewModel = new SettingsViewModel
                {
                    Title = model.Title,
                    ServiceTax = model.ServiceTax,
                    UrgencyTax = model.UrgencyTax,
                    valutas = valutas
                };
                var set = db.settings.FirstOrDefault();
                if (set != null || !String.IsNullOrWhiteSpace(set.Logo))
                {
                    viewModel.Logo = set.Logo;
                }
                if (ModelState.IsValid)
                {
                    Settings settings = db.settings.FirstOrDefault();
                    if (settings == null)
                    {
                        settings = new Settings();
                    }
                    settings.ServiceTax = model.ServiceTax;
                    settings.UrgencyTax = model.UrgencyTax;
                    if (model.Title.regexControl(@"^\w+\.+[a-zA-Z]{2,3}$"))
                    {
                        settings.Title = model.Title;
                    }
                    else
                    {
                        ModelState.AddModelError(String.Empty, "Title is incorrect");
                        TempData["AlertMessage"] = "Something gone wrong. Try again please!";
                        viewModel.Logo = settings.Logo;
                        return View(viewModel);
                    }
                    var a = Request.Params.AllKeys.Where(w => w.IndexOf("valuta") >= 0);
                    foreach (var item in a)
                    {
                        valutas.Single(s => s.Name.ToUpper() == item.Substring(item.IndexOf("-") + 1)).Value = Double.Parse(Request.Params[item]);
                    }
                    if (model.Logo != null && model.Logo.ContentLength > 0)
                    {
                        var fileExtension = Path.GetExtension(model.Logo.FileName);
                        var fileName = Path.GetFileNameWithoutExtension(model.Logo.FileName) + "-" +
                            DateTime.Now.ToString("yyyyMMddHHmmssfff") + fileExtension;
                        var fullpath = Path.GetFullPath(Server.MapPath("/Img/"));
                        var directoryName = fullpath + "Logo";
                        Directory.CreateDirectory(directoryName);
                        var path = Path.Combine(Server.MapPath("/Img/" + "Logo"), fileName);
                        model.Logo.SaveAs(path);
                        settings.Logo = "/Img/Logo/" + fileName;
                    }
                    viewModel.Logo = settings.Logo;
                    db.SaveChanges();
                    TempData["AlertMessage"] = "Settings has changed succesfully";
                }
                else { throw new Exception(); }
            }
            catch (Exception)
            {
                TempData["AlertMessage"] = "Something gone wrong. Try again please!";
            }

            return View(viewModel);
        }
    }
}