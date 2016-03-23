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

        public string EditorName { get; set; }

        public virtual Ticket Ticket { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
    }
}