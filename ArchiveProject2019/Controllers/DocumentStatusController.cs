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
    public class DocumentStatusController : Controller
    {

        private ApplicationDbContext _context;

        public DocumentStatusController()
        {
            _context = new ApplicationDbContext();
        }


        
       [AccessDeniedAuthorizeattribute(ActionName = "DocumentStatusIndex")]

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

            ViewBag.Current = "DocumentStatus";
            var Status = _context.DocumentStatuses.ToList();
            return View(Status.OrderByDescending(a=>a.CreatedAt).ToList());
        }

        
        [AccessDeniedAuthorizeattribute(ActionName = "DocumentStatusCreate")]

        public ActionResult Create()
        {

            ViewBag.Current = "DocumentStatus";

            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]

        
        [AccessDeniedAuthorizeattribute(ActionName = "DocumentStatusCreate")]

        public ActionResult Create([Bind(Include = "Id,Name,CreatedAt")] DocumentStatus Status)

        {
            ViewBag.Current = "DocumentStatus";


            if (_context.DocumentStatuses.Any(a => a.Name.Equals(Status.Name, StringComparison.OrdinalIgnoreCase)))
            {
                return RedirectToAction("Index", new { Id = "CreateError" });

            }
            if (ModelState.IsValid)
            {
                string NotificationTime = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss");


                Status.CreatedAt = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss");
                Status.CreatedById = User.Identity.GetUserId();

                _context.DocumentStatuses.Add(Status);
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
                        Message = "تم إضافة  حالة وثيقة جديدة   : " + Status.Name,
                        NotificationOwnerId = UserId
                    };
                    _context.Notifications.Add(notification);
                }
                _context.SaveChanges();


                return RedirectToAction("Index", new { Id = "CreateSuccess" });

            }

            return RedirectToAction("Index");

        }


        
        [AccessDeniedAuthorizeattribute(ActionName = "DocumentStatusEdit")]

        public ActionResult Edit(int? id)
        {
            ViewBag.Current = "DocumentStatus";

            if (id == null)
            {
                return RedirectToAction("BadRequestError", "ErrorController");
            }

            DocumentStatus status = _context.DocumentStatuses.Find(id);
            if (status == null)
            {
                return RedirectToAction("HttpNotFoundError", "ErrorController");
            }

            ViewBag.OldName = status.Name;
            return View(status);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]


        
        [AccessDeniedAuthorizeattribute(ActionName = "DocumentStatusEdit")]

        public ActionResult Edit(DocumentStatus status,string OldName)

        {

            ViewBag.Current = "DocumentStatus";

            if (_context.DocumentStatuses.Where(a => a.Id != status.Id).Any(a => a.Name.Equals(status.Name, StringComparison.OrdinalIgnoreCase)))
                return RedirectToAction("Index", new { Id = "EditError" });

            if (ModelState.IsValid)
            {
                status.UpdatedById= User.Identity.GetUserId(); 
                status.UpdatedAt = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss");
                string NotificationTime = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss");

                _context.Entry(status).State = EntityState.Modified;
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
                        Message = "تم تعديل اسم حالة الوثيقة من : " + OldName + " إلى :" + status.Name
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


        
        [AccessDeniedAuthorizeattribute(ActionName = "DocumentStatusDelete")]

        public ActionResult Delete(int? id)
        {
            ViewBag.Current = "DocumentStatus";


            if (id == null)
            {
                return RedirectToAction("BadRequestError", "ErrorController");
            }

            DocumentStatus ststus = _context.DocumentStatuses.Find(id);
            if (ststus == null)
            {
                return RedirectToAction("HttpNotFoundError", "ErrorController");
            }

            return View(ststus);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]

        
        [AccessDeniedAuthorizeattribute(ActionName = "DocumentStatusDelete")]

        public ActionResult Confirm(int? id)
        {
            ViewBag.Current = "DocumentStatus";


            DocumentStatus status = _context.DocumentStatuses.Find(id);

            _context.DocumentStatuses.Remove(status);
            _context.SaveChanges();

            string NotificationTime = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss");

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
                    Message = "تم حذف   حالة الوثائق : " + status.Name,
                    NotificationOwnerId = UserId
                };
                _context.Notifications.Add(notification);
            }
            _context.SaveChanges();



            return RedirectToAction("Index", new { Id = "DeleteSuccess" });
        }


        
        [AccessDeniedAuthorizeattribute(ActionName = "DocumentStatusDetails")]

        public ActionResult Details(int? id)
        {
            ViewBag.Current = "DocumentStatus";


            if (id == null)
            {
                return RedirectToAction("BadRequestError", "ErrorController");
            }

            DocumentStatus status = _context.DocumentStatuses.Include(a => a.CreatedBy).Include(a => a.UpdatedBy).SingleOrDefault(a => a.Id == id);
            if (status == null)
            {
                return RedirectToAction("HttpNotFoundError", "ErrorController");
            }

            return View(status);
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