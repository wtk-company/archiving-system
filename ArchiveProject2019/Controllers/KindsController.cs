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
    public class KindsController : Controller
    {
        
        private ApplicationDbContext _context;

        public KindsController()
        {
            _context = new ApplicationDbContext();
        }



        
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
            var DocKinds = _context.Kinds.Include(a=>a.CreatedBy).Include(a=>a.UpdatedBy).ToList();
            return View(DocKinds.OrderBy(a=>a.CreatedAt).ToList());
        }

        
        [AccessDeniedAuthorizeattribute(ActionName = "DocumentKindsCreate")]

        public ActionResult Create()
        {

            ViewBag.Current = "Kinds";

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        
        [AccessDeniedAuthorizeattribute(ActionName = "DocumentKindsCreate")]
        public ActionResult Create([Bind(Include = "Id,Name,CreatedAt")] Kind Kind)

        {
            ViewBag.Current = "Kinds";

            if (_context.Kinds.Any(a => a.Name.Equals(Kind.Name, StringComparison.OrdinalIgnoreCase)))
            {
                return RedirectToAction("Index", new { Id = "CreateError" });

            }
            if (ModelState.IsValid)
            {
                string NotificationTime = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss");
                Kind.CreatedAt = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss");
                Kind.CreatedById = User.Identity.GetUserId();

                _context.Kinds.Add(Kind);
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
                        Message = "تم إضافة نوع جديد من الوثائق : " + Kind.Name,
                        NotificationOwnerId = UserId
                    };
                    _context.Notifications.Add(notification);
                }
                _context.SaveChanges();

                return RedirectToAction("Index", new { Id = "CreateSuccess" });

            }

            return RedirectToAction("Index");

        }


       

        
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

            ViewBag.OldName = kinds.Name;
            return View(kinds);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]


        
        [AccessDeniedAuthorizeattribute(ActionName = "DocumentKindsEdit")]
        public ActionResult Edit(Kind kinds,string OldName)

        {
            if (_context.Kinds.Where(a => a.Id != kinds.Id).Any(a => a.Name.Equals(kinds.Name, StringComparison.OrdinalIgnoreCase)))
                return RedirectToAction("Index", new { Id = "EditError" });

           

            if (ModelState.IsValid)
            {
                string NotificationTime = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss");
                kinds.UpdatedAt= DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss");
                 kinds.UpdatedById  = User.Identity.GetUserId();
                _context.Entry(kinds).State = EntityState.Modified;
                

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
                        Message = "تم تعديل اسم نوع الوثائق من : " +OldName+" إلى :"+kinds.Name
                        ,
                        NotificationOwnerId = UserId
                    };
                    _context.Notifications.Add(notification);
                }
                _context.SaveChanges();

                return RedirectToAction("Index", new { Id = "EditSuccess" });
            }

            return RedirectToAction("Index");

        }



        
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

        
        [AccessDeniedAuthorizeattribute(ActionName = "DocumentKindsDelete")]
        public ActionResult Confirm(int? id)
        {
            ViewBag.Current = "Kind";

            Kind kind = _context.Kinds.Find(id);
            string NotificationTime = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss");

            _context.Kinds.Remove(kind);
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
                    Message = "تم حذف نوع من الوثائق : " + kind.Name,
                    NotificationOwnerId = UserId
                };
                _context.Notifications.Add(notification);
            }
            _context.SaveChanges();
            return RedirectToAction("Index", new { Id = "DeleteSuccess" });
        }



        
        [AccessDeniedAuthorizeattribute(ActionName = "DocumentKindsDetails")]

        public ActionResult Details(int? id)
        {
            ViewBag.Current = "Kind";


            if (id == null)
            {
                return RedirectToAction("BadRequestError", "ErrorController");

            }

            Kind kind = _context.Kinds.Include(a => a.CreatedBy).Include(a => a.UpdatedBy).SingleOrDefault(a=>a.Id==id);
            if (kind == null)
            {
                return RedirectToAction("HttpNotFoundError", "ErrorController");


            }


            return View(kind);
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