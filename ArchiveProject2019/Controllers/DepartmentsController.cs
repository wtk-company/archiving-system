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
using Microsoft.AspNet.Identity.EntityFramework;
using ArchiveProject2019.HelperClasses;
using System.Web.Security;

namespace ArchiveProject2019.Controllers
{
    public class DepartmentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        [AccessDeniedAuthorizeattribute(ActionName = "DepartmentsIndex")]
        public ActionResult Index(string Id = "none")
        {
            ViewBag.Current = "Department";


            var departments = db.Departments.Include(a => a.ParentDepartment).Include(a => a.ChildDepartment);
            if (Id != "none")
            {
                ViewBag.Msg = Id;

            }
            else
            {
                ViewBag.Msg = null;
            }


            return View(departments.OrderByDescending(a=>a.CreatedAt).ToList());
        }


        [AccessDeniedAuthorizeattribute(ActionName = "DepartmentsDetails")]
        public ActionResult Details(int ?id)
        {
            if(id==null)
            {
                return RedirectToAction("BadRequestError", "ErrorController");

            }

            Department department = db.Departments.Include(a => a.CreatedBy).Include(a=>a.UpdatedBy).SingleOrDefault(a=>a.Id==id);
            if(department==null)
            {
                return RedirectToAction("HttpNotFoundError", "ErrorController");

            }


            return View(department);
        }




