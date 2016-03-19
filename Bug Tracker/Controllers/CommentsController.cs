using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Bug_Tracker.Models;

namespace BugTracker.Controllers
{
    [Authorize(Roles = "Developer, Admin, Project Manager")]
    public class CommentsController : Controller
    {
                private ApplicationDbContext db = new ApplicationDbContext();
                // GET: Comments
                public ActionResult _CommentsViewPartial()
                {
                    var comments = db.Comments.Include(c => c.Creator);
                    return View(comments.OrderByDescending(x => x.CreationDate).ToList());
                }

        //        // GET: Comments/Details/5
        //        public ActionResult Details(int? id)
        //        {
        //            if (id == null)
        //            {
        //                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //            }
        //            Comment comment = db.Comments.Find(id);
        //            if (comment == null)
        //            {
        //                return HttpNotFound();
        //            }
        //            return View(comment);
        //        }

        //        // GET: Comments/Create
        //        public ActionResult Create()
        //        {
        //            return View();
        //        }

        // POST: Comments/Create
        [HttpPost]
        public ActionResult Create(Comment comment)
        {
            Ticket ticket = db.Tickets.FirstOrDefault(x => x.Id == comment.TicketId);

            //only allow specified users to post comments, otherwise redirect to unauthorized error page
            if (User.IsInRole("Admin") || ticket.Projects.ProjectManager.UserName == User.Identity.Name || ticket.Developers.UserName == User.Identity.Name || ticket.Submitters.UserName == User.Identity.Name)
            {
                ApplicationUser user = db.Users.FirstOrDefault(x => x.UserName == User.Identity.Name);

                comment.CreationDate = DateTime.Now;
                comment.CreatorId = user.Id;

                db.Comments.Add(comment);
                db.SaveChanges();

                return RedirectToAction("Details", "Tickets", new { id = comment.TicketId });
            }

            else
            {
                return RedirectToAction("Unauthorized", "Error");
            }
        }
    


//        // GET: Comments/Delete/5
//        public ActionResult Delete(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            Comment comment = db.Comments.Find(id);
//            if (comment == null)
//            {
//                return HttpNotFound();
//            }
//            return View(comment);
//        }

//        // POST: Comments/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public ActionResult DeleteConfirmed(int id)
//        {
//            Comment comment = db.Comments.Find(id);
//            db.Comments.Remove(comment);
//            db.SaveChanges();
//            return RedirectToAction("Index");
//        }

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
