﻿using ArchiveProject2019.HelperClasses;
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
    public class KindsController : Controller
    {
        
        private ApplicationDbContext _context;

        public KindsController()
        {
            _context = new ApplicationDbContext();
        }



        [Authorize]
        [AccessDeniedAuthorizeattribute(ActionName = "DocumentKindsIndex")]
 
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

            ViewBag.Current = "Kind";
            var DocKinds = _context.Kinds.Include(a=>a.CreatedBy).ToList();
            return View(DocKinds.ToList());
        }

        [Authorize]
        [AccessDeniedAuthorizeattribute(ActionName = "DocumentKindsCreate")]

        public ActionResult Create()
        {

            ViewBag.Current = "Kinds";

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        [Authorize]
        [AccessDeniedAuthorizeattribute(ActionName = "DocumentKindsCreate")]
        public ActionResult Create([Bind(Include = "Id,Name,CreatedAt")] DocumentKind DocumentKind)

        {
            ViewBag.Current = "Kinds";

            if (_context.Kinds.Any(a => a.Name.Equals(Kind.Name, StringComparison.OrdinalIgnoreCase)))
            {
                return RedirectToAction("Index", new { Id = "CreateError" });

            }
            if (ModelState.IsValid)
            {

                Kind.CreatedAt = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss");
                Kind.CreatedById = User.Identity.GetUserId();

                _context.Kinds.Add(Kind);
                _context.SaveChanges();
                return RedirectToAction("Index", new { Id = "CreateSuccess" });

            }

            return RedirectToAction("Index");

        }


       

        [Authorize]
        [AccessDeniedAuthorizeattribute(ActionName = "DocumentKindsEdit")]

        public ActionResult Edit(int? id)
        {
            ViewBag.Current = "Kind";

            if (id == null)
            {
                return RedirectToAction("BadRequestError", "ErrorController");

            }

            Kind kinds = _context.Kinds.Find(id);
            if (kinds == null)
            {
                return RedirectToAction("HttpNotFoundError", "ErrorController");

            }

            return View(kinds);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]


        [Authorize]
        [AccessDeniedAuthorizeattribute(ActionName = "DocumentKindsEdit")]
        public ActionResult Edit(DocumentKind kinds)

        {
            if (_context.Parties.Where(a => a.Id != kinds.Id).Any(a => a.Name.Equals(kinds.Name, StringComparison.OrdinalIgnoreCase)))
                return RedirectToAction("Index", new { Id = "EditError" });

            if (ModelState.IsValid)
            {

                kinds.UpdatedAt= DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss");
          
                _context.Entry(kinds).State = EntityState.Modified;
                _context.SaveChanges();

                return RedirectToAction("Index", new { Id = "EditSuccess" });
            }

            return RedirectToAction("Index");

        }



        [Authorize]
        [AccessDeniedAuthorizeattribute(ActionName = "DocumentKindsDelete")]

        public ActionResult Delete(int? id)
        {
            ViewBag.Current = "Kind";


            if (id == null)
            {
                return RedirectToAction("BadRequestError", "ErrorController");

            }

            Kind kind = _context.Kinds.Find(id);
            if (kind == null)
            {
                return RedirectToAction("HttpNotFoundError", "ErrorController");


            }


            if(CheckDelete.CheckDocumentKindsDelete(kind.Id)==false)
            {
                return RedirectToAction("HttpNotFoundError", "DeleteError");
            }
            return View(kind);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]

        [Authorize]
        [AccessDeniedAuthorizeattribute(ActionName = "DocumentKindsDelete")]
        public ActionResult Confirm(int? id)
        {
            ViewBag.Current = "Kind";

            Kind kind = _context.Kinds.Find(id);

            _context.Kinds.Remove(kind);
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