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
        private UserRolesHelper rolesHelper;

        public UserRolesController()
        {
            this.rolesHelper = new UserRolesHelper(db);    
        }

        // GET: UserRoles
        public ActionResult Index()
        {
            var roles = db.Roles.ToList();
            List<RolesViewModel> model = new List<RolesViewModel>();
            foreach (var r in roles)
            {
                model.Add(new RolesViewModel { RoleId = r.Id, RoleName = r.Name });
            }
            return View(model);
        }

        // Get: UserRoles/Edit
        public ActionResult EditUserRoles(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var role = db.Roles.Find(id);
            if (role == null)
            {
                return HttpNotFound();
            }

            var model = new UserRolesViewModel();
            model.RoleId = id;
            model.RoleName = role.Name;
            model.SelectedUsers = role.Users.Select(u => u.UserId).ToArray();
            var availableUsers = db.Users;
            model.Users = new MultiSelectList(availableUsers, "Id", "UserName", null);

            return View(model);
        }

        // POST: UserRoles/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditUserRoles([Bind(Include = "RoleId, RoleName, SelectedUsers")] UserRolesViewModel model)
        {
            if (ModelState.IsValid)
            {
                var role = db.Roles.Find(model.RoleId);
                var users = rolesHelper.UsersInRole(role.Name);
                // remove unselected users
                foreach (var u in users)
                {
                    if (!model.SelectedUsers.Contains(u.Id))
                    {
                        rolesHelper.RemoveUserFromRole(u.Id, role.Name);
                    }
                }
                //add newly selected users
                foreach (var id in model.SelectedUsers)
                {
                    if (!rolesHelper.IsUserInRole(id, role.Name))
                    {
                        rolesHelper.AddUserToRole(id, role.Name);
                    }
                }
                return RedirectToAction("Index");
            }
            return View(model);
        }

    }
}