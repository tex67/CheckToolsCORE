using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CheckToolsCORE.OmaData.Context;
using CheckToolsCORE.OmaData.DbModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.AspNetCore.Http;
using CheckToolsCORE.Services;
using CheckToolsCORE.Models;
using JqueryDataTables.ServerSide.AspNetCoreWeb.Models;
using Newtonsoft.Json;
using System.Linq.Dynamic.Core;
using Microsoft.Extensions.Logging;

namespace CheckToolsCORE.Controllers
{
    [Authorize(Roles ="ATZ_RW, ATZ_ADMIN, ATZ_400")]
    public class ATZ_CONTROLLIController : Controller
    {
        DbContextMitico db;
        public ATZ_CONTROLLIController(DbContextMitico context, ILogger<HomeController> logger) 
        {
            db = context;
        }
       
        // GET: ATZ_CONTROLLI
        public async Task<ActionResult> Index()
        {
            
            return View();
        }

        public async Task<ActionResult> DownloadEsito(int id)
        {

            MemoryStream ms = new MemoryStream();

            var chkl = await db.AtzControlli.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (chkl != null && !string.IsNullOrEmpty(chkl.MimeType))
            {

                Response.Headers.Add("Content-Disposition", "inline;filename=" + chkl.FileName);

                return File(chkl.Dati, chkl.MimeType);

            }

            return Json("File Non trovato!");

        }

