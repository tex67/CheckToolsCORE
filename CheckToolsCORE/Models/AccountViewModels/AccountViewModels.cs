

using CheckToolsCORE.OmaData.Context;
using CheckToolsCORE.OmaData.DbModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace CheckToolsCORE.Models.AccountViewModels
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Posta elettronica")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Codice")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Memorizzare questo browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Posta elettronica")]
        public string Email { get; set; }
    }

    //public class LoginViewModel
    //{
    //    [Required]
    //    [Display(Name = "Posta elettronica")]
    //    [EmailAddress]
    //    public string Email { get; set; }

    //    [Required]
    //    [DataType(DataType.Password)]
    //    [Display(Name = "Password")]
    //    public string Password { get; set; }

    //    [Display(Name = "Memorizza account")]
    //    public bool RememberMe { get; set; }
    //}





   
    public class RoleViewModel
    {
        public string RoleName { get; set; }
        public string Description { get; set; }

        public RoleViewModel() { }
        public RoleViewModel(ApplicationRole role)
        {
            this.RoleName = role.Name;
            this.Description = role.Description;
        }
    }
    public class EditRoleViewModel
    {
        public string OriginalRoleName { get; set; }
        public string RoleName { get; set; }
        public string Description { get; set; }

        public EditRoleViewModel() { }
        public EditRoleViewModel(ApplicationRole role)
        {
            this.OriginalRoleName = role.Name;
            this.RoleName = role.Name;
            this.Description = role.Description;
        }
    }
    public class SelectRoleEditorViewModel
    {
        public SelectRoleEditorViewModel() { }

        // Update this to accept an argument of type ApplicationRole:
        public SelectRoleEditorViewModel(ApplicationRole role)
        {
            this.RoleName = role.Name;

            // Assign the new Descrption property:
            this.Description = role.Description;
        }

        public bool Selected { get; set; }

        [Required]
        public string RoleName { get; set; }

        // Add the new Description property:
        public string Description { get; set; }
    }

    public class SelectUserRolesViewModel
    {
        public SelectUserRolesViewModel()
        {
            this.Roles = new List<SelectRoleEditorViewModel>();
        }


        //Enable initialization with an instance of ApplicationUser:
        public SelectUserRolesViewModel(ApplicationUser user, IServiceProvider _serviceProvider) : this()
        {

           // RoleManager<ApplicationRole> _roleManager = ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();; 
            //new RoleManager<ApplicationRole>(new RoleStore<ApplicationRole>(new ApplicationDbContext())); //new RoleManager<ApplicationRole>(new RoleStore<ApplicationRole>(new ApplicationDbContext()));

            this.UserName = user.UserName;
            //this.FirstName = user.FirstName;
            //this.LastName = user.LastName;

            var Db = new ApplicationDbContext();

            // Add all available roles to the list of EditorViewModels:
            var allRoles = Db.Roles;
            foreach (var role in allRoles)
            {
                // An EditorViewModel will be used by Editor Template:
                var rvm = new SelectRoleEditorViewModel(role);
                this.Roles.Add(rvm);
            }

            // Set the Selected property to true for those roles for 
            // which the current user is a member:
            //foreach (var userRole in user.Roles)
            //{
            //    var r = _roleManager.Roles.Where(x => x.Id == userRole.RoleId).FirstOrDefault();
            //    var checkUserRole = this.Roles.Find(x => x.RoleName == r.Name);
            //    checkUserRole.Selected = true;

            //}
        }

        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public List<SelectRoleEditorViewModel> Roles { get; set; }
    }

    public class EditUserViewModel
    {
        public EditUserViewModel() { }

        // Allow Initialization with an instance of ApplicationUser:
        public EditUserViewModel(ApplicationUser user)
        {
            this.UserName = user.UserName;
            //this.FirstName = user.FirstName;
            //this.LastName = user.LastName;
            this.Email = user.Email;
            this.UserId = user.Id;
        }

        [Required]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }

        public string UserId { get; set; }

    }
}


