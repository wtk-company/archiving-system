using ArchiveProject2019.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ArchiveProject2019.Controllers
{
    public class DocumentKindsController : Controller
    {
        
        private ApplicationDbContext _context;

        public DocumentKindsController()
        {
            _context = new ApplicationDbContext();
        }

        // GET: DocumentKinds
        public ActionResult Index(string Id = "none")
        {
            if (!Id.Equals("none"))
            {
                ViewBag.Msg = Id;

            }
            else
            {
                ViewBag.Msg = null;
            }

            ViewBag.Current = "DocumentKind";
            var DocKinds = _context.DocumentKinds.Include(a=>a.CreatedBy).Include(a=>a.UpdatedBy).ToList();
            return View(DocKinds.ToList());
        }

        

        public ActionResult Create()
        {

            ViewBag.Current = "DocumentKinds";

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,CreatedAt")] DocumentKind DocumentKind)
        {
            ViewBag.Current = "DocumentKinds";

            if (_context.DocumentKinds.Any(a => a.Name.Equals(DocumentKind.Name, StringComparison.OrdinalIgnoreCase)))
            {
                return RedirectToAction("Index", new { Id = "CreateError" });

            }
            if (ModelState.IsValid)
            {

                DocumentKind.CreatedAt = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss");
                DocumentKind.CreatedById = User.Identity.GetUserId();

                _context.DocumentKinds.Add(DocumentKind);
                _context.SaveChanges();
                return RedirectToAction("Index", new { Id = "CreateSuccess" });

            }

            return View(DocumentKind);
        }

        // GET: DocumentKinds/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.Current = "DocumentKind";

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            DocumentKind kinds = _context.DocumentKinds.Find(id);
            if (kinds == null)
            {
                return HttpNotFound();
            }

            return View(kinds);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DocumentKind kinds)
        {
            if (_context.ConcernedParties.Where(a => a.Id != kinds.Id).Any(a => a.Name.Equals(kinds.Name, StringComparison.OrdinalIgnoreCase)))
                return RedirectToAction("Index", new { Id = "EditError" });

            if (ModelState.IsValid)
            {

                kinds.UpdatedAt= DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss");
                kinds.UpdatedById = User.Identity.GetUserId();
                _context.Entry(kinds).State = EntityState.Modified;
                _context.SaveChanges();

                return RedirectToAction("Index", new { Id = "EditSuccess" });
            }

            return View(kinds);
        }

        // GET: ConcernedPartys/Delete/5
        public ActionResult Delete(int? id)
        {
            ViewBag.Current = "DocumentKind";


            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            DocumentKind kind = _context.DocumentKinds.Find(id);
            if (kind == null)
            {
                return HttpNotFound();
            }

            return View(kind);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Confirm(int? id)
        {
            ViewBag.Current = "DocumentKind";

            DocumentKind kind = _context.DocumentKinds.Find(id);

            _context.DocumentKinds.Remove(kind);
            _context.SaveChanges();

            return RedirectToAction("Index", new { Id = "DeleteSuccess" });
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