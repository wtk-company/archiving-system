using ArchiveProject2019.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ArchiveProject2019.HelperClasses;
namespace ArchiveProject2019.Controllers
{
    public class GroupsController : Controller
    {
        private ApplicationDbContext _context;

        public GroupsController()
        {
            _context = new ApplicationDbContext();
        }

        //Get All Group
        public ActionResult Index(string Id = "none")
        {
            ViewBag.Current = "Group";
            
            var Groups = _context.Groups.ToList();

            if (Id != "none")
            {
                ViewBag.Msg = Id;

            }
            else
            {
                ViewBag.Msg = null;
            }
            
            return View(Groups);
        }
        
        public ActionResult Create()
        {

            ViewBag.Current = "Groups";

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Group Group)
        {
            ViewBag.Current = "Group";
            
            if (_context.Groups.Any(g => g.Name.Equals(Group.Name, StringComparison.OrdinalIgnoreCase)))
                    return RedirectToAction("Index", new { Id = "CreateError" });

            if (ModelState.IsValid)
            {
                Group.CreatedAt = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss");
                Group.CreatedById = User.Identity.GetUserId();

                _context.Groups.Add(Group);
                _context.SaveChanges();

                return RedirectToAction("Index", new { Id = "CreateSuccess" });
            }

            return RedirectToAction("Index", new { Id = "CreateError" });
        }

        // Edit Department:
        public ActionResult Edit(int? id)
        {
            ViewBag.Current = "Group";
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Group Group = _context.Groups.Find(id);
            if (Group == null)
            {
                return HttpNotFound();
            }
            
            return View(Group);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Group Group)
        {
            if (_context.Groups.Where(a => a.Id != Group.Id).Any(a => a.Name.Equals(Group.Name, StringComparison.OrdinalIgnoreCase)))
                return RedirectToAction("Index", new { Id = "EditError" });
            
            if (ModelState.IsValid)
            {

                Group.UpdatedAt = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss");
                Group.UpdatedById = User.Identity.GetUserId();
                _context.Entry(Group).State = EntityState.Modified;
                _context.SaveChanges();

                return RedirectToAction("Index", new { Id = "EditSuccess" });
            }

            return RedirectToAction("Index", new { Id = "EditError" });
        }

        //Delete Department:
        public ActionResult Delete(int? id)
        {
            ViewBag.Current = "Group";


            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Group Group = _context.Groups.Find(id);
            if (Group == null)
            {
                return HttpNotFound();
            }
            if (CheckDelete.CheckGroupDelete(id.Value) == false)
            {
                return RedirectToAction("Index", new { Id = "DeleteError" });

            }

            return View(Group);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Confirm(int? id)
        {
            ViewBag.Current = "Group";

            Group Group = _context.Groups.Find(id);

            _context.Groups.Remove(Group);
            _context.SaveChanges();

            return RedirectToAction("Index", new { Id = "DeleteSuccess" });
        }


        public ActionResult Details(int? id)
        {
            ViewBag.Current = "Group";


            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Group Group = _context.Groups.Include(async => async.CreatedBy).Include(a => a.UpdatedBy).SingleOrDefault(a=>a.Id==id);
            if (Group == null)
            {
                return HttpNotFound();
            }
           

            return View(Group);
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