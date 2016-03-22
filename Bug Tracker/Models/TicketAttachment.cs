using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bug_Tracker.Models
{
    public class TicketAttachment
    {
        public int Id { get; set; }
        public int TicketId { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public DateTimeOffset CreationDate { get; set; }

        public string AttacherId { get; set; }
        public virtual ApplicationUser Attacher { get; set; }

        public virtual Ticket Ticket { get; set; }
    }
}