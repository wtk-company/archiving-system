using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ArchiveProject2019.Models;
using Microsoft.AspNet.Identity;
using ArchiveProject2019.HelperClasses;

namespace ArchiveProject2019.Controllers
{
    public class JobTitlesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        
        [AccessDeniedAuthorizeattribute(ActionName = "JobTitlesIndex")]
        public ActionResult Index(string Id="none")
        {

            ViewBag.Current = "JobTitles";

            if (!Id.Equals("none"))
            {
                ViewBag.Msg = Id;

            }
            else
            {
                ViewBag.Msg = null;
            }
            var jobTitles = db.JobTitles.Include(j => j.CreatedBy);
            return View(jobTitles.OrderByDescending(a=>a.CreatedAt).ToList());
        }


        
        [AccessDeniedAuthorizeattribute(ActionName = "JobTitlesDetails")]
        public ActionResult Details(int? id)
        {
            ViewBag.Current = "JobTitles";

            if (id == null)
            {
                return RedirectToAction("BadRequestError", "ErrorController");
            }
            JobTitle jobTitle = db.JobTitles.Include(a=>a.UpdatedBy).Include(a => a.CreatedBy).SingleOrDefault(a=>a.Id==id);
            if (jobTitle == null)
            {
                return RedirectToAction("HttpNotFoundError", "ErrorController");
            }
            return View(jobTitle);
        }


        
        [AccessDeniedAuthorizeattribute(ActionName = "JobTitlesCreate")]
        public ActionResult Create()
        {
            ViewBag.Current = "JobTitles";

            return View();
        }


        
        [AccessDeniedAuthorizeattribute(ActionName = "JobTitlesCreate")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Symbol,MaximumMember")] JobTitle jobTitle)
        {
            ViewBag.Current = "JobTitles";

            if (ModelState.IsValid)
            {

                //Dublicated
                if(db.JobTitles.Any(a=>a.Name.Equals(jobTitle.Name,StringComparison.OrdinalIgnoreCase)))
                {
                    return RedirectToAction("Index", new { Id = "CreateError" });


                }

                jobTitle.CreatedAt = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss");
                    jobTitle.CreatedById = this.User.Identity.GetUserId();
                db.JobTitles.Add(jobTitle);
                db.SaveChanges();

                string NotificationTime = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss");

                string UserId = User.Identity.GetUserId();
                Notification notification = null;
                List<ApplicationUser> Users = db.Users.Where(a => a.RoleName.Equals("Master") && !a.Id.Equals(UserId)).ToList();
                foreach (ApplicationUser user in Users)
                {

                    notification = new Notification()
                    {

                        CreatedAt = NotificationTime,
                        Active = false,
                        UserId = user.Id,
                        Message = "تم إضافة مسمى وظيفي جديد : " + jobTitle.Name
                       ,
                        NotificationOwnerId = UserId
                    };
                    db.Notifications.Add(notification);
                }
                db.SaveChanges();

                return RedirectToAction("Index", new { Id = "CreateSuccess" });
        
            }


            return RedirectToAction("Index");
        }


        
        [AccessDeniedAuthorizeattribute(ActionName = "JobTitlesEdit")]
        public ActionResult Edit(int? id)
        {
            ViewBag.Current = "JobTitles";

            if (id == null)
            {
                return RedirectToAction("BadRequestError", "ErrorController");
            }
            JobTitle jobTitle = db.JobTitles.Find(id);
            if (jobTitle == null)
            {
                return RedirectToAction("HttpNotFoundError", "ErrorController");
            }
            ViewBag.OldName = jobTitle.Name;

            return View(jobTitle);
        }

     
        [HttpPost]
        [ValidateAntiForgeryToken]


        
        [AccessDeniedAuthorizeattribute(ActionName = "JobTitlesEdit")]
        public ActionResult Edit([Bind(Include = "Id,Name,Symbol,MaximumMember,CreatedAt,CreatedById")] JobTitle jobTitle,string OldName)
        {
            ViewBag.Current = "JobTitles";

            if (ModelState.IsValid)
            {


                if (db.Fields.Where(a => a.Id!=jobTitle.Id).Any(a => a.Name.Equals(jobTitle.Name, StringComparison.OrdinalIgnoreCase)) == true)
                {

                    return RedirectToAction("Index", new {  Id = "EditError" });
                }

              //  Numbers
                List<ApplicationUser> Users = db.Users.Include(a => a.Department).Where(a => a.JobTitleId == jobTitle.Id).ToList();
                var EG = Users.GroupBy(a => a.Department.Id);
                bool x = true;
                foreach (var v in EG)
                {
                    if (v.Count() > jobTitle.MaximumMember)
                    {
                        x = false;
                        break;
                    }
                }

                if (x == false)
                {
                    return RedirectToAction("Index", new { Id = "EditErrorMaxNumber" });

                }

                jobTitle.UpdatedAt = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss");
            
                db.Entry(jobTitle).State = EntityState.Modified;
                string NotificationTime = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss");
                jobTitle.UpdatedById = this.User.Identity.GetUserId();
                db.SaveChanges();

                string UserId = User.Identity.GetUserId();
                Notification notification = null;
                List<string> UserId1 = db.Users.Where(a => a.RoleName.Equals("Master") && !a.Id.Equals(UserId)).Select(a => a.Id).ToList();
                List<string> UsersId2 = db.Users.Where(a => a.JobTitleId == jobTitle.Id).Select(a => a.Id).ToList();
                List<string> UsersId = UserId1.DefaultIfEmpty().Union(UsersId2.DefaultIfEmpty()).ToList();
                List<ApplicationUser> UsersNot = db.Users.Where(a => UsersId.Contains(a.Id)).ToList();
                foreach (ApplicationUser user in UsersNot)
                {

                    notification = new Notification()
                    {

                        CreatedAt = NotificationTime,
                        Active = false,
                        UserId = user.Id,
                        Message = "تم تعديل المسمى الوظيفي من :" + OldName + " إلى :" + jobTitle.Name
                       ,
                        NotificationOwnerId = UserId
                    };
                    db.Notifications.Add(notification);
                }
                db.SaveChanges();


                return RedirectToAction("Index", new { Id = "EditSuccess" });

            }
            return View(jobTitle);
        }


        
        [AccessDeniedAuthorizeattribute(ActionName = "JobTitlesDelete")]
        public ActionResult Delete(int? id)
        {
            ViewBag.Current = "JobTitles";


            if (id == null)
            {
                return RedirectToAction("BadRequestError", "ErrorController");
            }
            JobTitle jobTitle = db.JobTitles.Find(id);
            if (jobTitle == null)
            {
                return RedirectToAction("HttpNotFoundError", "ErrorController");
            }


            if (CheckDelete.CheckJobTitleDelete(jobTitle.Id) == false)
            {
                return RedirectToAction("Index", new { Id = "DeleteError" });


            }
            return View(jobTitle);
        }


        
        [AccessDeniedAuthorizeattribute(ActionName = "JobTitlesDelete")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ViewBag.Current = "JobTitles";

            JobTitle jobTitle = db.JobTitles.Find(id);
            db.JobTitles.Remove(jobTitle);
            db.SaveChanges();

            string NotificationTime = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss");

            string UserId = User.Identity.GetUserId();
            Notification notification = null;
            List<ApplicationUser> Users = db.Users.Where(a => a.RoleName.Equals("Master") && !a.Id.Equals(UserId)).ToList();
            foreach (ApplicationUser user in Users)
            {

                notification = new Notification()
                {

                    CreatedAt = NotificationTime,
                    Active = false,
                    UserId = user.Id,
                    Message = "تم حذف المسمى الوظيفي  : " + jobTitle.Name
                   ,
                    NotificationOwnerId = UserId
                };
                db.Notifications.Add(notification);
            }
            db.SaveChanges();
            return RedirectToAction("Index", new { Id = "DeleteSuccess" });

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
