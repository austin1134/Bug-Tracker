using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Bug_Tracker.Models
{
        public class UserDropDownViewModel
        {
            public string Id { get; set; }
            public string Name { get; set; }
        }

        public class ProjectUsersViewModel
        {
            public int ProjectId { get; set; }
            [Display(Name = "Project Name")]
            public string ProjectName { get; set; }
            [Display(Name = "Assigned Developers")]
            public System.Web.Mvc.MultiSelectList Developers { get; set; }
            public string[] SelectedDevelopers { get; set; }
            [Display(Name = "Project Manager")]
            public SelectList ProjectManagers { get; set; }
            public string SelectedProjectManagerId { get; set; }
        }
    }
