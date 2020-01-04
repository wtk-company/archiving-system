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
    public class DocumentDepartmentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        [AccessDeniedAuthorizeattribute(ActionName = "DocumentDepartmentsIndex")]
        public ActionResult Index(int? Id, string msg = "none")
        {

            ViewBag.Current = "Document";


            if (Id == null)
            {

                return RedirectToAction("BadRequestError", "ErrorController");


            }

            Document document = db.Documents.Find(Id);
            if (document == null)
            {
                return RedirectToAction("HttpNotFoundError", "ErrorController");

            }


            if (!msg.Equals("none"))
            {
                ViewBag.Msg = msg;

            }
            else
            {
                ViewBag.Msg = null;
            }
            Session["document_Id"] = Id;
            var DocumentDepartments = db.DocumentDepartments.Where(a => a.DocumentId == Id).Include(a => a.CreatedBy).Include(a => a.Department).Include(a => a.document).ToList();
            return View(DocumentDepartments.OrderByDescending(a => a.CreatedAt).ToList());
        }

   
        [AccessDeniedAuthorizeattribute(ActionName = "DocumentDepartmentsCreate")]

        public ActionResult Create(int? Id)
        {
            ViewBag.Current = "Document";


            if (Id == null)
            {

                return RedirectToAction("BadRequestError", "ErrorController");


            }

            Document document = db.Documents.Find(Id);
            if (document == null)
            {
                return RedirectToAction("HttpNotFoundError", "ErrorController");

            }


            Session["Document_Id"] = Id;




            List<int> Departments_Document = db.DocumentDepartments.Where(a => a.DocumentId == Id.Value).Select(a => a.DepartmentId).Distinct().ToList();

            //All Departments Expect current :
            List<int> DepartmentsId = db.Departments.Select(a => a.Id).Except(Departments_Document).ToList();
            ViewBag.DocumentIdValue = Id.Value;


            IEnumerable<DepartmentListDisplay> Departments = DepartmentListDisplay.CreateDepartmentListDisplay().Where(a => DepartmentsId.Contains(a.Id)).ToList();




            return View(Departments);

        }



        [HttpPost]
  
        [AccessDeniedAuthorizeattribute(ActionName = "DocumentDepartmentsCreate")]
     
        public ActionResult Create(int DocumentIdValue, List<int> Departments)
        {

            ViewBag.Current = "Document";

            List<string> UsersId = new List<string>();
            string NotificationTime = string.Empty;
            string UserId = User.Identity.GetUserId();
            Document doc = db.Documents.Find(DocumentIdValue);

            if (Departments == null)
            {
                return RedirectToAction("Index", new { @id = DocumentIdValue, @msg = "CreateError" });

            }
            if (ModelState.IsValid)
            {

                DocumentDepartment documentDepartment = null;
                foreach (int i in Departments)
                {
                    documentDepartment = new DocumentDepartment()
                    {

                        DepartmentId = i,
                        DocumentId = DocumentIdValue,

                        EnableEdit = true,
                        EnableRelate=true,
                        EnableSeal=true,
                        EnableReplay=true,
                        CreatedAt = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss"),
                        CreatedById = this.User.Identity.GetUserId()

                    };
                    db.DocumentDepartments.Add(documentDepartment);
                    NotificationTime = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss");
                    UsersId = db.Users.Where(a => a.DepartmentId == i).Select(a => a.Id).ToList();

                    Notification notification = null;

                    List<ApplicationUser> Users = db.Users.Where(a => UsersId.Contains(a.Id)).ToList();
                    foreach (ApplicationUser user in Users)
                    {

                        notification = new Notification()
                        {

                            CreatedAt = NotificationTime,
                            Active = false,
                            UserId = user.Id,
                            Message = "تم إضافة وثيقة جديدة للقسم الحالي، رقم الوثيقة :" + doc.DocumentNumber + " موضوع الوثيقة :" + doc.Subject
                            + " ،عنوان الوثيقة :" + doc.Address + "،وصف الوثيقة :" + doc.Description
                           ,
                            NotificationOwnerId = UserId
                        };
                        db.Notifications.Add(notification);
                    }





                }

                db.SaveChanges();
                return RedirectToAction("Index", new { @id = DocumentIdValue, @msg = "CreateSuccess" });

            }




            return RedirectToAction("Index", new { @id = DocumentIdValue });
        }


       
        [AccessDeniedAuthorizeattribute(ActionName = "DocumentDepartmentsDelete")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("BadRequestError", "ErrorController");

            }
            DocumentDepartment documentDepartment = db.DocumentDepartments.Include(a => a.document).Include(a => a.Department).SingleOrDefault(a => a.Id == id);
            if (documentDepartment == null)
            {
                return RedirectToAction("HttpNotFoundError", "ErrorController");

            }
            return View(documentDepartment);
        }

        // POST: FormDepartments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
   
        [AccessDeniedAuthorizeattribute(ActionName = "DocumentDepartmentsDelete")]
        public ActionResult DeleteConfirmed(int id)
        {
            List<string> UsersId = new List<string>();
            string NotificationTime = string.Empty;
            string UserId = User.Identity.GetUserId();
            Document doc = db.DocumentDepartments.Include(a => a.document).SingleOrDefault(a => a.Id == id).document;

            DocumentDepartment documentDepartment = db.DocumentDepartments.Find(id);
            db.DocumentDepartments.Remove(documentDepartment);

            int Form_id = documentDepartment.DocumentId;
            db.SaveChanges();

            UsersId = db.Users.Where(a => a.DepartmentId == documentDepartment.DepartmentId).Select(a => a.Id).ToList();

            Notification notification = null;
            NotificationTime = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss");

            List<ApplicationUser> Users = db.Users.Where(a => UsersId.Contains(a.Id)).ToList();
            foreach (ApplicationUser user in Users)
            {

                notification = new Notification()
                {

                    CreatedAt = NotificationTime,
                    Active = false,
                    UserId = user.Id,
                    Message = "تم  إزالة وثيقة من  القسم الحالي، رقم الوثيقة :" + doc.DocumentNumber + " موضوع الوثيقة :" + doc.Subject
                    + " ،عنوان الوثيقة :" + doc.Address + "،وصف الوثيقة :" + doc.Description
                   ,
                    NotificationOwnerId = UserId
                };
                db.Notifications.Add(notification);
            }
            db.SaveChanges();


            return RedirectToAction("Index", new { @id = Form_id, @msg = "DeleteSuccess" });
        }


       
        [AccessDeniedAuthorizeattribute(ActionName = "DocumentDepartmentsActiveNonActiveEdit")]

        public ActionResult ActiveNOnActive(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("BadRequestError", "ErrorController");

            }
            DocumentDepartment documentDepartment = db.DocumentDepartments.Include(a => a.document).Include(a => a.Department).SingleOrDefault(a => a.Id == id);
            if (documentDepartment == null)
            {
                return RedirectToAction("HttpNotFoundError", "ErrorController");

            }
            return View(documentDepartment);
        }


        [HttpPost, ActionName("ActiveNOnActive")]
        [ValidateAntiForgeryToken]
    
       [AccessDeniedAuthorizeattribute(ActionName = "DocumentDepartmentsActiveNonActiveEdit")]
        public ActionResult ActiveNOnActiveConfirm (int id)
        {
            string ActiveMode = string.Empty;
            List<string> UsersId = new List<string>();
            string NotificationTime = string.Empty;
            string UserId = User.Identity.GetUserId();
            Document doc = db.DocumentDepartments.Include(a => a.document).SingleOrDefault(a => a.Id == id).document;

            DocumentDepartment documentDepartment = db.DocumentDepartments.Find(id);
            if(documentDepartment.EnableEdit==true)
            {
                documentDepartment.EnableEdit = false;
                ActiveMode = "الغاء  التعديل";
            }
            else
            {
                documentDepartment.EnableEdit = true;
                ActiveMode = "تفعيل التعديل";

            }
            documentDepartment.UpdatedAt= DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss");
            db.Entry(documentDepartment).State = EntityState.Modified;

            int Form_id = documentDepartment.DocumentId;
            db.SaveChanges();

            UsersId = db.Users.Where(a => a.DepartmentId == documentDepartment.DepartmentId).Select(a => a.Id).ToList();

            Notification notification = null;
            NotificationTime = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss");

            List<ApplicationUser> Users = db.Users.Where(a => UsersId.Contains(a.Id)).ToList();
            foreach (ApplicationUser user in Users)
            {

                notification = new Notification()
                {

                    CreatedAt = NotificationTime,
                    Active = false,
                    UserId = user.Id,
                    Message = "تمت عملية  "+ActiveMode+" في القسم الحالي ، رقم الوثيقة"+"" + doc.DocumentNumber + " موضوع الوثيقة :" + doc.Subject
                    + " ،عنوان الوثيقة :" + doc.Address + "،وصف الوثيقة :" + doc.Description
                   ,
                    NotificationOwnerId = UserId
                };
                db.Notifications.Add(notification);
            }
            db.SaveChanges();


            return RedirectToAction("Index", new { @id = Form_id, @msg = "EditSuccess" });
        }




        [AccessDeniedAuthorizeattribute(ActionName = "DocumentDepartmentsActiveNonActiveReplay")]

        public ActionResult ActiveNOnActiveReplay(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("BadRequestError", "ErrorController");

            }
            DocumentDepartment documentDepartment = db.DocumentDepartments.Include(a => a.document).Include(a => a.Department).SingleOrDefault(a => a.Id == id);
            if (documentDepartment == null)
            {
                return RedirectToAction("HttpNotFoundError", "ErrorController");

            }
            return View(documentDepartment);
        }


        [HttpPost, ActionName("ActiveNOnActiveReplay")]
        [ValidateAntiForgeryToken]
      
        [AccessDeniedAuthorizeattribute(ActionName = "DocumentDepartmentsActiveNonActiveReplay")]

        public ActionResult ActiveNOnActiveConfirmReplay(int id)
        {
            string ActiveMode = string.Empty;
            List<string> UsersId = new List<string>();
            string NotificationTime = string.Empty;
            string UserId = User.Identity.GetUserId();
            Document doc = db.DocumentDepartments.Include(a => a.document).SingleOrDefault(a => a.Id == id).document;

            DocumentDepartment documentDepartment = db.DocumentDepartments.Find(id);
            if (documentDepartment.EnableReplay == true)
            {
                documentDepartment.EnableReplay = false;
                ActiveMode = "الغاء  إمكانية الرد";
            }
            else
            {
                documentDepartment.EnableReplay = true;
                ActiveMode = "تفعيل إمكانية الرد";

            }
            documentDepartment.UpdatedAt = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss");
            db.Entry(documentDepartment).State = EntityState.Modified;

            int Form_id = documentDepartment.DocumentId;
            db.SaveChanges();

            UsersId = db.Users.Where(a => a.DepartmentId == documentDepartment.DepartmentId).Select(a => a.Id).ToList();

            Notification notification = null;
            NotificationTime = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss");

            List<ApplicationUser> Users = db.Users.Where(a => UsersId.Contains(a.Id)).ToList();
            foreach (ApplicationUser user in Users)
            {

                notification = new Notification()
                {

                    CreatedAt = NotificationTime,
                    Active = false,
                    UserId = user.Id,
                    Message = "تمت عملية  " + ActiveMode + " في القسم الحالي ، رقم الوثيقة" + "" + doc.DocumentNumber + " موضوع الوثيقة :" + doc.Subject
                    + " ،عنوان الوثيقة :" + doc.Address + "،وصف الوثيقة :" + doc.Description
                   ,
                    NotificationOwnerId = UserId
                };
                db.Notifications.Add(notification);
            }
            db.SaveChanges();


            return RedirectToAction("Index", new { @id = Form_id, @msg = "EditSuccess" });
        }




  
        [AccessDeniedAuthorizeattribute(ActionName = "DocumentDepartmentsActiveNonActiveRelate")]

        public ActionResult ActiveNOnActiveRelate(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("BadRequestError", "ErrorController");

            }
            DocumentDepartment documentDepartment = db.DocumentDepartments.Include(a => a.document).Include(a => a.Department).SingleOrDefault(a => a.Id == id);
            if (documentDepartment == null)
            {
                return RedirectToAction("HttpNotFoundError", "ErrorController");

            }
            return View(documentDepartment);
        }


        [HttpPost, ActionName("ActiveNOnActiveRelate")]
        [ValidateAntiForgeryToken]
     
        [AccessDeniedAuthorizeattribute(ActionName = "DocumentDepartmentsActiveNonActiveRelate")]

        public ActionResult ActiveNOnActiveConfirmRelate(int id)
        {
            string ActiveMode = string.Empty;
            List<string> UsersId = new List<string>();
            string NotificationTime = string.Empty;
            string UserId = User.Identity.GetUserId();
            Document doc = db.DocumentDepartments.Include(a => a.document).SingleOrDefault(a => a.Id == id).document;

            DocumentDepartment documentDepartment = db.DocumentDepartments.Find(id);
            if (documentDepartment.EnableRelate == true)
            {
                documentDepartment.EnableRelate = false;
                ActiveMode = "الغاء  إمكانية الربط";
            }
            else
            {
                documentDepartment.EnableRelate = true;
                ActiveMode = "تفعيل إمكانية الربط";

            }
            documentDepartment.UpdatedAt = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss");
            db.Entry(documentDepartment).State = EntityState.Modified;

            int Form_id = documentDepartment.DocumentId;
            db.SaveChanges();

            UsersId = db.Users.Where(a => a.DepartmentId == documentDepartment.DepartmentId).Select(a => a.Id).ToList();

            Notification notification = null;
            NotificationTime = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss");

            List<ApplicationUser> Users = db.Users.Where(a => UsersId.Contains(a.Id)).ToList();
            foreach (ApplicationUser user in Users)
            {

                notification = new Notification()
                {

                    CreatedAt = NotificationTime,
                    Active = false,
                    UserId = user.Id,
                    Message = "تمت عملية  " + ActiveMode + " في القسم الحالي ، رقم الوثيقة" + "" + doc.DocumentNumber + " موضوع الوثيقة :" + doc.Subject
                    + " ،عنوان الوثيقة :" + doc.Address + "،وصف الوثيقة :" + doc.Description
                   ,
                    NotificationOwnerId = UserId
                };
                db.Notifications.Add(notification);
            }
            db.SaveChanges();


            return RedirectToAction("Index", new { @id = Form_id, @msg = "EditSuccess" });
        }





        [AccessDeniedAuthorizeattribute(ActionName = "DocumentDepartmentsActiveNonActiveSeal")]

        public ActionResult ActiveNOnActiveSeal(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("BadRequestError", "ErrorController");

            }
            DocumentDepartment documentDepartment = db.DocumentDepartments.Include(a => a.document).Include(a => a.Department).SingleOrDefault(a => a.Id == id);
            if (documentDepartment == null)
            {
                return RedirectToAction("HttpNotFoundError", "ErrorController");

            }
            return View(documentDepartment);
        }


        [HttpPost, ActionName("ActiveNOnActiveSeal")]
        [ValidateAntiForgeryToken]
       
        [AccessDeniedAuthorizeattribute(ActionName = "DocumentDepartmentsActiveNonActiveSeal")]

        public ActionResult ActiveNOnActiveConfirmSeal(int id)
        {
            string ActiveMode = string.Empty;
            List<string> UsersId = new List<string>();
            string NotificationTime = string.Empty;
            string UserId = User.Identity.GetUserId();
            Document doc = db.DocumentDepartments.Include(a => a.document).SingleOrDefault(a => a.Id == id).document;

            DocumentDepartment documentDepartment = db.DocumentDepartments.Find(id);
            if (documentDepartment.EnableSeal == true)
            {
                documentDepartment.EnableSeal = false;
                ActiveMode = "الغاء  إمكانية التسديد";
            }
            else
            {
                documentDepartment.EnableSeal = true;
                ActiveMode = "تفعيل إمكانية التسديد";

            }
            documentDepartment.UpdatedAt = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss");
            db.Entry(documentDepartment).State = EntityState.Modified;

            int Form_id = documentDepartment.DocumentId;
            db.SaveChanges();

            UsersId = db.Users.Where(a => a.DepartmentId == documentDepartment.DepartmentId).Select(a => a.Id).ToList();

            Notification notification = null;
            NotificationTime = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss");

            List<ApplicationUser> Users = db.Users.Where(a => UsersId.Contains(a.Id)).ToList();
            foreach (ApplicationUser user in Users)
            {

                notification = new Notification()
                {

                    CreatedAt = NotificationTime,
                    Active = false,
                    UserId = user.Id,
                    Message = "تمت عملية  " + ActiveMode + " في القسم الحالي ، رقم الوثيقة" + "" + doc.DocumentNumber + " موضوع الوثيقة :" + doc.Subject
                    + " ،عنوان الوثيقة :" + doc.Address + "،وصف الوثيقة :" + doc.Description
                   ,
                    NotificationOwnerId = UserId
                };
                db.Notifications.Add(notification);
            }
            db.SaveChanges();


            return RedirectToAction("Index", new { @id = Form_id, @msg = "EditSuccess" });
        }



        

     
          [AccessDeniedAuthorizeattribute(ActionName = "DocumentDepartmentsDetails")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("BadRequestError", "ErrorController");

            }
            DocumentDepartment documentDepartment = db.DocumentDepartments.Include(a=>a.CreatedBy).Include(a => a.document).Include(a => a.Department).SingleOrDefault(a => a.Id == id);
            if (documentDepartment == null)
            {
                return RedirectToAction("HttpNotFoundError", "ErrorController");

            }
            return View(documentDepartment);
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