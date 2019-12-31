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
            var TypeMails = _context.TypeMails.ToList();
            return View(TypeMails.OrderByDescending(a=>a.CreatedAt).ToList());
        }

        
        [AccessDeniedAuthorizeattribute(ActionName = "TypeMailsCreate")]
        public ActionResult Create()
        {

            ViewBag.Current = "TypeMails";

            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]

        
        [AccessDeniedAuthorizeattribute(ActionName = "TypeMailsCreate")]
        public ActionResult Create([Bind(Include = "Id,Name,CreatedAt")] TypeMail TypeMail)

        {
            ViewBag.Current = "TypeMails";

            if (_context.TypeMails.Any(a => a.Name.Equals(TypeMail.Name, StringComparison.OrdinalIgnoreCase)))
            {
                return RedirectToAction("Index", new { Id = "CreateError" });

            }
            if (ModelState.IsValid)
            {
                string NotificationTime = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss");


                TypeMail.CreatedAt = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss");
                TypeMail.CreatedById = User.Identity.GetUserId();

                _context.TypeMails.Add(TypeMail);
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
                        Message = "تم إضافة نوع جديد من البريد : " + TypeMail.Name,
                        NotificationOwnerId = UserId
                    };
                    _context.Notifications.Add(notification);
                }
                _context.SaveChanges();


                return RedirectToAction("Index", new { Id = "CreateSuccess" });

            }

            return RedirectToAction("Index");

        }


        
        [AccessDeniedAuthorizeattribute(ActionName = "TypeMailsEdit")]
        public ActionResult Edit(int? id)
        {
            ViewBag.Current = "TypeMails";

            if (id == null)
            {
                return RedirectToAction("BadRequestError", "ErrorController");
            }

            TypeMail mail = _context.TypeMails.Find(id);
            if (mail == null)
            {
                return RedirectToAction("HttpNotFoundError", "ErrorController");
            }

            ViewBag.OldName = mail.Name;
            return View(mail);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]


        
        [AccessDeniedAuthorizeattribute(ActionName = "TypeMailsEdit")]
        public ActionResult Edit(TypeMail mail,string OldName)

        {

            ViewBag.Current = "TypeMails";

            if (_context.TypeMails.Where(a => a.Id != mail.Id).Any(a => a.Name.Equals(mail.Name, StringComparison.OrdinalIgnoreCase)))
                return RedirectToAction("Index", new { Id = "EditError" });

            if (ModelState.IsValid)
            {

                mail.UpdatedAt = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss");
                string NotificationTime = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss");
                mail.UpdatedById= User.Identity.GetUserId(); 
                _context.Entry(mail).State = EntityState.Modified;
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
                        Message = "تم تعديل اسم نوع البريد من : " + OldName + " إلى :" + mail.Name
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


        
        [AccessDeniedAuthorizeattribute(ActionName = "TypeMailsDelete")]
        public ActionResult Delete(int? id)
        {
            ViewBag.Current = "TypeMails";


            if (id == null)
            {
                return RedirectToAction("BadRequestError", "ErrorController");
            }

            TypeMail mail = _context.TypeMails.Find(id);
            if (mail == null)
            {
                return RedirectToAction("HttpNotFoundError", "ErrorController");
            }

            return View(mail);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]

        
        [AccessDeniedAuthorizeattribute(ActionName = "TypeMailsDelete")]
        public ActionResult Confirm(int? id)
        {
            ViewBag.Current = "TypeMails";


            TypeMail mail = _context.TypeMails.Find(id);

            _context.TypeMails.Remove(mail);
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
                    Message = "تم حذف نوع من البريد : " + mail.Name,
                    NotificationOwnerId = UserId
                };
                _context.Notifications.Add(notification);
            }
            _context.SaveChanges();



            return RedirectToAction("Index", new { Id = "DeleteSuccess" });
        }



        
       [AccessDeniedAuthorizeattribute(ActionName = "TypeMailsDetails")]
        public ActionResult Details(int? id)
        {
            ViewBag.Current = "TypeMails";


            if (id == null)
            {
                return RedirectToAction("BadRequestError", "ErrorController");
            }

            TypeMail mail = _context.TypeMails.Include(a=>a.CreatedBy).Include(a=>a.UpdatedBy).SingleOrDefault(a=>a.Id==id);
            if (mail == null)
            {
                return RedirectToAction("HttpNotFoundError", "ErrorController");
            }

            return View(mail);
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