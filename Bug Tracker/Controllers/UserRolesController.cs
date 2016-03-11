using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Bug_Tracker.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Bug_Tracker.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserRolesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: UserRoles
        public ActionResult Index()
        {
            UserRolesViewModel model = new UserRolesViewModel();
            UserRolesHelper helper = new UserRolesHelper(db);

            model.AllUsers = db.Users.ToList();
            model.Submitters = helper.UsersInRole("Submitter");
            

            return View(model);
        }

//        // GET: UserRoles/Edit
//        public ActionResult EditUserRoles(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            ApplicationUser user = db.Users.FirstOrDefault(x => x.UserName == User.Identity.Name);

//            ApplicationUserManager usermanager = new ApplicationUserManager();
//            usermanager.GetRoles(user);
//            if (user == null)
//            {
//                return HttpNotFound();
//            }
//            return View();
//        }

//        // POST: UserRoles/Edit
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult EditUserRoles([Bind(Include = "Id,Name,ProjectManagerId")] UserRolesViewModel userRoles)
//        {
//            if (ModelState.IsValid)
//            {
//                db.Entry(userRoles).State = EntityState.Modified;
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }
//            ViewBag.ProjectManagerId = new SelectList(db.Users, "Id", "FirstName", userRoles.);
//            return View();
//        }

    }
}