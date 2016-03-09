using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bug_Tracker.Models
{
    public class TicketAttachment
    {
        public int Id { get; set; }
        public string AuthorId { get; set; }
        public string TicketId { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public DateTime CreationDate { get; set; }

        public virtual Ticket Ticket { get; set; }
    }
}