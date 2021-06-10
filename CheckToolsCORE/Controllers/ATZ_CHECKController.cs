using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;

using System.IO;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CheckToolsCORE.OmaData.Context;
using Microsoft.EntityFrameworkCore;
using CheckToolsCORE.OmaData.DbModel;
using CheckToolsCORE.Models;
using CheckToolsCORE.Services;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Logging;

namespace CheckToolsCORE.Controllers
{
    [Authorize(Roles = "ATZ_RW, ATZ_ADMIN, ATZ_400")]
    public class ATZ_CHECKController : Controller
    {
        private DbContextMitico db; //= new DbContextMitico();

        public ATZ_CHECKController(DbContextMitico context, ILogger<HomeController> logger) 
        {
            db = context;
        }
        
        // GET: ATZ_CHECK
        public async Task<ActionResult> Index()
        {
            var appo = db.AtzCheckList.ToListAsync();
            return View(await appo);
        }


        public async Task<ActionResult> OpenFile()
        {
            return PartialView("_OpenFile");
        }

        public async Task<ActionResult> DownloadCheckList(string codArt)
        {
            MemoryStream ms = new MemoryStream();

            var chkl = await db.AtzCheckList.Where(x => x.Codart == codArt && x.Active==1).FirstOrDefaultAsync();

            if (chkl != null) 
            {
                
                                
                Response.Headers.Add("Content-Disposition", "inline;filename="+chkl.FileName);

                return File(chkl.Data, chkl.MimeType);
               
            }

            return   Json("File Non trovato!");

        }
        [HttpPost]
        public async Task<ActionResult> CheckListStor(string codArt)
        {

            try
            {
                var chkl = db.AtzCheckList.Where(x => x.Codart == codArt && x.Active == 0).OrderBy(x => x.DataDocumento);

                var dati = await chkl.Select(cl => new
                {
                    Id = cl.Id,
                    CodArt = cl.Codart,
                    Codice = cl.Codice,
                    DataDoc = cl.DataDocumento,
                    FileName = cl.FileName


                }).ToListAsync();
                if (dati.Count > 0)
                    return Json(dati);
                else
                {
                    return BadRequest("Nessuna check list storicizzata");
                }
            }
            catch (Exception ex)
            {
                var oex = ex.GetBaseException();

                return BadRequest(oex.Message);
                
            }
          

        }

        // GET: ATZ_CHECK/Details/5
        public async Task<ActionResult> Details(decimal id)
        {
            if (id == null)
            {
                return BadRequest("Chiave non valorizzata");
            }
            AtzCheckList aTZ_CHECK_LIST = await db.AtzCheckList.FindAsync(id);
            if (aTZ_CHECK_LIST == null)
            {
                return StatusCode(404);
            }
            return View(aTZ_CHECK_LIST);
        }

        // GET: ATZ_CHECK/Create
        public IActionResult Create(string id)
        {
            VwAtzControlli controllo = db.VwAtzControlli.Where(X => X.Codart == id).FirstOrDefault();

            if (controllo == null)
            {
                return StatusCode(404);
            }
            CheckListViewModel cvm = new CheckListViewModel()
            {
                CodiceArticolo = controllo.Codart,
                Pn = controllo.Pn,
                DataDocumento = DateTime.Now
               

            };
            return View(cvm);
        }

        // POST: ATZ_CHECK/Create
        // Per proteggere da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        // Per ulteriori dettagli, vedere https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
       // [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(string codart, string pn, string riferimento, string dataDocumento)
        {
            var check = CheckListService.IsValidCheckList(dataDocumento, riferimento);
            if (!check.Item1)
            {
                return Json(new { success = false, responseText = check.Item2 });
                
            }
            if (Request.Form.Files.Count > 0)
            {
                try
                {
                    var files = Request.Form.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        var file = files[i];
                        string fname;
                        if (Request.Headers["User-Agent"].ToString().ToUpper() == "IE" || Request.Headers["User-Agent"].ToString().ToUpper()  == "INTERNETEXPLORER")
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

                        AtzCheckList cl = new AtzCheckList()
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

                        var oldCheck = db.AtzCheckList.Where(x => x.Codart == codart).ToList();
                        //DISATTITO FLAG ACTIVE DELLE PRECEDENTI CHECK LIST

                        foreach (var item in oldCheck)
                        {
                            item.Active = 0;
                        }

                        db.AtzCheckList.Add(cl);
                        

                        db.SaveChanges();
                        //var uploadRootFolderInput = AppDomain.CurrentDomain.BaseDirectory + "\\Uploads";
                        //Directory.CreateDirectory(uploadRootFolderInput);
                        //var directoryFullPathInput = uploadRootFolderInput;
                        //fname = Path.Combine(directoryFullPathInput, fname);
                        //file.SaveAs(fname);
                    }
                 
                    return Json(new { success = true, responseText = "File caricato correttamente" });
                }
                catch (Exception ex)
                {
                    var oex = ex.GetBaseException();

                  
                    return Json(new { success = true, responseText = oex.Message });

                }
            }
            else
            {
                return Json("No files selected.");
            }
            //int PIPPO = Request.Files.Count;
            //    if (ModelState.IsValid)
            //{
            //    db.ATZ_CHECK_LIST.Add(aTZ_CHECK_LIST);
            //    await db.SaveChangesAsync();
            //    return RedirectToAction("Index");
            //}

            //return View(aTZ_CHECK_LIST);
        }

        // GET: ATZ_CHECK/Edit/5
        public async Task<ActionResult> Edit(decimal id)
        {
            if (id == null)
            {
                return BadRequest("Id non valorizzato");
            }
            AtzCheckList aTZ_CHECK_LIST = await db.AtzCheckList.FindAsync(id);
            if (aTZ_CHECK_LIST == null)
            {
                return NotFound();
            }
            return View(aTZ_CHECK_LIST);
        }

        // POST: ATZ_CHECK/Edit/5
        // Per proteggere da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        // Per ulteriori dettagli, vedere https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(AtzCheckList aTZ_CHECK_LIST)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aTZ_CHECK_LIST).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(aTZ_CHECK_LIST);
        }

        // GET: ATZ_CHECK/Delete/5
        public async Task<ActionResult> Delete(decimal id)
        {
            if (id == null)
            {
                return BadRequest("Id non valorizzato");
            }
            AtzCheckList aTZ_CHECK_LIST = await db.AtzCheckList.FindAsync(id);
            if (aTZ_CHECK_LIST == null)
            {
                return NotFound();
            }
            return View(aTZ_CHECK_LIST);
        }

        // POST: ATZ_CHECK/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(decimal id)
        {
            AtzCheckList aTZ_CHECK_LIST = await db.AtzCheckList.FindAsync(id);
            db.AtzCheckList.Remove(aTZ_CHECK_LIST);
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
                        var file = files[i];
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

                        AtzCheckList cl = new AtzCheckList()
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
