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


        [Authorize]
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

        [Authorize]
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
        [Authorize]
        [AccessDeniedAuthorizeattribute(ActionName = "DocumentDepartmentsCreate")]
        //[ValidateAntiForgeryToken]
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


        [Authorize]
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
        [Authorize]
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


        [Authorize]
        //  [AccessDeniedAuthorizeattribute(ActionName = "DocumentDepartmentsDelete")]
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
        [Authorize]
       // [AccessDeniedAuthorizeattribute(ActionName = "DocumentDepartmentsDelete")]
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




        [Authorize]
        //  [AccessDeniedAuthorizeattribute(ActionName = "DocumentDepartmentsDelete")]
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