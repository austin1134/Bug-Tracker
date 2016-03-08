using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bug_Tracker.Models
{
    public class UserRoles
    {
        public List<ApplicationUser> Users { get; set; }
        public List<ApplicationUser> Submitters { get; set; }
        public List<ApplicationUser> Developers { get; set; }
        public List<ApplicationUser> ProjectManagers { get; set; }
        public List<ApplicationUser> Admins { get; set; }
    }
}