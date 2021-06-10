using CheckToolsCORE.OmaData.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckToolsCORE.Services
{
    public class UserService
    {
        public static  bool IsSessioneAttiva(DbContextMitico _context, string userId, decimal session)
        {
          
            var apxSes = _context.ApexWsSessions.Where(x => x.ApexSessionId == session && x.UserName == userId);
            if (apxSes == null)
                return false;
            else
                return true;
        }
    }
}
