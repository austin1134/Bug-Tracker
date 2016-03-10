using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bug_Tracker.Models;

namespace Bug_Tracker.Controllers
{
    [Authorize (Roles = "Admin")]   
    public class UserRolesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: UserRoles
        public ActionResult Index()
        {
            UserRolesViewModel model = new UserRolesViewModel();
            UserRolesHelper helper = new UserRolesHelper();

            List<ApplicationUser> AllUsers = db.Users.ToList();
            model.AllUsers = new List<ApplicationUser>();
            model.Submitters = new List<ApplicationUser>();

            foreach (ApplicationUser user in AllUsers)
            { 
            {
                model.AllUsers.Add(user);
            }
            return View(model);
            }

            foreach (ApplicationUser user in AllUsers)
            {
                if (helper.IsUserInRole(user.Id, "Submitter"))
                {
                    model.Submitters.Add(user);
                }
            }

           return View(model);
        }

        // GET: Tickets/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Description,AuthorId,DeveloperId,ProjectId,CreationDate")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                var user = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
                if (user != null)

                {
                    //user.Tickets.Add(ticket.AuthorId);
                    ticket.AuthorId = user.Id;
                    ticket.CreationDate = DateTime.Now;

                    db.Tickets.Add(ticket);
                    db.SaveChanges();
                    return RedirectToAction("Details", "Tickets", new { id = user.Tickets });
                }
            }

            return View(ticket);
        }
    }
}