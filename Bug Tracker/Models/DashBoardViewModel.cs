using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bug_Tracker.Models
{
    public class DashBoardViewModel
    {
        public IEnumerable<Ticket> Tickets { get; set; }
        public IEnumerable<Ticket> UnassignedTickets { get; set; }
        public IEnumerable<Ticket> ImmediateAttentionTickets { get; set; }  
        public IEnumerable<Project> Projects { get; set; }
        public List<ApplicationUser> Developers { get; set; }
        public List<ApplicationUser> ProjectManagers { get; set; }
        public List<ApplicationUser> Admins { get; set; }
    }
}