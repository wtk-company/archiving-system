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

        
        [AccessDeniedAuthorizeattribute(ActionName = "GroupsIndex")]
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
            
            return View(Groups.OrderByDescending(a=>a.CreatedAt).ToList());
        }
        
        [AccessDeniedAuthorizeattribute(ActionName = "GroupsCreate")]
        public ActionResult Create()
        {

            ViewBag.Current = "Groups";

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        
        [AccessDeniedAuthorizeattribute(ActionName = "GroupsCreate")]
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


                string NotificationTime = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss");

                string UserId = User.Identity.GetUserId();
                Notification notification = null;
                List<ApplicationUser> Users = _context.Users.Where(a => a.RoleName.Equals("Master") && !a.Id.Equals(UserId)).ToList();
                foreach (ApplicationUser user in Users)
                {

                    notification = new Notification()
                    {

                        CreatedAt = NotificationTime,
                        Active = false,
                        UserId = user.Id,
                        Message = "تم إضافة مجموعة جديدة : " + Group.Name
                       ,
                        NotificationOwnerId = UserId
                    };
                    _context.Notifications.Add(notification);
                }
                _context.SaveChanges();
                return RedirectToAction("Index", new { Id = "CreateSuccess" });
            }

            return RedirectToAction("Index", new { Id = "CreateError" });
        }

        
        [AccessDeniedAuthorizeattribute(ActionName = "GroupsEdit")]
        public ActionResult Edit(int? id)
        {
            ViewBag.Current = "Group";
            
            if (id == null)
            {
                return RedirectToAction("BadRequestError", "ErrorController");
            }

            Group Group = _context.Groups.Find(id);
            if (Group == null)
            {
                return RedirectToAction("HttpNotFoundError","ErrorController");
            }
            
            ViewBag.OldName = Group.Name;

            return View(Group);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        
        [AccessDeniedAuthorizeattribute(ActionName = "GroupsEdit")]
        public ActionResult Edit(Group Group,string OldName)
        {
            if (_context.Groups.Where(a => a.Id != Group.Id).Any(a => a.Name.Equals(Group.Name, StringComparison.OrdinalIgnoreCase)))
                return RedirectToAction("Index", new { Id = "EditError" });
            
            if (ModelState.IsValid)
            {

                Group.UpdatedAt = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss");
                string NotificationTime = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss");
                Group.UpdatedById= User.Identity.GetUserId();
                _context.Entry(Group).State = EntityState.Modified;
                _context.SaveChanges();


                string UserId = User.Identity.GetUserId();
                Notification notification = null;
                List<string> UserId1 = _context.Users.Where(a => a.RoleName.Equals("Master")&& !a.Id.Equals(UserId)).Select(a => a.Id).ToList();
                List<string> UsersId2 = _context.UsersGroups.Where(a => a.GroupId == Group.Id).Select(a => a.UserId).ToList();
                List<string> UsersId = UserId1.DefaultIfEmpty().Union(UsersId2.DefaultIfEmpty()).ToList();
                List<ApplicationUser> Users = _context.Users.Where(a => UsersId.Contains(a.Id)).ToList();
                foreach (ApplicationUser user in Users)
                {

                    notification = new Notification()
                    {

                        CreatedAt = NotificationTime,
                        Active = false,
                        UserId = user.Id,
                        Message = "تم تعديل المجموعة من :"+OldName +" إلى :"+ Group.Name
                       ,
                        NotificationOwnerId = UserId
                    };
                    _context.Notifications.Add(notification);
                }
                _context.SaveChanges();
                return RedirectToAction("Index", new { Id = "EditSuccess" });
            }

            return RedirectToAction("Index", new { Id = "EditError" });
        }

        
        [AccessDeniedAuthorizeattribute(ActionName = "GroupsDelete")]
        public ActionResult Delete(int? id)
        {
            ViewBag.Current = "Group";


            if (id == null)
            {
                return RedirectToAction("BadRequestError", "ErrorController");
            }

            Group Group = _context.Groups.Find(id);
            if (Group == null)
            {
                return RedirectToAction("HttpNotFoundError", "ErrorController");
            }
            if (CheckDelete.CheckGroupDelete(id.Value) == false)
            {
                return RedirectToAction("Index", new { Id = "DeleteError" });

            }

            return View(Group);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]

        
        [AccessDeniedAuthorizeattribute(ActionName = "GroupsDelete")]
        public ActionResult Confirm(int? id)
        {
            ViewBag.Current = "Group";

            Group Group = _context.Groups.Find(id);

            _context.Groups.Remove(Group);
            _context.SaveChanges();
            string NotificationTime = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss");

            string UserId = User.Identity.GetUserId();
            Notification notification = null;
            List<ApplicationUser> Users = _context.Users.Where(a => a.RoleName.Equals("Master") && !a.Id.Equals(UserId)).ToList();
            foreach (ApplicationUser user in Users)
            {

                notification = new Notification()
                {

                    CreatedAt = NotificationTime,
                    Active = false,
                    UserId = user.Id,
                    Message = "تم حذف المجموعة  : " + Group.Name
                   ,
                    NotificationOwnerId = UserId
                };
                _context.Notifications.Add(notification);
            }
            _context.SaveChanges();

            return RedirectToAction("Index", new { Id = "DeleteSuccess" });
        }


        
        [AccessDeniedAuthorizeattribute(ActionName = "GroupsDetails")]
        public ActionResult Details(int? id)
        {
            ViewBag.Current = "Group";


            if (id == null)
            {
                return RedirectToAction("BadRequestError", "ErrorController");
            }

            Group Group = _context.Groups.Include(a => a.CreatedBy).Include(a=>a.UpdatedBy).SingleOrDefault(a=>a.Id==id);
            if (Group == null)
            {
                return RedirectToAction("HttpNotFoundError", "ErrorController");
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