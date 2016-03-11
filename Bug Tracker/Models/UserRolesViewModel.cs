using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;

namespace Bug_Tracker.Models
{
    public class UserRolesViewModel
    {
        public List<ApplicationUser> AllUsers { get; set; }
        public string RoleId { get; set; }
        [Display(Name = "Role Name")]
        public string RoleName { get; set; }
        [Display(Name = "Assigned Users")]
        public System.Web.Mvc.MultiSelectList Users { get; set; }
        public string[] SelectedUsers { get; set; }
        //public ICollection<UserDropDownViewModel> Submitters { get; set; }
        //public List<ApplicationUser> Developers { get; set; }
        //public List<ApplicationUser> ProjectManagers { get; set; }
        //public List<ApplicationUser> Admins { get; set; }
    }

    public class RolesViewModel
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }
    }
}