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

        //Get All Departments:
      //  [AccessDeniedAuthorizeattribute(ActionName ="Asmi")]
        //[AccessDeniedAuthorizeattribute(Roles = "xxx")]
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


            return View(departments.ToList());
        }


        //Details:

        public ActionResult Details(int ?id)
        {
            if(id==null)
            {
                return RedirectToAction("BadRequestError", "ErrorController");

            }

            Department department = db.Departments.Include(a => a.CreatedBy).SingleOrDefault(a=>a.Id==id);
            if(department==null)
            {
                return RedirectToAction("HttpNotFoundError", "ErrorController");

            }


            return View(department);
        }


  


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
       
        //Create Department:
        public ActionResult Create(int id,int msg)
        {
            ViewBag.Current = "Department";
            Session["Parent_Id"] = msg;




            return View();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
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

               
                department.CreatedAt = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss");


                department.CreatedById = User.Identity.GetUserId();
                if (Convert.ToInt32(Session["Parent_Id"]) != 0)
                {
                    department.ParentId =Convert.ToInt32(Session["Parent_Id"]);
                }
                db.Departments.Add(department);
              
                
                db.SaveChanges();
                return RedirectToAction("Index", new { Id = "CreateSuccess" });
            }


            return RedirectToAction("Index");

        }

        // Edit Department:
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
                db.Entry(department).State = EntityState.Modified;
                
                department.UpdatedAt= DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss");
                db.SaveChanges();
                return RedirectToAction("Index",new { Id = "EditSuccess" });
            }

            return RedirectToAction("Index");

        }

        //Delete Department:

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
        public ActionResult Confirm(int? id)
        {
            ViewBag.Current = "Department";

            Department department = db.Departments.Find(id);
          

            db.Departments.Remove(department);
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
