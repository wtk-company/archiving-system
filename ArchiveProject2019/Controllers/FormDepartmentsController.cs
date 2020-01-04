using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ArchiveProject2019.Models;
using ArchiveProject2019.HelperClasses;
using Microsoft.AspNet.Identity;

namespace ArchiveProject2019.Controllers
{
    public class FormDepartmentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();




        
        [AccessDeniedAuthorizeattribute(ActionName = "FormDepartmentsIndex")]
        public ActionResult Index(int? Id,string msg="none")
        {

            ViewBag.Current = "Forms";

            if (Id == null)
            {
               
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            }

            Form form = db.Forms.Find(Id);
            if(form==null)
            {
                return HttpNotFound();
            }


            if (!msg.Equals("none"))
            {
                ViewBag.Msg = msg;

            }
            else
            {
                ViewBag.Msg = null;
            }
            Session["Form_Id"] = Id;
            var formDepartments = db.FormDepartments.Where(a => a.FormId == Id).Include(f => f.CreatedBy).Include(f => f.Department).Include(f => f.Form);
            return View(formDepartments.OrderByDescending(a=>a.CreatedAt).ToList());
        }

        // GET: FormDepartments/Details/5

        
        [AccessDeniedAuthorizeattribute(ActionName = "FormDepartmentsDetails")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("BadRequestError", "ErrorController");

            }
            FormDepartment formDepartment = db.FormDepartments.Include(a => a.CreatedBy)
                .Include(a => a.Department).Include(a => a.Form).Include(a=>a.UpdatedBy)
                .SingleOrDefault(a=>a.Id==id);
            if (formDepartment == null)
            {
            return RedirectToAction("HttpNotFoundError", "ErrorController");

            }
            return View(formDepartment);
        }


        
        [AccessDeniedAuthorizeattribute(ActionName = "FormDepartmentsCreate")]
        public ActionResult Create(int ?Id)
        {
            ViewBag.Current = "Forms";

            if (Id == null)
            {

                return RedirectToAction("BadRequestError", "ErrorController");


            }

            Form form = db.Forms.Find(Id);
            if (form == null)
            {
                return RedirectToAction("HttpNotFoundError", "ErrorController");

            }


            Session["Form_Id"] = Id;


            int Form_Id = Convert.ToInt32(Session["Form_Id"]);

            List<int> Departments_Form = db.FormDepartments.Where(a => a.FormId == Form_Id).Select(a => a.DepartmentId).Distinct().ToList();

            //All Departments Expect current :
            List<int> DepartmentsId = db.Departments.Select(a => a.Id).Except(Departments_Form).ToList();
            ViewBag.FormIdValue = Form_Id;


            IEnumerable<DepartmentListDisplay> Departments = DepartmentListDisplay.CreateDepartmentListDisplay().Where(a => DepartmentsId.Contains(a.Id)).ToList();




            return View(Departments);
          
        }

    
        [HttpPost]

        
        [AccessDeniedAuthorizeattribute(ActionName = "FormDepartmentsCreate")]
        public ActionResult Create(int FormIdValue,List<int>Departments)
        {

            ViewBag.Current = "Forms";

            if (Departments == null)
            {
                return RedirectToAction("Index", new { @id = FormIdValue, @msg = "CreateError" });

            }
            if (ModelState.IsValid)
            {

                List<string> UsersId = new List<string>();
                string NotificationTime = string.Empty;
                string UserId = User.Identity.GetUserId();
                FormDepartment formDepartment = null;
                Notification notification = null;


                foreach (int i in Departments)
                {
                    NotificationTime = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss");

                    formDepartment = new FormDepartment() {

                        DepartmentId=i,
                        FormId= FormIdValue,
                        Is_Active=true,
                        CreatedAt = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss"),
                        CreatedById = this.User.Identity.GetUserId()

                    };
                    db.FormDepartments.Add(formDepartment);


                    List<ApplicationUser> Users = db.Users.Where(a =>a.DepartmentId==i).ToList();
                    foreach (ApplicationUser user in Users)
                    {

                        notification = new Notification()
                        {

                            CreatedAt = NotificationTime,
                            Active = false,
                            UserId = user.Id,
                            Message = "تم إضافة نموذج جديد إلى القسم، النموذج :"+db.Forms.Find(FormIdValue).Name,
                            NotificationOwnerId = UserId
                        };
                        db.Notifications.Add(notification);
                    }



                }
                db.SaveChanges();
                    return RedirectToAction("Index", new { @id = FormIdValue, @msg = "CreateSuccess" });
               
            }




            return RedirectToAction("Index");

        }

        
        [AccessDeniedAuthorizeattribute(ActionName = "FormDepartmentsEdit")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("BadRequestError", "ErrorController");

            }
            FormDepartment formDepartment = db.FormDepartments.Include(a => a.Department).Include(a => a.Form).SingleOrDefault(a => a.Id == id);
            if (formDepartment == null)
            {
                return RedirectToAction("HttpNotFoundError", "ErrorController");

            }


            return View(formDepartment);
        }

        [HttpPost]

        
        [AccessDeniedAuthorizeattribute(ActionName = "FormDepartmentsEdit")]
        public ActionResult Edit(int Id)
        {

            FormDepartment formDepartment = db.FormDepartments.Find(Id);
            if(formDepartment==null)
            {
                return RedirectToAction("HttpNotFoundError", "ErrorController");



            }

            List<string> UsersId = new List<string>();
            string NotificationTime = string.Empty;
            string UserId = User.Identity.GetUserId();
            string Message = string.Empty;
            string FormName = string.Empty;
            Notification notification = null;

            int Form_Id = formDepartment.FormId;
            formDepartment.Updatedat = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss");
            if(formDepartment.Is_Active==true)
            {
                formDepartment.Is_Active = false;
                Message = "تم إلغاء تفعيل النموذج في القسم، النموذج:";
            }
            else
            {
                formDepartment.Is_Active = true;
                Message = "تم  تفعيل النموذج في القسم، النموذج:";

            }
            NotificationTime = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss");
            formDepartment .UpdatedById= User.Identity.GetUserId();

            db.Entry(formDepartment).State = EntityState.Modified;
             db.SaveChanges();

            FormName = db.Forms.Find(formDepartment.FormId).Name;
            List<ApplicationUser> Users = db.Users.Where(a => a.DepartmentId == formDepartment.DepartmentId).ToList();
            foreach (ApplicationUser user in Users)
            {

                notification = new Notification()
                {

                    CreatedAt = NotificationTime,
                    Active = false,
                    UserId = user.Id,
                    Message = Message+" "+FormName,
                    NotificationOwnerId = UserId
                };
                db.Notifications.Add(notification);
            }


            db.SaveChanges();
            return RedirectToAction("Index", new { @id =Form_Id , @msg = "EditSuccess" });



        }


        
        [AccessDeniedAuthorizeattribute(ActionName = "FormDepartmentsDelete")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("BadRequestError", "ErrorController");

            }

            FormDepartment formDepartment = db.FormDepartments.Include(a => a.Form).Include(a => a.Department).SingleOrDefault(a=>a.Id==id);
            if (formDepartment == null)
            {
                return RedirectToAction("HttpNotFoundError", "ErrorController");

            }
            return View(formDepartment);
        }

       
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]


        
        [AccessDeniedAuthorizeattribute(ActionName = "FormDepartmentsDelete")]
        public ActionResult DeleteConfirmed(int id)
        {
            //Notification information:
            List<string> UsersId = new List<string>();
           string NotificationTime = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss");
            string UserId = User.Identity.GetUserId();
            string FormName = string.Empty;
            Notification notification = null;
          



            FormDepartment formDepartment = db.FormDepartments.Find(id);
            db.FormDepartments.Remove(formDepartment);
            int Form_id = formDepartment.FormId;
            FormName = db.Forms.Find(formDepartment.FormId).Name;
            int DepartmentId = formDepartment.DepartmentId;

            List<ApplicationUser> Users = db.Users.Where(a => a.DepartmentId == DepartmentId).ToList();
            foreach (ApplicationUser user in Users)
            {

                notification = new Notification()
                {

                    CreatedAt = NotificationTime,
                    Active = false,
                    UserId = user.Id,
                    Message = "تم إزالةالنموذج من القسم، النموذج: " + " " + FormName,
                    NotificationOwnerId = UserId
                };


                db.Notifications.Add(notification);
            }
            db.SaveChanges();


            //Remove Favorite user forms:
            List<string> UsersIdForDepartment = Users.Select(a => a.Id).ToList();
            List<FavouriteForms> favoriteForms = db.FavouriteForms.Where(a => a.FormId == Form_id && UsersIdForDepartment.Contains(a.UserId)).ToList();
            foreach(FavouriteForms f in favoriteForms)
            {
                db.FavouriteForms.Remove(f);

            }
            db.SaveChanges();
           

            return RedirectToAction("Index", new { @id = Form_id, @msg = "DeleteSuccess" });
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
