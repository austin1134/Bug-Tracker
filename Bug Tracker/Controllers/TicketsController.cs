using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
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

            Ticket ticket = db.Tickets.Include(t => t.Comments).Include(t => t.Attachments).FirstOrDefault(t => t.Id == (int)id);
            UserRolesHelper helper = new UserRolesHelper(db);
            var developers = helper.UsersInRole("Developer");
            ViewBag.DeveloperId = new SelectList(developers, "Id", "UserName");
            ticket.TicketChanges = db.TicketChanges.Where(t => t.TicketId == id).OrderBy(t => t.ChangeDate).ToList();

            var currentDeveloper = db.Users.FirstOrDefault(u => u.Id == ticket.DeveloperId);
            if (currentDeveloper != null)
            {
                ViewBag.AssignedDeveloper = currentDeveloper.UserName;
            }

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
                    //var oldTicket = (Ticket)TempData["oldticket"];
                    //TicketNotification notification = null;

                    //if (oldTicket.DeveloperId != ticket.DeveloperId)
                    //{
                    //    db.TicketChanges.Add(new TicketChange
                    //    {
                    //        Property = "Developer",
                    //        ChangeDate = DateTimeOffset.Now,
                    //        UserId = User.Identity.GetUserId(),
                    //        TicketId = ticket.Id,
                    //        OldValue = oldTicket.Developers.UserName,
                    //        NewValue = db.Users.Find(ticket.DeveloperId).UserName
                    //    });
                    //    notification = await Notify(ticket);
                    //}
                    //ticket.Updated = new DateTimeOffset(DateTime.Now);
                    //if (notification != null)
                    //{
                    //    //notification was sent, so log it to the db
                    //    //db.TicketNotification.Add(notification);
                    //}

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
        public async Task<ActionResult> Edit(int? id)
        {
            UserRolesHelper helper = new UserRolesHelper(db);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = await db.Tickets.FindAsync(id);
            TempData["oldticket"] = ticket;

            if (ticket == null)
            {
                return HttpNotFound();
            }
            //ViewBag.AssignedUSerId = new SelectList(helper.ListUsersOnProject(ticket.ProjectId), "Id", "Name",
            //    ticket.DeveloperId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", ticket.ProjectId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Priority", ticket.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatuses, "Id", "Status", ticket.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId);
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Title,Description,AuthorId,DeveloperId,ProjectId,TicketTypeId,TicketPriorityId,TicketStatusId,CreationDate")] Ticket ticket)
        {
            UserRolesHelper helper = new UserRolesHelper(db);
            if (ModelState.IsValid)
            {
                var oldTicket = (Ticket) TempData["oldticket"];
                TicketNotification notification = null;
                //determine whats changed and for each changed property, add a new TicketHistory entry
                // to the DB and save the changes again
                if (oldTicket.Description != ticket.Description)
                {
                    db.TicketChanges.Add(new TicketChange
                    {
                        Property = "Description",
                        ChangeDate = DateTimeOffset.Now,
                        UserId = User.Identity.GetUserId(),
                        TicketId = ticket.Id,
                        OldValue = oldTicket.Description,
                        NewValue = ticket.Description
                    });
                    notification = await Notify(ticket);
                }

                if (oldTicket.ProjectId != ticket.ProjectId)
                {
                    db.TicketChanges.Add(new TicketChange
                    {
                        Property = "Project",
                        ChangeDate = DateTimeOffset.Now,
                        UserId = User.Identity.GetUserId(),
                        TicketId = ticket.Id,
                        OldValue = oldTicket.Projects.Name,
                        NewValue = db.Projects.Find(ticket.ProjectId).Name
                    });
                    notification = await Notify(ticket);
                }

                if (oldTicket.TicketTypeId != ticket.TicketTypeId)
                {
                    db.TicketChanges.Add(new TicketChange
                    {
                        Property = "Type",
                        ChangeDate = DateTimeOffset.Now,
                        UserId = User.Identity.GetUserId(),
                        TicketId = ticket.Id,
                        OldValue = oldTicket.TicketType.Name,
                        NewValue = db.TicketTypes.Find(ticket.TicketTypeId).Name
                    });
                    notification = await Notify(ticket);
                }

                if (oldTicket.TicketPriorityId != ticket.TicketPriorityId)
                {
                    db.TicketChanges.Add(new TicketChange
                    {
                        Property = "Priority",
                        ChangeDate = DateTimeOffset.Now,
                        UserId = User.Identity.GetUserId(),
                        TicketId = ticket.Id,
                        OldValue = oldTicket.TicketPriority.Priority,
                        NewValue = db.TicketPriorities.Find(ticket.TicketPriorityId).Priority
                    });
                    notification = await Notify(ticket);
                }

                if (oldTicket.TicketStatusId != ticket.TicketStatusId)
                {
                    db.TicketChanges.Add(new TicketChange
                    {
                        Property = "Status",
                        ChangeDate = DateTimeOffset.Now,
                        UserId = User.Identity.GetUserId(),
                        TicketId = ticket.Id,
                        OldValue = oldTicket.TicketStatus.Status,
                        NewValue = db.TicketStatuses.Find(ticket.TicketStatusId).Status
                    });
                    notification = await Notify(ticket);
                }

                if (oldTicket.DeveloperId != ticket.DeveloperId)
                {
                    db.TicketChanges.Add(new TicketChange
                    {
                        Property = "Developer",
                        ChangeDate = DateTimeOffset.Now,
                        UserId = User.Identity.GetUserId(),
                        TicketId = ticket.Id,
                        OldValue = oldTicket.Developers.UserName,
                        NewValue = db.Users.Find(ticket.DeveloperId).UserName
                    });
                    notification = await Notify(ticket);
                }
                ticket.Updated = new DateTimeOffset(DateTime.Now);
                if (notification != null)
                {
                    //notification was sent, so log it to the db
                    //db.TicketNotification.Add(notification);
                }
                db.Entry(ticket).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            //ViewBag.DeveloperId = new SelectList(helper.ListUsersOnProject(ticket.ProjectId), "Id", "Name",
            //    ticket.DeveloperId);
            ViewBag.Projects = new SelectList(db.Projects, "Id", "Name", ticket.ProjectId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatuses, "Id", "Name", ticket.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId);

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

        private async Task<TicketNotification> Notify(Ticket ticket)
        {
            string body = null;
            ApplicationUser toUser = null;
            var userId = User.Identity.GetUserId();
            var fromUser = db.Users.FirstOrDefault(u => u.Id == userId);
            var subject = "changes to Ticket: " + ticket.Title;

            if (userId != ticket.DeveloperId)
            {
                //person making the change is NOT the assigned developer, so notify the developer
                toUser = db.Users.Find(ticket.DeveloperId);
                //Build the mail message
                body = "<p>" + toUser.FirstName + ",<br/>" + fromUser.FirstName + fromUser.LastName +
                       " has made some changes to a ticket assigned to you. Please check your assigned tickets to view these changes. </p>";
            }
            else
            {
                //person making the change is the assigned developer, so notify the PM
                toUser = db.Projects.Find(ticket.ProjectId).ProjectManager;
                body = "<p>" + toUser.FirstName + ",<br/>" + fromUser.FirstName + fromUser.LastName +
                       " hase made some changes to an assigned ticket for project " +
                       db.Projects.Find(ticket.ProjectId).Name + ".</p>";
            }

            EmailService e = new EmailService();
            await e.SendAsync(new IdentityMessage {Subject = subject, Body = body, Destination = toUser.Email});

            //if we get this far, we've successfully send the notification, so create a new entry for the db

            return new TicketNotification
            {
                Sent = new DateTimeOffset(DateTime.Now),
                TicketId = ticket.Id,
                FromUserId = fromUser.Id,
                ToUserId = toUser.Id
            };
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
