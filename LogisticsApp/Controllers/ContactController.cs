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
    public class ContactController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Contact
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View(db.contacts.FirstOrDefault());
        }
        
        public ActionResult List()
        {
            return View(db.contacts.FirstOrDefault());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Address,Phonenumber,Mobilenumber,Email,Facebook,Instagram")] Contact model)
        {
            if (ModelState.IsValid)
            {
                var contact = db.contacts.FirstOrDefault();
                if (contact.Equals(null))
                {
                    contact = new Contact();
                    contact.Address = model.Address;
                    contact.Phonenumber = model.Phonenumber;
                    contact.Mobilenumber = model.Phonenumber;
                    contact.Facebook = model.Facebook;
                    contact.Instagram = model.Instagram;
                    contact.Email = model.Email;
                    db.contacts.Add(contact);
                    
                }
                else {
                    contact.Address = model.Address;
                    contact.Phonenumber = model.Phonenumber;
                    contact.Mobilenumber = model.Mobilenumber;
                    contact.Facebook = model.Facebook;
                    contact.Instagram = model.Instagram;
                    contact.Email = model.Email;
                    
                }
                   db.SaveChanges();
           
                
            }
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
