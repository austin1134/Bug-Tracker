﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bug_Tracker.Models
{
    public class TicketPriority
    {
        public TicketPriority()
        {
            this.Tickets = new HashSet<Ticket>();
        }
        public int Id { get; set; }
        public int Priority { get; set; }

        public virtual ICollection<Ticket> Tickets { get; set; } 
    }
}