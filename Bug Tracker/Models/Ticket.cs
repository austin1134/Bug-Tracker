using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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
        [AllowHtml]
        public string Description { get; set; }
        public string AuthorId { get; set; }
        public string DeveloperId { get; set; }
        public int ProjectId { get; set; }
        public DateTimeOffset CreationDate { get; set; }

        public int TicketTypeId { get; set; }
        public int? TicketPriorityId { get; set; }
        public int? TicketStatusId { get; set; }

        public virtual ApplicationUser Submitters { get; set; }
        public virtual ApplicationUser Developers { get; set; }
        public virtual Project Projects { get; set; }
        public virtual TicketType TicketType { get; set; }
        public virtual TicketPriority TicketPriority { get; set; }
        public virtual TicketStatus TicketStatus { get; set; }

        public ICollection<TicketChange> TicketChanges { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<TicketAttachment> Attachments { get; set; }
        public DateTimeOffset Updated { get; set; }
    }
}