using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bug_Tracker.Models
{
    public class DashBoardViewModel
    {
        public IQueryable<Ticket> UserInterfaceTickets { get; set; }
        public IQueryable<Ticket> PerformanceIssueTickets { get; set; }
        public IQueryable<Ticket> BrokenFunctionalityTickets { get; set; }
        public IQueryable<Ticket> OtherTickets { get; set; }

        public IEnumerable<Ticket> ResolvedTickets { get; set; }
        public IEnumerable<Ticket> UnassignedTickets { get; set; }
        public IEnumerable<Ticket> ImmediateAttentionTickets { get; set; }
        public IEnumerable<Ticket> AllTickets { get; set; }   
        public List<ApplicationUser> AllUsers { get; set; }
    }
}