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

            return View(new DashBoardViewModel
            {
                ImmediateAttentionTickets = db.Tickets.Where(t => t.TicketPriorityId == 3 && t.DeveloperId == null)
                    .OrderByDescending(t => t.CreationDate).ToList(),
                UnassignedTickets = db.Tickets.Where(t => t.DeveloperId == null)
                    .OrderByDescending(t => t.CreationDate).ToList(),
                UserInterfaceTickets = db.Tickets.Where(t => t.TicketTypeId == 1),
                PerformanceIssueTickets = db.Tickets.Where(t => t.TicketTypeId == 2),
                BrokenFunctionalityTickets = db.Tickets.Where(t => t.TicketTypeId == 3),
                OtherTickets = db.Tickets.Where(t => t.TicketTypeId == 4),
                ResolvedTickets = db.Tickets.Where(x => x.TicketStatusId == 2),
                AllTickets = db.Tickets,
                AllUsers = db.Users.ToList()
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