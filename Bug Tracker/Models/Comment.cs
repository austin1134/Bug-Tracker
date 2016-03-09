using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bug_Tracker.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string TicketId { get; set; }
        public string CommentBody { get; set; }
        public DateTime CreationDate { get; set; }

        public virtual Ticket Ticket { get; set; }
        public virtual ApplicationUser Creator { get; set; }
    }
}