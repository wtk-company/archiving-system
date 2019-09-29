using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ArchiveProject2019.Models;
using Microsoft.AspNet.Identity;
using ArchiveProject2019.HelperClasses;

namespace ArchiveProject2019.Controllers
{
    public class FormsController : Controller
    {
        private ApplicationDbContext _context;

        public FormsController ()
        {
            _context = new ApplicationDbContext();
        }

        // GET: Forms
        public ActionResult Index(string Id="none")
        {
            if (!Id.Equals("none"))
            {
                ViewBag.Msg = Id;

            }
            else
            {
                ViewBag.Msg = null;
            }
            string UID = this.User.Identity.GetUserId();
            ViewBag.Current = "Forms";
            var forms = _context.Forms.Include(a => a.CreatedBy).Include(a => a.Documents).Include(a => a.CreatedBy);
            return View(forms.ToList());
        }

      

        public ActionResult Create()
        {

            ViewBag.Current = "Forms";

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,CreatedAt")] Form form)
        {///
            ViewBag.Current = "Forms";

            if (_context.Forms.Any(a => a.Name.Equals(form.Name, StringComparison.OrdinalIgnoreCase)))
            {
                return RedirectToAction("Index", new { Id = "CreateError" });

            }
            if (ModelState.IsValid)
            {

                form.CreatedAt = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss");
                form.CreatedById = User.Identity.GetUserId();

                _context.Forms.Add(form);
                _context.SaveChanges();
                return RedirectToAction("Index", new { Id = "CreateSuccess" });

            }


            return RedirectToAction("Index");
        }

        // GET: Forms/Edit/5
        public ActionResult Edit(int? id)
        {

            ViewBag.Current = "Forms";



            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Form form = _context.Forms.Find(id);
            if (form == null)
            {
                return HttpNotFound();
            }



            return View(form);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,CreatedAt,CreatedById")] Form form)
        {
            ViewBag.Current = "Forms";


            if (_context.Forms.Where(a=>a.Id!=form.Id).Any(a=>a.Name.Equals(form.Name,StringComparison.OrdinalIgnoreCase)))
            {
                return RedirectToAction("Index", new { Id = "EditError" });

            }

            if (ModelState.IsValid)
            {
                form.UpdatedAt = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss");

                _context.Entry(form).State = EntityState.Modified;
                _context.SaveChanges();
                return RedirectToAction("Index", new { Id = "EditSuccess" });
            }

            return RedirectToAction("Index");

        }

        // GET: Forms/Delete/5
        public ActionResult Delete(int? id)
        {

            ViewBag.Current = "Forms";


            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Form form = _context.Forms.Include(a=>a.Fields).Include(a=>a.CreatedBy).SingleOrDefault(a=>a.Id==id);
            if (form == null)
            {
                return HttpNotFound();
            }
            
            IEnumerable<Document> Documents = _context.Documents.Where(a => a.FormId == id);
            if(CheckDelete.checkFormDelete(id.Value)==false)
            {
                return RedirectToAction("Index", new { Id = "DeleteError" });

            }

            return View(form);
           
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Confirm(int? id)
        {

            ViewBag.Current = "Forms";

            Form cat = _context.Forms.Find(id);

            // Delete Fields
           List<Field> Fields = _context.Fields.Where(a=>a.FormId==id).ToList();
            foreach(Field f in Fields)
            {
                _context.Fields.Remove(f);
            }

            _context.Forms.Remove(cat);
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
