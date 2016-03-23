namespace Bug_Tracker.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Bug_Tracker.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<Bug_Tracker.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(Bug_Tracker.Models.ApplicationDbContext context)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                roleManager.Create(new IdentityRole { Name = "Admin" });
            }
            if (!context.Roles.Any(r => r.Name == "Project Manager"))
            {
                roleManager.Create(new IdentityRole { Name = "Project Manager" });
            }
            if (!context.Roles.Any(r => r.Name == "Developer"))
            {
                roleManager.Create(new IdentityRole { Name = "Developer" });
            }
            if (!context.Roles.Any(r => r.Name == "Submitter"))
            {
                roleManager.Create(new IdentityRole { Name = "Submitter" });
            }

            var uStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(uStore);

            if (userManager.FindByEmail("austin.torres@colorado.edu") != null)
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "austin.torres@colorado.edu",
                    Email = "austin.torres@colorado.edu",
                    FirstName = "Austin",
                    LastName = "Torres",

                }, "TA1234ta!!");
            }

            var userId = userManager.FindByEmail("austin.torres@colorado.edu").Id;
            userManager.AddToRole(userId, "Admin");
            {
                userManager.AddToRole(userId, "Admin");
            }

            //add Demo Admin to the database
            if (userManager.FindByEmail("DemoAdmin@FacePalm.com") == null)
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "DemoAdmin",
                    Email = "DemoAdmin@FacePalm.com",
                    FirstName = "Demo",
                    LastName = "Admin"
                }, "Password-1");
            }

            userId = userManager.FindByEmail("DemoAdmin@FacePalm.com").Id;
            if (!userManager.IsInRole(userId, "Admin"))
            {
                userManager.AddToRole(userId, "Admin");
            }

            //add Demo Project Manager to the database
            if (userManager.FindByEmail("DemoProjectManager@FacePalm.com") == null)
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "DemoProjectManager",
                    Email = "DemoProjectManager@FacePalm.com",
                    FirstName = "Project",
                    LastName = "Manager"
                }, "Password-1");
            }

            userId = userManager.FindByEmail("DemoProjectManager@FacePalm.com").Id;
            if (!userManager.IsInRole(userId, "Project Manager"))
            {
                userManager.AddToRole(userId, "Project Manager");
            }

            //add Demo Developer to the database
            if (userManager.FindByEmail("DemoDeveloper@FacePalm.com") == null)
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "DemoDeveloper",
                    Email = "DemoDeveloper@FacePalm.com",
                    FirstName = "Demo",
                    LastName = "Developer"
                }, "Password-1");
            }

            userId = userManager.FindByEmail("DemoDeveloper@FacePalm.com").Id;
            if (!userManager.IsInRole(userId, "Developer"))
            {
                userManager.AddToRole(userId, "Developer");
            }

            //add Demo Submitter to the database
            if (userManager.FindByEmail("DemoSubmitter@FacePalm.com") == null)
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "DemoSubmitter",
                    Email = "DemoSubmitter@FacePalm.com",
                    FirstName = "Demo",
                    LastName = "Submitter"
                }, "Password-1");
            }

            userId = userManager.FindByEmail("DemoSubmitter@FacePalm.com").Id;
            if (!userManager.IsInRole(userId, "Submitter"))
            {
                userManager.AddToRole(userId, "Submitter");
            }

            //add Bobby to the database
            //if (userManager.FindByEmail("bdavis@coderfoundry.com") == null)
            //{
            //    userManager.Create(new ApplicationUser
            //    {
            //        UserName = "bdavis@coderfoundry.com",
            //        Email = "bdavis@coderfoundry.com",
            //        FirstName = "Bobby",
            //        LastName = "Davis"
            //    }, "Password-1");
            //}

            //userId = userManager.FindByEmail("bdavis@coderfoundry.com").Id;
            //if (!userManager.IsInRole(userId, "Moderator"))
            //{
            //    userManager.AddToRole(userId, "Moderator");
            //}
        }
    }
}