        public ActionResult GetDataTableData(JqueryDataTablesParameters requestModel, AdvancedSearchControlliViewModel searchViewModel)
        {
            string[] gruppi = { "ATZ_400", "ATZ_500", "ATZ_600", "ATZ_700" };
           
            IQueryable<VwAtzControlli> query = db.VwAtzControlli;
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claimss = identity.Claims;
            var abiGr2 = claimss.FirstOrDefault(c => c.Type == "CODGR2");
            var ruolo = claimss.FirstOrDefault(c => c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role");

            //se l'utente non è amministratore filtro a monte i dati
            if (ruolo != null &&  ruolo.Value !="ATZ_ADMIN") 
            {
                query = query.Where(x => x.Codgr2 == abiGr2.Value);
                
            }
            
            var totalCount = query.Count();
            
            // searching and sorting

            query = SearchMagana(requestModel, searchViewModel, query);

            var filteredCount = query.Count();

            // Paging
            query = query.Skip(requestModel.Start).Take(requestModel.Length);

            
            var data = query.Select(cont => new
            {
                CodArt = cont.Codart,
                Pn = cont.Pn,
                Descrizione = cont.Desart,
                Id = cont.Id,
                DataControllo = cont.DataControllo,
                Scadenza = cont.Scadenza,
                Periodicita = cont.Periodicita,
                CurrentUser = User.Identity.Name,
                FileName = cont.FileName 
                
            }).ToList();


            return Json(new {extra="Filtro Gruppo merce: "+ (abiGr2 !=null?abiGr2.Value :""), draw = requestModel.Draw, recordsFiltered = filteredCount, recordsTotal = totalCount, data = data });

          

        }
        
        /// <summary>
        /// Array object per popolare jquery datatable in modalita' client
        /// </summary>
        /// <returns></returns>

        public ActionResult GetDataTableDataF(string user)
        {

            IQueryable<VwAtzControlli> query = db.VwAtzControlli.Where(X => X.Codart.StartsWith("2"));

            // Paging


            var dati = query.Select(cont => new
            {
                CodArt = cont.Codart.Trim(),
                Pn = cont.Pn,
                Descrizione = cont.Desart,
               // RevLev = cont.ULTREV,
                UltimoControllo = cont.DataControllo,
                Scadenza = cont.Scadenza,
                Periodicita = cont.Periodicita,
                CurrentUser = User.Identity.Name


            }).ToList();
            
            List<ControlliGridViewModel> copy = new List<ControlliGridViewModel>();

            foreach (var item in dati)
            {
                ControlliGridViewModel com = new ControlliGridViewModel()
                {
                    CodArt = item.CodArt,
                    CurrentUser = item.CurrentUser,
                    Descrizione = item.Descrizione,
                    Periodicita = (int)item.Periodicita,
                    Pn = item.Pn,
                    //RevLev = item.RevLev,
                    Scadenza = item.Scadenza,
                    UltimoControllo = item.UltimoControllo

                };
                copy.Add(com);
            }


            var collectionWrapper = new
            {

                data = copy

            };

            return Content(JsonConvert.SerializeObject(collectionWrapper), "application/json");

        }

        private IQueryable<VwAtzControlli> SearchMagana(JqueryDataTablesParameters requestModel, AdvancedSearchControlliViewModel searchViewModel, IQueryable<VwAtzControlli> query)
        {
            
            // Filtro per grupppo merce codgr2
            //verificare se admin o responsabile
            if (searchViewModel.CodGr2 != null && searchViewModel.CodGr2.Count > 0)
            {
                              
                query = query.Where(x =>  searchViewModel.CodGr2.Contains(x.Codgr2));
               
            }
            else if (!string.IsNullOrEmpty(searchViewModel.GiorniScadenza))
            {
                //l'utente ha digitato un nmero nel campo di testo giorni scad
                DateTime datatarget =  DateTime.Parse(searchViewModel.GiorniScadenza);

                query = query.Where(x => x.Scadenza <= datatarget && x.Scadenza >= DateTime.Now);
            }
            else if (requestModel.Search != null && ! string.IsNullOrEmpty(requestModel.Search.Value))
            {
                var value = requestModel.Search.Value.Trim();
                query = query.Where(p => p.Codart.Contains(value));
            }

            ///***** Advanced Search ******/
            //if (searchViewModel.FacilitySite != Guid.Empty)
            //    query = query.Where(x => x.FacilitySiteID == searchViewModel.FacilitySite);

            //if (searchViewModel.Building != null)
            //    query = query.Where(x => x.Building == searchViewModel.Building);

            //if (searchViewModel.Manufacturer != null)
            //    query = query.Where(x => x.Manufacturer == searchViewModel.Manufacturer);

            //if (searchViewModel.Status != null)
            //{
            //    bool Issued = bool.Parse(searchViewModel.Status);
            //    query = query.Where(x => x.Issued == Issued);
            //}

            /***** Advanced Search ******/

            var filteredCount = query.Count();

            // Sort
            var sortedColumns = "";//requestModel.Columns.GetSortedColumns();
            var orderByString = String.Empty;

           

            foreach (var item in requestModel.Order)
            {
                sortedColumns += requestModel.Columns[item.Column].Data + " " + item.Dir + ",";
            }

            if (sortedColumns.Substring(sortedColumns.Length - 1) == ",")
                sortedColumns = sortedColumns.Substring(0, sortedColumns.Length - 1);

            query = query.OrderBy(orderByString == string.Empty ? "CODART asc" : orderByString);


            return query;

        }
        // GET: ATZ_CONTROLLI/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            AtzControlli aTZ_CONTROLLI = await db.AtzControlli.FindAsync(id);
            if (aTZ_CONTROLLI == null)
            {
                return NotFound();
            }
            return View(aTZ_CONTROLLI);
        }

