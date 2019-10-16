using ArchiveProject2019.HelperClasses;
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
    public class TypeMailsController : Controller
    {

        private ApplicationDbContext _context;

        public TypeMailsController()
        {
            _context = new ApplicationDbContext();
        }

        [Authorize]
        [AccessDeniedAuthorizeattribute(ActionName = "TypeMailsIndex")]
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

            ViewBag.Current = "TypeMails";
            var TypeMails = _context.TypeMails.Include(a => a.CreatedBy).ToList();
            return View(TypeMails.ToList());
        }

        [Authorize]
        [AccessDeniedAuthorizeattribute(ActionName = "TypeMailsCreate")]
        public ActionResult Create()
        {

            ViewBag.Current = "TypeMails";

            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [AccessDeniedAuthorizeattribute(ActionName = "TypeMailsCreate")]
        public ActionResult Create([Bind(Include = "Id,Name,CreatedAt")] TypMail TypeMail)
        {
            ViewBag.Current = "TypeMails";

            if (_context.TypeMails.Any(a => a.Name.Equals(TypeMail.Name, StringComparison.OrdinalIgnoreCase)))
            {
                return RedirectToAction("Index", new { Id = "CreateError" });

            }
            if (ModelState.IsValid)
            {

                TypeMail.CreatedAt = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss");
                TypeMail.CreatedById = User.Identity.GetUserId();

                _context.TypeMails.Add(TypeMail);
                _context.SaveChanges();
                return RedirectToAction("Index", new { Id = "CreateSuccess" });

            }

            return RedirectToAction("Index");

        }


        [Authorize]
        [AccessDeniedAuthorizeattribute(ActionName = "TypeMailsEdit")]
        public ActionResult Edit(int? id)
        {
            ViewBag.Current = "TypeMails";

            if (id == null)
            {
                return RedirectToAction("BadRequestError", "ErrorController");
            }

            TypMail mail = _context.TypeMails.Find(id);
            if (mail == null)
            {
                return RedirectToAction("HttpNotFoundError", "ErrorController");
            }

            return View(mail);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        [Authorize]
        [AccessDeniedAuthorizeattribute(ActionName = "TypeMailsEdit")]
        public ActionResult Edit(TypMail mail)
        {

            ViewBag.Current = "TypeMails";

            if (_context.ConcernedParties.Where(a => a.Id != mail.Id).Any(a => a.Name.Equals(mail.Name, StringComparison.OrdinalIgnoreCase)))
                return RedirectToAction("Index", new { Id = "EditError" });

            if (ModelState.IsValid)
            {

                mail.UpdatedAt = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss");

                _context.Entry(mail).State = EntityState.Modified;
                _context.SaveChanges();

                return RedirectToAction("Index", new { Id = "EditSuccess" });
            }

            return RedirectToAction("Index");

        }


        [Authorize]
        [AccessDeniedAuthorizeattribute(ActionName = "TypeMailsDelete")]
        public ActionResult Delete(int? id)
        {
            ViewBag.Current = "TypeMails";


            if (id == null)
            {
                return RedirectToAction("BadRequestError", "ErrorController");
            }

            TypMail mail = _context.TypeMails.Find(id);
            if (mail == null)
            {
                return RedirectToAction("HttpNotFoundError", "ErrorController");
            }

            return View(mail);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]

        [Authorize]
        [AccessDeniedAuthorizeattribute(ActionName = "TypeMailsDelete")]
        public ActionResult Confirm(int? id)
        {
            ViewBag.Current = "TypeMails";


            TypMail mail = _context.TypeMails.Find(id);

            _context.TypeMails.Remove(mail);
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