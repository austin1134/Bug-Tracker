using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bug_Tracker.Models
{
    public class TicketsViewModel
    {
        public List<Ticket> AllTickets = new List<Ticket>();
        public List<Ticket> SubmitterTickets = new List<Ticket>();
        public List<Ticket> AssignedDeveloperTickets = new List<Ticket>();
        public List<Ticket> DeveloperProjectTickets = new List<Ticket>();
        public List<Ticket> ProjectManagerTickets = new List<Ticket>();
    }
}