using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace CheckToolsCORE.Models.AccountViewModels 
{ 
    
        public class RegisterViewModel
        {
            [Required]
            [Display(Name = "User Name", Prompt = "Inserire Utente Active Directory")]
            public string UserName { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Posta elettronica")]
            public string Email { get; set; }


            [Required]
            [StringLength(100, ErrorMessage = "La lunghezza di {0} deve essere di almeno {2} caratteri.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Conferma password")]
            [Compare("Password", ErrorMessage = "La password e la password di conferma non corrispondono.")]
            public string ConfirmPassword { get; set; }
        }

    }

