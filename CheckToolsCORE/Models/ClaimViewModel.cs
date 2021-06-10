using CheckToolsCORE.OmaData.DbModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckToolsCORE.Models
{
    public class ClaimViewModel
    {
        public List<AspNetUserClaims> Claims { get; set; }
        public AspNetUsers Utente { get; set; }
    }
}