        [AccessDeniedAuthorizeattribute(ActionName = "DepartmentsUsers")]
        public ActionResult DepartmentUsers(int? Id)
        {

            if(Id==null)
            {
                return RedirectToAction("BadRequestError", "ErrorController");

            }

            Department department = db.Departments.Find(Id);
            if(department==null)
            {
                return RedirectToAction("HttpNotFoundError", "ErrorController");


            }
            IEnumerable<ApplicationUser> Users = db.Users.Where(a=>a.DepartmentId==Id);


            return View(Users);

        }

    
        [AccessDeniedAuthorizeattribute(ActionName = "DepartmentsCreate")]
        public ActionResult Create(int id,int msg)
        {
            ViewBag.Current = "Department";
            Session["Parent_Id"] = msg;




            return View();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
    
        [AccessDeniedAuthorizeattribute(ActionName = "DepartmentsCreate")]
        public ActionResult Create([Bind(Include = "Id,Name,CreatedAt,CreatedById")] Department department)
        {
            ViewBag.Current = "Department";

            if (Convert.ToInt32(Session["Parent_Id"]) != 0)
            {

                int Pid = Convert.ToInt32(Session["Parent_Id"]);
                //Get Childerns:
                Department Childs = db.Departments.Include(a=>a.ChildDepartment).SingleOrDefault(a=>a.Id==Pid);

                if (Childs.ChildDepartment.Any(a => a.Name.Equals(department.Name, StringComparison.OrdinalIgnoreCase)) == true)
                    return RedirectToAction("Index", new { Id = "CreateError" });



            }
            else
            {
                if (db.Departments.Any(a => a.Name.Equals(department.Name, StringComparison.OrdinalIgnoreCase)) == true)
                    return RedirectToAction("Index", new { Id = "CreateError" });

            }
            ////Duplicated Department name:
            //if (db.Departments.Any(a => a.Name.Equals(department.Name, StringComparison.OrdinalIgnoreCase)) == true)
            //    return RedirectToAction("Index", new { Id = "CreateError" });

            if (ModelState.IsValid)
            {


                string NotificationTime = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss");
                department.CreatedAt = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss");


                department.CreatedById = User.Identity.GetUserId();
                if (Convert.ToInt32(Session["Parent_Id"]) != 0)
                {
                    department.ParentId =Convert.ToInt32(Session["Parent_Id"]);
                }
                db.Departments.Add(department);
              
                
                db.SaveChanges();


                string UserId = User.Identity.GetUserId();
                Notification notification = null;
                List<ApplicationUser> Users = db.Users.Where(a => !a.Id.Equals(UserId) ).ToList();
                foreach(ApplicationUser user in Users)
                {

                    notification = new Notification() {

                        CreatedAt = NotificationTime,
                        Active = false,
                        UserId = user.Id,
                        Message = "تم إضافة قسم جديد: " + DepartmentListDisplay.CreateDepartmentDisplay(department.Id),
                        NotificationOwnerId= UserId
                    };
                    db.Notifications.Add(notification);
                }
                db.SaveChanges();
                return RedirectToAction("Index", new { Id = "CreateSuccess" });
            }


            return RedirectToAction("Index");

        }

   
        [AccessDeniedAuthorizeattribute(ActionName = "DepartmentsEdit")]
        public ActionResult Edit(int? id)
        {
            ViewBag.Current = "Department";



            if (id == null)
            {
                return RedirectToAction("BadRequestError", "ErrorController");

            }
            Department department = db.Departments.Find(id);
            if (department == null)
            {
                return RedirectToAction("HttpNotFoundError", "ErrorController");

            }



            return View(department);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        [AccessDeniedAuthorizeattribute(ActionName = "DepartmentsEdit")]
        public ActionResult Edit([Bind(Include = "Id,Name,CreatedAt,CreatedById,ParentId")] Department department)
        {
            ViewBag.Current = "Department";

            if(Convert.ToInt32(Session["Parent_Id"]) != 0)
            {
                int Pid = Convert.ToInt32(Session["Parent_Id"]);
                ////Get Childerns:
                Department Childs = db.Departments.Include(a => a.ChildDepartment).SingleOrDefault(a => a.Id == Pid);
               if( Childs.ChildDepartment.Where(a => a.Id != department.Id).Any(a => a.Name.Equals(department.Name, StringComparison.OrdinalIgnoreCase)) == true)
            

                return RedirectToAction("Index", new { Id = "EditError" });

            }

            else

            {
               if( db.Departments.Where(a=>a.Id!=department.Id).Any(a => a.Name.Equals(department.Name, StringComparison.OrdinalIgnoreCase)) == true)
            

                return RedirectToAction("Index", new { Id = "EditError" });
            }
            if (ModelState.IsValid)
            {
                string OldName = DepartmentListDisplay.CreateDepartmentDisplay(department.Id);
                department.UpdatedAt = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss");
                department.UpdatedById = User.Identity.GetUserId();
                db.Entry(department).State = EntityState.Modified;
              
                db.SaveChanges();
                string Newname= DepartmentListDisplay.CreateDepartmentDisplay(department.Id);
               string NotificationTime= DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss");


                string UserId = User.Identity.GetUserId();
                Notification notification = null;
                List<ApplicationUser> Users = db.Users.Where(a => !a.Id.Equals(UserId)).ToList();
                foreach (ApplicationUser user in Users)
                {

                    notification = new Notification()
                    {

                        CreatedAt = NotificationTime,
                        Active = false,
                        UserId = user.Id,
                        Message = "تم تعديل اسم القسم من: " + OldName+" إلى:"+Newname,
                        NotificationOwnerId = UserId
                    };
                    db.Notifications.Add(notification);
                }
                db.SaveChanges();
                return RedirectToAction("Index",new { Id = "EditSuccess" });
            }

            return RedirectToAction("Index");

        }

     
        [AccessDeniedAuthorizeattribute(ActionName = "DepartmentsDelete")]
        public ActionResult Delete(int? id)
        {
            ViewBag.Current = "Department";


            if (id == null)
            {
                return RedirectToAction("BadRequestError", "ErrorController");

            }
            Department department = db.Departments.Include(a => a.CreatedBy).SingleOrDefault(a=>a.Id==id);
            if (department == null)
            {
                return RedirectToAction("HttpNotFoundError", "ErrorController");

            }

            if (CheckDelete.CheckDepertmentDelete(id.Value)==false)
            {
                return RedirectToAction("Index", new { Id = "DeleteError" });

            }

            return View(department);
           
        }

        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]

    
        [AccessDeniedAuthorizeattribute(ActionName = "DepartmentsDelete")]
        public ActionResult Confirm(int? id)
        {
            ViewBag.Current = "Department";

            Department department = db.Departments.Find(id);

            string DepartmentName = DepartmentListDisplay.CreateDepartmentDisplay(department.Id);
            db.Departments.Remove(department);
            db.SaveChanges();
            string NotificationTime = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss");


            string UserId = User.Identity.GetUserId();
            Notification notification = null;
            List<ApplicationUser> Users = db.Users.Where(a => !a.Id.Equals(UserId)).ToList();
            foreach (ApplicationUser user in Users)
            {

                notification = new Notification()
                {

                    CreatedAt = NotificationTime,
                    Active = false,
                    UserId = user.Id,
                    Message = "تم حذف القسم : "+ DepartmentName,
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
