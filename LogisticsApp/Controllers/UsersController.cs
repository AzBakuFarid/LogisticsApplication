using LogisticsApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace LogisticsApp.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: Users
        public ActionResult ListUsers()
        {
            var role = db.Roles.Single(s => s.Name.Equals("User"));
            var model = db.Users.Where(w=>w.Roles.Count==1&&w.Roles.Any(a=>a.RoleId==role.Id)).ToList();
            return View(model);
        }

        public ActionResult ListAdmins()
        {
            var userrole = db.Roles.Single(s => s.Name.Equals("User")).Id;
            var model = db.Users.Where(w => !w.Roles.Any(a=>a.RoleId==userrole)).ToList();
            return View(model);
        }

        public ActionResult AdminUserDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser user = db.Users.FirstOrDefault(f=>f.CustomerNumber==id);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.OrderCounter = db.orders.Where(w => w.customer.CustomerNumber == id).Count();
            ViewBag.BundleCounter = db.bundles.Where(w => w.Customer.CustomerNumber == id).Count();
            ViewBag.Roles=db.Roles.Where(w=>!w.Name.Equals("User")).ToList();
            var role = user.Roles.First().RoleId;
            var curRole = db.Roles.Single(s=>s.Id.Equals(role));
            ViewBag.UserCurRole = curRole.Name;

            return View(user);
        }
    }
}