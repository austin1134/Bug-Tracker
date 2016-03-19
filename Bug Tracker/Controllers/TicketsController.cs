using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.DynamicData;
using System.Web.Mvc;
using Bug_Tracker.Models;
using Microsoft.AspNet.Identity;

namespace Bug_Tracker.Controllers
{
    [Authorize]
    public class TicketsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Tickets
        public ActionResult Index()
        {
            TicketsViewModel model = new TicketsViewModel();
            ApplicationUser user = db.Users.FirstOrDefault(x => x.UserName == User.Identity.Name);

            model.AllTickets = db.Tickets.ToList();

            if (User.IsInRole("Developer"))
            {
                //get a list of all tickets in projects in which user is a developer
                List<Project> developerProjects = user.DeveloperProjects.ToList();
                List<Ticket> developerProjectTickets = new List<Ticket>();
                List<Ticket> assignedDeveloperTickets = new List<Ticket>();
                List<Ticket> projectManagerTickets = new List<Ticket>();

                foreach (Project project in developerProjects)
                {
                    foreach (Ticket ticket in project.Tickets)
                    {
                        developerProjectTickets.Add(ticket);

                        //get a list of all tickets that the developer is assigned to solve
                        if (user.Id == ticket.DeveloperId)
                        {
                            assignedDeveloperTickets.Add(ticket);
                        }
                    }
                }

                if (User.IsInRole("Project Manager"))
                {
                    foreach (Ticket ticket in model.AllTickets)
                    {
                        if (ticket.Projects.ProjectManager.Id == user.Id)
                        {
                            projectManagerTickets.Add(ticket);
                        }
                    }
                }

                model.DeveloperProjectTickets = developerProjectTickets;
                model.AssignedDeveloperTickets = assignedDeveloperTickets;
                model.ProjectManagerTickets = projectManagerTickets;
            }

            //get a list of all projects in which the user is a submitter
            List<Ticket> submitterTickets = db.Tickets.Where(x => x.AuthorId == user.Id).ToList();
            model.SubmitterTickets = submitterTickets;

            return View(model);
        }

        // GET: Tickets/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Ticket ticket = db.Tickets.Find(id);
            UserRolesHelper helper = new UserRolesHelper(db);
            var developers = helper.UsersInRole("Developer");
            ViewBag.DeveloperId = new SelectList(developers, "Id", "UserName");

            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        // POST: Tickets/Details/AssignDeveloper
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AssignDeveloper([Bind(Include = "Id,Title,Description,AuthorId,DeveloperId,ProjectId,TicketTypeId,TicketPriorityId,CreationDate")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                {
                    db.Entry(ticket).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index", "Tickets");
                }
            }
            ViewBag.DeveloperId = new SelectList(db.Projects, "Id", "UserName", ticket.DeveloperId);
            return RedirectToAction("Details", "Tickets", new { id = ticket.Id});
        }

        // GET: Tickets/Create
        public ActionResult Create()
        {
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name");
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name");
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Priority");

            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Description,AuthorId,DeveloperId,ProjectId,TicketTypeId,TicketPriorityId,CreationDate")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                {
                    ticket.AuthorId = User.Identity.GetUserId();
                    ticket.CreationDate = DateTime.Now;

                    db.Tickets.Add(ticket);
                    db.SaveChanges();
                    return RedirectToAction("Index", "Tickets");
                }
            }
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", ticket.ProjectId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Priority", ticket.TicketPriorityId);

            return View(ticket);
        }

        // GET: Tickets/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Description,AuthorId,DeveloperId,ProjectId,TicketTypeId,TicketPriorityId,CreationDate")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ticket).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ticket);
        }

        // GET: Tickets/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Ticket ticket = db.Tickets.Find(id);
            db.Tickets.Remove(ticket);
            db.SaveChanges();
            return RedirectToAction("Index");
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
