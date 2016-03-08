using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services.Configuration;

namespace Bug_Tracker.Models
{
    public class Ticket
    {
        public Ticket()
        {
            this.TicketChanges = new HashSet<TicketChange>();
            this.Comments = new HashSet<Comment>();
            this.Attachments = new HashSet<TicketAttachment>();
        }
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int AuthorId { get; set; }
        public int DeveloperId { get; set; }
        public int ProjectId { get; set; }
        public DateTime CreationDate { get; set; }

        public virtual ApplicationUser Submitters { get; set; }
        public virtual ApplicationUser Developers { get; set; }
        public virtual Project Projects { get; set; }

        public ICollection<TicketChange> TicketChanges { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<TicketAttachment> Attachments { get; set; }
    }
}