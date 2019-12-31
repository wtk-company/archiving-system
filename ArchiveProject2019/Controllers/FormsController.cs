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
    public class FormsController : Controller
    {
        private ApplicationDbContext _context;

        public FormsController ()
        {
            _context = new ApplicationDbContext();
        }

      
        [AccessDeniedAuthorizeattribute(ActionName = "FormsIndex")]
        public ActionResult Index(string Id="none")
        {
            if (!Id.Equals("none"))
            {
                ViewBag.Msg = Id;

            }
            else
            {
                ViewBag.Msg = null;
            }
            string UID = this.User.Identity.GetUserId();
            ViewBag.Current = "Forms";
            var forms = _context.Forms;
            return View(forms.OrderByDescending(a=>a.CreatedAt).ToList());
        }



        
        [AccessDeniedAuthorizeattribute(ActionName = "FormsCreate")]
        public ActionResult Create()
        {

            ViewBag.Current = "Forms";

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]


        
        [AccessDeniedAuthorizeattribute(ActionName = "FormsCreate")]
        public ActionResult Create([Bind(Include = "Id,Name,CreatedAt")] Form form)
        {///
            ViewBag.Current = "Forms";

            if (_context.Forms.Any(a => a.Name.Equals(form.Name, StringComparison.OrdinalIgnoreCase)))
            {
                return RedirectToAction("Index", new { Id = "CreateError" });

            }
            if (ModelState.IsValid)
            {

                form.CreatedAt = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss");
                form.CreatedById = User.Identity.GetUserId();
                form.Type = 0;
                _context.Forms.Add(form);


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
                        Message = "تم إضافة نموذج جديد : " + form.Name
                       ,
                        NotificationOwnerId = UserId
                    };
                    _context.Notifications.Add(notification);
                }
                _context.SaveChanges();

                return RedirectToAction("Index", new { Id = "CreateSuccess" });

            }


            return RedirectToAction("Index");
        }


        
        [AccessDeniedAuthorizeattribute(ActionName = "FormsEdit")]
        public ActionResult Edit(int? id)
        {

            ViewBag.Current = "Forms";



            if (id == null)
            {
                return RedirectToAction("BadRequestError", "ErrorController");
            }
            Form form = _context.Forms.Find(id);
            if (form == null)
            {
                return RedirectToAction("HttpNotFoundError", "ErrorController");
            }


            ViewBag.Oldname = form.Name;

            return View(form);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        
        [AccessDeniedAuthorizeattribute(ActionName = "FormsEdit")]
        public ActionResult Edit([Bind(Include = "Id,Name,CreatedAt,CreatedById")] Form form,string OldName)
        {
            ViewBag.Current = "Forms";


            if (_context.Forms.Where(a=>a.Id!=form.Id).Any(a=>a.Name.Equals(form.Name,StringComparison.OrdinalIgnoreCase)))
            {
                return RedirectToAction("Index", new { Id = "EditError" });

            }

            if (ModelState.IsValid)
            {
                form.UpdatedAt = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss");
                string NotificationTime = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss");
                form.UpdatedById = User.Identity.GetUserId();
                form.Type = 0;

                _context.Entry(form).State = EntityState.Modified;
                _context.SaveChanges();

                string UserId = User.Identity.GetUserId();
                Notification notification = null;

                List<int> DepartmentsFormId = _context.FormDepartments.Where(a => a.FormId == form.Id).Select(a => a.DepartmentId).ToList();
                List<int> GroupsFormId = _context.FormGroups.Where(a => a.FormId == form.Id).Select(a => a.GroupId).ToList();
                List<string> UserId1 = _context.Users.Where(a => a.RoleName.Equals("Master") && !a.Id.Equals(UserId)).Select(a => a.Id).ToList();
                List<string> UsersId2 = _context.Users.Where(a => DepartmentsFormId.Contains(a.DepartmentId.Value)).Select(a=>a.Id).ToList();
                List<string> UserId3 = _context.UsersGroups.Where(a => GroupsFormId.Contains(a.GroupId)).Select(a => a.UserId).ToList();

                List<string> UsersId = UserId1.DefaultIfEmpty().Union(UsersId2.DefaultIfEmpty()).Union(UserId3.DefaultIfEmpty()).Distinct().ToList();
                List<ApplicationUser> Users = _context.Users.Where(a => UsersId.Contains(a.Id)).ToList();
                foreach (ApplicationUser user in Users)
                {

                    notification = new Notification()
                    {

                        CreatedAt = NotificationTime,
                        Active = false,
                        UserId = user.Id,
                        Message = "تم تعديل اسم النموذج من :" + OldName + " إلى :" + form.Name
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


        
        [AccessDeniedAuthorizeattribute(ActionName = "FormsDelete")]
        public ActionResult Delete(int? id)
        {

            ViewBag.Current = "Forms";


            if (id == null)
            {
                return RedirectToAction("BadRequestError", "ErrorController");
            }
            Form form = _context.Forms.Include(a=>a.Fields).Include(a=>a.CreatedBy).SingleOrDefault(a=>a.Id==id);
            if (form == null)
            {
                return RedirectToAction("HttpNotFoundError", "ErrorController");
            }
            
            IEnumerable<Document> Documents = _context.Documents.Where(a => a.FormId == id);
            if(CheckDelete.checkFormDelete(id.Value)==false)
            {
                return RedirectToAction("Index", new { Id = "DeleteError" });

            }

            return View(form);
           
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]

        
        [AccessDeniedAuthorizeattribute(ActionName = "FormsDelete")]
        public ActionResult Confirm(int? id)
        {

            ViewBag.Current = "Forms";

            Form cat = _context.Forms.Find(id);

            // Delete Fields
           List<Field> Fields = _context.Fields.Where(a=>a.FormId==id).ToList();
            foreach(Field f in Fields)
            {
                _context.Fields.Remove(f);
            }

            _context.Forms.Remove(cat);
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
                    Message = "تم إزالة نموذج  : " + cat.Name
                   ,
                    NotificationOwnerId = UserId
                };
                _context.Notifications.Add(notification);
            }
            _context.SaveChanges();
            return RedirectToAction("Index", new { Id = "DeleteSuccess" });

        }


        
       [AccessDeniedAuthorizeattribute(ActionName = "FormsDetails")]
        public ActionResult Details(int? id)
        {

            ViewBag.Current = "Forms";


            if (id == null)
            {
                return RedirectToAction("BadRequestError", "ErrorController");
            }
            Form form = _context.Forms.Include(a=>a.UpdatedBy).Include(a => a.CreatedBy).SingleOrDefault(a => a.Id == id);
            if (form == null)
            {
                return RedirectToAction("HttpNotFoundError", "ErrorController");
            }

            if (CheckDelete.checkFormDelete(id.Value) == false)
            {
                return RedirectToAction("Index", new { Id = "DeleteError" });

            }

            return View(form);

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
