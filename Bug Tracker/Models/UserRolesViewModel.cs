﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bug_Tracker.Models
{
    public class UserRolesViewModel
    {
        public List<ApplicationUser> AllUsers { get; set; }
        public ICollection<UserDropDownViewModel> Submitters { get; set; }
        public List<ApplicationUser> Developers { get; set; }
        public List<ApplicationUser> ProjectManagers { get; set; }
        public List<ApplicationUser> Admins { get; set; }
    }
}