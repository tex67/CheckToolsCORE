

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using CheckToolsCORE.OmaData.Context;
using Microsoft.AspNetCore.Identity;
using CheckToolsCORE.OmaData.DbModel;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using CheckTools.Models;
using CheckToolsCORE.Models;

namespace CheckToolsCORE.Services
{
    public class oldUserService
    {
        public static bool IsUserInRole(IPrincipal  user, string role)
        {
           
            ApplicationDbContext context = new ApplicationDbContext();
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var idUser = user.Identity.GetUserId();
            return UserManager.IsInRole(idUser, role);
            
        }
        public static IList<string> UserRoles(IPrincipal user)
        {

            ApplicationDbContext context = new ApplicationDbContext();
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var idUser = user.Identity.GetUserId();
            return UserManager.GetRoles(idUser);
        }

        public static List<EditUserViewModel> GetAllUserWithRoles() 
        {
            var Db = new ApplicationDbContext();
            var users = Db.Users;
            var model = new List<EditUserViewModel>();
            foreach (var user in users)
            {
                var u = new EditUserViewModel(user);
                model.Add(u);
            }
            return model;
        }
    }
}