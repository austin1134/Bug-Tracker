using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Bug_Tracker.Models
{
    public class Project
    {
        public Project()
        {
            this.Developers = new HashSet<ApplicationUser>();
            this.Tickets = new HashSet<Ticket>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTimeOffset CreationDate { get; set; }
        public string ProjectManagerId { get; set; }

        [InverseProperty("ProjectManagerProjects")]
        public virtual ApplicationUser ProjectManager { get; set; }

        [InverseProperty("DeveloperProjects")]
        public virtual ICollection<ApplicationUser> Developers { get; set; }
        public virtual ICollection<Ticket> Tickets { get; set; }

    }
}