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
            var parties = _context.Parties.ToList();
            return View(parties.OrderByDescending(a=>a.CreatedAt).ToList());
        }


        
        [AccessDeniedAuthorizeattribute(ActionName = "ConcernedPartiesCreate")]
        public ActionResult Create()
        {

            ViewBag.Current = "Partys";

            return View();
        }

        
        [AccessDeniedAuthorizeattribute(ActionName = "ConcernedPartiesCreate")]
        [HttpPost]
        [ValidateAntiForgeryToken]


        public ActionResult Create([Bind(Include = "Id,Name,CreatedAt")] Party Party)

        {
            ViewBag.Current = "Partys";

            if (_context.Parties.Any(a => a.Name.Equals(Party.Name, StringComparison.OrdinalIgnoreCase)))
            {
                return RedirectToAction("Index", new { Id = "CreateError" });

            }
            if (ModelState.IsValid)
            {
                string NotificationTime = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss");

                Party.CreatedAt = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss");
                Party.CreatedById = User.Identity.GetUserId();

                _context.Parties.Add(Party);
                _context.SaveChanges();

                string UserId = User.Identity.GetUserId();
                Notification notification = null;
                List<ApplicationUser> Users = _context.Users.Where(a => !a.Id.Equals(UserId)).ToList();
                foreach (ApplicationUser user in Users)
                {

                    notification = new Notification()
                    {

                        CreatedAt = NotificationTime,
                        Active = false,
                        UserId = user.Id,
                        Message = "تم إضافة نوع جديد من الجهات : " + Party.Name,
                        NotificationOwnerId = UserId
                    };
                    _context.Notifications.Add(notification);
                }
                _context.SaveChanges();


                return RedirectToAction("Index", new { Id = "CreateSuccess" });

            }
            return RedirectToAction("Index");

        }


        
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
            ViewBag.OldName = party.Name;
            return View(party);
        }

        
        [AccessDeniedAuthorizeattribute(ActionName = "ConcernedPartiesEdit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Party party,string OldName)
        {
            if (_context.Parties.Where(a => a.Id != party.Id).Any(a => a.Name.Equals(party.Name, StringComparison.OrdinalIgnoreCase)))
                return RedirectToAction("Index", new { Id = "EditError" });

            if (ModelState.IsValid)
            {
                party .UpdatedAt= DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss");
                string NotificationTime = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss");
                party.UpdatedById = User.Identity.GetUserId();

                _context.Entry(party).State = EntityState.Modified;


                _context.SaveChanges();

                string UserId = User.Identity.GetUserId();
                Notification notification = null;
                List<ApplicationUser> Users = _context.Users.Where(a => !a.Id.Equals(UserId)).ToList();
                foreach (ApplicationUser user in Users)
                {

                    notification = new Notification()
                    {

                        CreatedAt = NotificationTime,
                        Active = false,
                        UserId = user.Id,
                        Message = "تم تعديل اسم الجهة من :"+OldName+" إلى الاسم :" + party.Name,
                        NotificationOwnerId = UserId
                    };
                    _context.Notifications.Add(notification);
                }
                _context.SaveChanges();


                return RedirectToAction("Index", new { Id = "EditSuccess" });
            }

            return RedirectToAction("Index");
        }



        
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

            if (CheckDelete.CheckPartydelete(party.Id) == false)
            {
                return RedirectToAction("Index", new { Id = "DeleteError" });


            }

            return View(party);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        
        [AccessDeniedAuthorizeattribute(ActionName = "ConcernedPartiesDelete")]
        public ActionResult Confirm(int? id)
        {
            ViewBag.Current = "Party";

            Party party = _context.Parties.Find(id);
            string NotificationTime = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss");

            _context.Parties.Remove(party);
            _context.SaveChanges();

            string UserId = User.Identity.GetUserId();
            Notification notification = null;
            List<ApplicationUser> Users = _context.Users.Where(a => !a.Id.Equals(UserId)).ToList();
            foreach (ApplicationUser user in Users)
            {

                notification = new Notification()
                {

                    CreatedAt = NotificationTime,
                    Active = false,
                    UserId = user.Id,
                    Message = "تم حذف اسم الجهة  :" + party.Name,
                    NotificationOwnerId = UserId
                };
                _context.Notifications.Add(notification);
            }
            _context.SaveChanges();


            return RedirectToAction("Index", new { Id = "DeleteSuccess" });
        }


        
      [AccessDeniedAuthorizeattribute(ActionName = "ConcernedPartiesDetails")]

        public ActionResult Details(int? id)
        {
            ViewBag.Current = "Party";


            if (id == null)
            {
                return RedirectToAction("BadRequestError", "ErrorController");

            }

            Party party = _context.Parties.Include(a=>a.CreatedBy).Include(a=>a.UpdatedBy).SingleOrDefault(a=>a.Id==id);
            if (party == null)
            {
                return RedirectToAction("HttpNotFoundError", "ErrorController");

            }

            return View(party);
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