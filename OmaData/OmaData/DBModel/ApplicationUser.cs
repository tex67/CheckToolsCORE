using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace CheckToolsCORE.OmaData.DbModel
{
    public class ApplicationUser : IdentityUser
    {
        public string ApplicationId { get; set; }
    }
}
