
using CheckToolsCORE.OmaData.DbModel;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Text;


namespace CheckToolsCORE.OmaData.Context
{ 

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        // Add an instance IDbSet using the 'new' keyword:
        new public virtual IDbSet<ApplicationRole> Roles { get; set; }
     

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

      
        public ApplicationDbContext() 
        {
           
        }
    }
}
