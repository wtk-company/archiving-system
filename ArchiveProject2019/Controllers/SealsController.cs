using ArchiveProject2019.HelperClasses;
using ArchiveProject2019.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ArchiveProject2019.Controllers
{
    public class SealsController : Controller
    {
        ApplicationDbContext _context;

        public SealsController()
        {
            _context = new ApplicationDbContext();
        }

        // GET: Seal


        [Authorize]
        [AccessDeniedAuthorizeattribute(ActionName = "DocumentSealsIndex")]
        public ActionResult Index(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("BadRequestError", "ErrorController");
            }

            var seals = _context.SealDocuments.Where(s => s.DocumentId == id).Include(s => s.Document).Include(s => s.CreatedBy).ToList();

            if (seals == null)
            {
                return RedirectToAction("HttpNotFoundError", "ErrorController");
            }

            return View(seals);
        }


        [Authorize]
        [AccessDeniedAuthorizeattribute(ActionName = "DocumentSealsDetails")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("BadRequestError", "ErrorController");
            }

            var seal = _context.SealDocuments.Find(id);

            if (seal == null)
            {
                return RedirectToAction("HttpNotFoundError", "ErrorController");
            }

            return View(seal);
        }


        [Authorize]
        [AccessDeniedAuthorizeattribute(ActionName = "DocumentSealsCreate")]
        public ActionResult Create(int id)
        {
            var seal = new SealDocument()
            {
                DocumentId = id,
            };

            return View(seal);
        }

        // POST: Seal/Create
        [HttpPost]

        [Authorize]
        [AccessDeniedAuthorizeattribute(ActionName = "DocumentSealsCreate")]
        public ActionResult Create(SealDocument Seal, HttpPostedFileBase SealFile)
        {
            if (SealFile == null)
            {
                ModelState.AddModelError("File", "يجب إدخال ملف");
            }

            if (!CheckFileFormatting.PermissionFile(SealFile))
            {
                ModelState.AddModelError("File", "صيغة الملف غير مدعومة!");
            }

            if (ModelState.IsValid)
            {
                Seal.CreatedAt = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss");
                Seal.CreatedById = User.Identity.GetUserId();
                Seal.FileName = Path.GetFileName(SealFile.FileName);
                
                Seal.File = new byte[SealFile.ContentLength];
                SealFile.InputStream.Read(Seal.File, 0, SealFile.ContentLength);
                
                _context.SealDocuments.Add(Seal);
                _context.SaveChanges();
                
                return RedirectToAction("Index", new { id = Seal.DocumentId });
            }
            
            return View(Seal);
            
        }




        [Authorize]
        [AccessDeniedAuthorizeattribute(ActionName = "DocumentSealsEdit")]

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("BadRequestError", "ErrorController");
            }

            var seal = _context.SealDocuments.Find(id);

            if (seal == null)
            {
                return RedirectToAction("HttpNotFoundError", "ErrorController");
            }

            return View(seal);
        }

        // POST: Seal/Edit/5
        [HttpPost]

        [Authorize]
        [AccessDeniedAuthorizeattribute(ActionName = "DocumentSealsEdit")]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }


        [Authorize]
        [AccessDeniedAuthorizeattribute(ActionName = "DocumentSealsDelete")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("BadRequestError", "ErrorController");
            }

            var seal = _context.SealDocuments.Find(id);

            if (seal == null)
            {
                return RedirectToAction("HttpNotFoundError", "ErrorController");
            }

            return View(seal);
        }

        // POST: Seal/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]

        [Authorize]
        [AccessDeniedAuthorizeattribute(ActionName = "DocumentSealsDelete")]
        public ActionResult Confirm(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("BadRequestError", "ErrorController");
            }

            var seal = _context.SealDocuments.Find(id);

            if (seal == null)
            {
                return RedirectToAction("HttpNotFoundError", "ErrorController");
            }

            _context.SealDocuments.Remove(seal);
            _context.SaveChanges();

            return RedirectToAction("Index", new { id = seal.DocumentId });
        }


        [Authorize]
        public FileResult DownloadDocument(int? id)
        {
            var seal = _context.SealDocuments.Find(id);
            return File(seal.File, System.Net.Mime.MediaTypeNames.Application.Octet, seal.FileName);
        }
        [Authorize]
        public FileResult DisplayDocument(int? id)
        {
            if (id != null)
            { 

                var seal = _context.SealDocuments.Find(id);

                // Images
                if (seal.FileName.EndsWith("jpeg") || seal.FileName.EndsWith("JPEG"))
                    return File(seal.File, "image/jpeg");

                if (seal.FileName.EndsWith("jpg") || seal.FileName.EndsWith("JPG"))
                    return File(seal.File, "image/jpg");

                if (seal.FileName.EndsWith("png") || seal.FileName.EndsWith("PNG"))
                    return File(seal.File, "image/png");

                if (seal.FileName.EndsWith("gif") || seal.FileName.EndsWith("GIF"))
                    return File(seal.File, "image/gif");

                // Pdf
                if (seal.FileName.EndsWith("pdf") || seal.FileName.EndsWith("PDF"))
                    return File(seal.File, "application/pdf");

            }
            return DownloadDocument(id);

        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
