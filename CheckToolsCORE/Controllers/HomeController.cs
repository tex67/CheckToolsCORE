
using CheckToolsCORE.OmaData.Context;
using CheckToolsCORE.OmaData.DbModel;
using CheckToolsCORE.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;


namespace CheckToolsCORE.Controllers
{
    public class HomeController : Controller
    {
        //private DbContextMitico _context = new DbContextMitico();
        
        public ActionResult Index()
        {
            return View();

        }

        

       
    }
}