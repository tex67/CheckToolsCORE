
using CheckToolsCORE.OmaData.Context;
using CheckToolsCORE.OmaData.DbModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace CheckToolsCORE.Controllers
{
    public class CalendarioController : Controller
    {
        private DbContextMitico db;// = new DbContextMitico();

        public CalendarioController(DbContextMitico context, ILogger<HomeController> logger) 
        {
            db = context;
        }
        public ActionResult Index()
        {
            return View();
        }
        
        public ActionResult CalScadenze(string start, string end)
        {
            DateTime data1 = DateTime.Parse(start);
            DateTime data2 = DateTime.Parse(end);


            IQueryable<VwAtzControlli> query = db.VwAtzControlli.Where(x => x.Scadenza >= data1 && x.Scadenza <= data2);

            // Paging


            var data = query.Select(cont => new
            {
                title = cont.Codart,
                
                description = cont.Desart,
               
                start = cont.Scadenza,
                               
                end = cont.Scadenza
            }).Take(10).ToList();


            return Json(data);

        }
    }
}