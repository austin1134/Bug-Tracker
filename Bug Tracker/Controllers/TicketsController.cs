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

namespace Bug_Tracker.Controllers
{
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
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        // GET: Tickets/Create
        public ActionResult Create()
        {
            ViewBag.Projects = new SelectList(db.Projects, "Id", "Name");
            ViewBag.TicketType = new SelectList(db.TicketTypes, "Id", "Name");
            ViewBag.TicketPriority = new SelectList(db.TicketPriorities, "Id", "Name");

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
                    var authorId = user.UserName;
                    ticket.AuthorId = user.UserName;
                    ticket.CreationDate = DateTime.Now;

                    db.Tickets.Add(ticket);
                    db.SaveChanges();
                    return RedirectToAction("Details", "Tickets", new { id = user.Tickets });
                }
            }

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
        public ActionResult Edit([Bind(Include = "Id,Title,Description,AuthorId,DeveloperId,ProjectId,CreationDate")] Ticket ticket)
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