        // GET: ATZ_CONTROLLI/Create
        public ActionResult Create(string id)
        {
            if (id == null)
            {
                return  BadRequest();
            }

            var atr = db.VwAtzControlli.Where(x => x.Codart == id).FirstOrDefault();

            
            if (atr == null) 
            {
                Response.StatusCode = 400;
                return Json(new { success = false, responseText = "Attrezzo non presente" });
            }

            //VERIFICA CHECK LIST DISPONIBILI
            var cl = db.AtzCheckList.Where(x => x.Codart == id && x.Active == 1).FirstOrDefault();
            if (cl == null)
            {
                Response.StatusCode = 400;
                return Json(new { success = false, responseText = "CheckList non presente" });
            }
            
            ControlliViewModel cvm = new ControlliViewModel() 
            {
                 CodiceArticolo = id,
                 Pn = atr.Pn ,
                 IdCheckList = (int)cl.Id,
                 DataChecklist = cl.DataDocumento.ToShortDateString(),
                 RifChecklist = cl.Codice,
                 DataControllo = DateTime.Now
            };
            

            return View(cvm);
        }

        // POST: ATZ_CONTROLLI/Create
        // Per proteggere da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        // Per ulteriori dettagli, vedere https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
       // [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(string codart, string pn, string riferimento, string dataControllo, int checkList)
        {
            if (ModelState.IsValid)
            {
                if (Request.Form.Files.Count == 0)
                {
                    Response.StatusCode = 400;
                    return Json(new { success = false, responseText = "Allegato non presente" });
                }
                try
                {
                    var files = Request.Form.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        IFormFile file = files[i];
                        string fname;
                        if (Request.Headers["User-Agent"].ToString().ToUpper() == "IE" || Request.Headers["User-Agent"].ToString().ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] testfiles = file.FileName.Split(new char[] { '\\' });
                            fname = testfiles[testfiles.Length - 1];
                        }
                        else
                        {
                            fname = file.FileName;
                        }
                        MemoryStream target = new MemoryStream();
                        file.CopyTo(target);
                        byte[] data = target.ToArray();
                        string contentType = "";
                        new FileExtensionContentTypeProvider().TryGetContentType(file.FileName, out contentType); 
                       
                        AtzControlli cont = new AtzControlli()
                        {
                             Codart = codart,
                             DataControllo = DateTime.Parse(dataControllo),
                             IdCheckList = checkList,
                             LastEdit = DateTime.Now,
                             LastUser = User.Identity!= null ? User.Identity.Name: "",
                             Dati = data,
                             MimeType = contentType,
                             FileName = file.FileName

                        };
                          
                        db.AtzControlli.Add(cont);

                        await db.SaveChangesAsync();

                       
                        //var uploadRootFolderInput = AppDomain.CurrentDomain.BaseDirectory + "\\Uploads";
                        //Directory.CreateDirectory(uploadRootFolderInput);
                        //var directoryFullPathInput = uploadRootFolderInput;
                        //fname = Path.Combine(directoryFullPathInput, fname);
                        //file.SaveAs(fname);
                    }
                    return Json("File caricato correttamente!");
                }
                catch (Exception ex)
                {
                    var excOri= ex.GetBaseException();
                    return Json("Error occurred. Error details: " + excOri.Message);
                }
                //db.ATZ_CONTROLLI.Add(aTZ_CONTROLLI);
                //await db.SaveChangesAsync();
                //return RedirectToAction("Index");
            }

