using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Bug_Tracker.Models
{
    public class UserRolesHelper
    {
        private ApplicationDbContext db;
        private UserManager<ApplicationUser> userManager;
        private RoleManager<IdentityRole> roleManager;
        
        public UserRolesHelper(ApplicationDbContext context)
        {
            this.userManager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(context));
            this.roleManager = new RoleManager<IdentityRole>(
         new RoleStore<IdentityRole>(context));
            this.db = context;
        }

        public bool IsUserInRole(string userId, string roleName)
        {
            return userManager.IsInRole(userId, roleName);
        }

        public IList<string> ListUserRoles(string userId)
        {
            return userManager.GetRoles(userId);
        }

        public bool AddUserToRole(string userId, string roleName)
        {
            var result = userManager.AddToRole(userId, roleName);
            return result.Succeeded;
        }

        public bool RemoveUserFromRole(string userId, string roleName)
        {
            var result = userManager.RemoveFromRole(userId, roleName);
            return result.Succeeded;
        }

        public ICollection<UserDropDownViewModel> UsersInRole(string roleName)
        {
            var userIDs = roleManager.FindByName(roleName).Users.Select(r => r.UserId);
            var model = new List<UserDropDownViewModel>();
            var users = userManager.Users.Where(u => userIDs.Contains(u.Id));
            foreach(var u in users)
            {
                model.Add(new UserDropDownViewModel {Id = u.Id, UserName = u.UserName});
            }
            return model;
            //var userIDs = roleManager.FindByName(roleName).Users.Select(r => r.UserId);
            //return userManager.Users.Where(u => userIDs.Contains(u.Id)).Select(u =>
            //    new UserDropDownViewModel {Id = u.Id, UserName = u.UserName}).ToList();
        } 
    }
}