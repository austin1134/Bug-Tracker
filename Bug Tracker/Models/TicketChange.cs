using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services.Configuration;

namespace Bug_Tracker.Models
{
    public class TicketChange
    {
        public int Id { get; set; }
        public int TicketId { get; set; }
        public DateTime ChangeDate { get; set; }
        public int EditorId { get; set; }
        public int NewDeveloperId { get; set; }
        public int OldDeveloperId { get; set; }

        public virtual TicketStatus OldStatus { get; set; }
        public virtual TicketType OldType { get; set; }
        public virtual TicketPriority OldPriority { get; set; }
        public virtual TicketStatus NewStatus { get; set; }
        public virtual TicketType NewType { get; set; }
        public virtual TicketPriority NewPriority { get; set; }
    }
}