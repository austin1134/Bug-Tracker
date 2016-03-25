using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bug_Tracker.Models;
using Microsoft.AspNet.Identity;

namespace Bug_Tracker.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            string currentuser = User.Identity.GetUserId();
            return View(new DashBoardViewModel
            {
                ImmediateAttentionTickets = db.Tickets.Where(t => t.TicketPriorityId == 3 && t.DeveloperId == null)
                    .OrderByDescending(t => t.CreationDate).ToList(),
                UnassignedTickets = db.Tickets.Where(t => t.DeveloperId == null & t.Projects.ProjectManagerId == currentuser)
                    .OrderByDescending(t => t.CreationDate).ToList(),
                Projects = db.Projects.OrderByDescending(t => t.CreationDate).ToList(),
                Tickets = db.Tickets.OrderByDescending(t => t.CreationDate)
            });
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult LandingPage()
        {
            ViewBag.Message = "FacePalm landing page.";

            return View();
        }
    }
}