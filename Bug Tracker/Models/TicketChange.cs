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
        public string Property { get; set; }
        public string UserId { get; set; }
        public DateTimeOffset ChangeDate { get; set; }

        public string OldAssignedDeveloper { get; set; }
        public string OldStatus { get; set; }
        public string OldType { get; set; }
        public string OldPriority { get; set; }
        public string NewStatus { get; set; }
        public string NewType { get; set; }
        public string NewPriority { get; set; }
        public string NewAssignedDeveloper { get; set; }
        public string EditorName { get; set; }

        public string AllChanges { get; set; }

        public virtual Ticket Ticket { get; set; }
    }
}