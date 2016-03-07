using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bug_Tracker.Models
{
    public class TicketAttachment
    {
        public int Id { get; set; }
        public int AuthorId { get; set; }
        public int TicketId { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public DateTime CreationDate { get; set; }
    }
}