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
    public class ConcernedPartiesController : Controller
    {
        // GET: ConcernedParties
        private ApplicationDbContext _context;

        public ConcernedPartiesController()
        {
            _context = new ApplicationDbContext();
        }

        [Authorize]
        [AccessDeniedAuthorizeattribute(ActionName = "ConcernedPartiesIndex")]
        // GET: ConcernedPartys
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

            ViewBag.Current = "ConcernedParty";
            var parties = _context.ConcernedParties.Include(a=>a.CreatedBy).ToList();
            return View(parties.ToList());
        }


        [Authorize]
        [AccessDeniedAuthorizeattribute(ActionName = "ConcernedPartiesCreate")]
        public ActionResult Create()
        {

            ViewBag.Current = "ConcernedPartys";

            return View();
        }

        [Authorize]
        [AccessDeniedAuthorizeattribute(ActionName = "ConcernedPartiesCreate")]
        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Create([Bind(Include = "Id,Name,CreatedAt")] ConcernedParty ConcernedParty)
        {
            ViewBag.Current = "ConcernedPartys";

            if (_context.ConcernedParties.Any(a => a.Name.Equals(ConcernedParty.Name, StringComparison.OrdinalIgnoreCase)))
            {
                return RedirectToAction("Index", new { Id = "CreateError" });

            }
            if (ModelState.IsValid)
            {

                ConcernedParty.CreatedAt = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss");
                ConcernedParty.CreatedById = User.Identity.GetUserId();

                _context.ConcernedParties.Add(ConcernedParty);
                _context.SaveChanges();
                return RedirectToAction("Index", new { Id = "CreateSuccess" });

            }
            return RedirectToAction("Index");

        }


        [Authorize]
        [AccessDeniedAuthorizeattribute(ActionName = "ConcernedPartiesEdit")]
        public ActionResult Edit(int? id)
        {
            ViewBag.Current = "ConcernedParty";

            if (id == null)
            {
                return RedirectToAction("BadRequestError", "ErrorController");
            }

            ConcernedParty party = _context.ConcernedParties.Find(id);
            if (party == null)
            {
            return RedirectToAction("HttpNotFoundError", "ErrorController");

            }

            return View(party);
        }

        [Authorize]
        [AccessDeniedAuthorizeattribute(ActionName = "ConcernedPartiesEdit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ConcernedParty party)
        {
            if (_context.ConcernedParties.Where(a => a.Id != party.Id).Any(a => a.Name.Equals(party.Name, StringComparison.OrdinalIgnoreCase)))
                return RedirectToAction("Index", new { Id = "EditError" });

            if (ModelState.IsValid)
            {
                party .UpdatedAt= DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss");
                _context.Entry(party).State = EntityState.Modified;

                _context.SaveChanges();

                return RedirectToAction("Index", new { Id = "EditSuccess" });
            }

            return RedirectToAction("Index");
        }


        [Authorize]
        [AccessDeniedAuthorizeattribute(ActionName = "ConcernedPartiesDelete")]
        // GET: ConcernedPartys/Delete/5
        public ActionResult Delete(int? id)
        {
            ViewBag.Current = "ConcernedParty";


            if (id == null)
            {
                return RedirectToAction("BadRequestError", "ErrorController");

            }

            ConcernedParty party = _context.ConcernedParties.Find(id);
            if (party == null)
            {
                return RedirectToAction("HttpNotFoundError", "ErrorController");

            }

            return View(party);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        [AccessDeniedAuthorizeattribute(ActionName = "ConcernedPartiesDelete")]
        public ActionResult Confirm(int? id)
        {
            ViewBag.Current = "ConcernedParty";

            ConcernedParty party = _context.ConcernedParties.Find(id);

            _context.ConcernedParties.Remove(party);
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