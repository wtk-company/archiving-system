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
    public class PartiesController : Controller
    {
        // GET: Parties
        private ApplicationDbContext _context;

        public PartiesController()
        {
            _context = new ApplicationDbContext();
        }

        [Authorize]
        [AccessDeniedAuthorizeattribute(ActionName = "ConcernedPartiesIndex")]

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

            ViewBag.Current = "Party";
            var parties = _context.Parties.Include(a=>a.CreatedBy).ToList();
            return View(parties.ToList());
        }


        [Authorize]
        [AccessDeniedAuthorizeattribute(ActionName = "ConcernedPartiesCreate")]
        public ActionResult Create()
        {

            ViewBag.Current = "Partys";

            return View();
        }

        [Authorize]
        [AccessDeniedAuthorizeattribute(ActionName = "ConcernedPartiesCreate")]
        [HttpPost]
        [ValidateAntiForgeryToken]


        public ActionResult Create([Bind(Include = "Id,Name,CreatedAt")] ConcernedParty ConcernedParty)

        {
            ViewBag.Current = "Partys";

            if (_context.Parties.Any(a => a.Name.Equals(Party.Name, StringComparison.OrdinalIgnoreCase)))
            {
                return RedirectToAction("Index", new { Id = "CreateError" });

            }
            if (ModelState.IsValid)
            {

                Party.CreatedAt = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss");
                Party.CreatedById = User.Identity.GetUserId();

                _context.Parties.Add(Party);
                _context.SaveChanges();
                return RedirectToAction("Index", new { Id = "CreateSuccess" });

            }
            return RedirectToAction("Index");

        }


        [Authorize]
        [AccessDeniedAuthorizeattribute(ActionName = "ConcernedPartiesEdit")]
        public ActionResult Edit(int? id)
        {
            ViewBag.Current = "Party";

            if (id == null)
            {
                return RedirectToAction("BadRequestError", "ErrorController");
            }

            Party party = _context.Parties.Find(id);
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
        public ActionResult Edit(Party party)
        {
            if (_context.Parties.Where(a => a.Id != party.Id).Any(a => a.Name.Equals(party.Name, StringComparison.OrdinalIgnoreCase)))
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

        public ActionResult Delete(int? id)
        {
            ViewBag.Current = "Party";


            if (id == null)
            {
                return RedirectToAction("BadRequestError", "ErrorController");

            }

            Party party = _context.Parties.Find(id);
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
            ViewBag.Current = "Party";

            Party party = _context.Parties.Find(id);

            _context.Parties.Remove(party);
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