            return View();
        }

        // GET: ATZ_CONTROLLI/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return BadRequest("Id non valorizzato");
            }
            VwAtzControlli aTZ_CONTROLLI = await db.VwAtzControlli.Where(X => X.Codart == id).FirstOrDefaultAsync();
            if (aTZ_CONTROLLI == null)
            {
                return NotFound();
            }
            return View(aTZ_CONTROLLI);
        }
        // GET: ATZ_CONTROLLI/Edit/5
        public ActionResult Upload(string id)
        {
            if (id == null)
            {
                return BadRequest("Id non valorizzato");
            }

            return RedirectToAction("Create", "ATZ_CHECK", new { id = id });
        }

        // POST: ATZ_CONTROLLI/Edit/5
        // Per proteggere da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        // Per ulteriori dettagli, vedere https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit( AtzControlli aTZ_CONTROLLI)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aTZ_CONTROLLI).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(aTZ_CONTROLLI);
        }

        // GET: ATZ_CONTROLLI/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return BadRequest("Chiave non trovata");
            }
            AtzControlli aTZ_CONTROLLI = await db.AtzControlli.FindAsync(id);
            if (aTZ_CONTROLLI == null)
            {
                return NotFound();
            }
            return View(aTZ_CONTROLLI);
        }

        public async Task<ActionResult> DeleteAll(string id)
        {
            if (id == null)
            {
                return BadRequest("Chiave non trovata");
            }
            AtzControlli aTZ_CONTROLLI = await db.AtzControlli.FindAsync(id);
            
            if (aTZ_CONTROLLI == null)
            {
                return NotFound();
            }
            return View(aTZ_CONTROLLI);
        }


        // POST: ATZ_CONTROLLI/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            AtzControlli aTZ_CONTROLLI = await db.AtzControlli.FindAsync(id);
            db.AtzControlli.Remove(aTZ_CONTROLLI);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Uploads(string codart, string pn, string riferimento, string dataDocumento)
        {
            var check = CheckListService.IsValidCheckList(dataDocumento, riferimento);
            if (!check.Item1)
            {
                return Json(check.Item2);
            }
            if (Request.Form.Files.Count > 0)
            {
                try
                {
                    var files = Request.Form.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        IFormFile file = files[i];
                        string fname;
                        if ( Request.Headers["User-Agent"].ToString().ToUpper() == "IE" || Request.Headers["User-Agent"].ToString().ToUpper()  == "INTERNETEXPLORER")
                        {
                            string[] testfiles = file.FileName.Split(new char[] { '\\' });
                            fname = testfiles[testfiles.Length - 1];
                        }
                        else
                        {
                            fname = file.FileName;
                        }
                        MemoryStream target = new MemoryStream();
                        file.CopyTo(target);
                        byte[] data = target.ToArray();
                        string contentType = "";
                        new FileExtensionContentTypeProvider().TryGetContentType(file.FileName, out contentType);

                        AtzCheckList cl = new  AtzCheckList()
                        {
                            Codart = codart,
                            Codice = riferimento,
                            Active = 1,
                            DataDocumento = DateTime.Parse(dataDocumento),
                            DataCaricamento = DateTime.Now,
                            Data = data,
                            MimeType = contentType,
                            FileName = file.FileName


                        };
                        db.AtzCheckList.Add(cl);
                        db.SaveChanges();
                        //var uploadRootFolderInput = AppDomain.CurrentDomain.BaseDirectory + "\\Uploads";
                        //Directory.CreateDirectory(uploadRootFolderInput);
                        //var directoryFullPathInput = uploadRootFolderInput;
                        //fname = Path.Combine(directoryFullPathInput, fname);
                        //file.SaveAs(fname);
                    }
                    return Json("File caricato correttamente!");
                }
                catch (Exception ex)
                {
                    return Json("Error occurred. Error details: " + ex.Message);
                }
            }
            else
            {
                return Json("No files selected.");
            }
        }

        [HttpPost]
        public async Task<ActionResult> ControlliStor([FromBody] string search)
        {
            dynamic myObject = JsonConvert.DeserializeObject<dynamic>(search);
            string codArtSearch = myObject.codArt;
            try
            {
                var chkl = db.AtzControlli.Where(x => x.Codart == codArtSearch && x.FileName !=null).OrderByDescending(x => x.DataControllo);

                var dati = await chkl.Select(cl => new
                {
                    Id = cl.Id,
                    CodArt = cl.Codart,
                    DataDoc = cl.DataControllo,
                    FileName = cl.FileName


                }).ToListAsync();
                if (dati.Count() > 0)
                    return Json(dati);
                else
                {
                    return BadRequest("Nessuna controllo disponibile");
                }
            }
            catch (Exception ex)
            {
                var oex = ex.GetBaseException();

                return  BadRequest(oex.Message);

            }


        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
