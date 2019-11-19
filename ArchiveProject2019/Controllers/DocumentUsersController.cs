﻿using ArchiveProject2019.Models;
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
    public class DocumentUsersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        [Authorize]
       // [AccessDeniedAuthorizeattribute(ActionName = "DocumentGroupsIndex")]
        public ActionResult Index(int? Id, string msg = "none")
        {

            ViewBag.Current = "Document";

            if (Id == null)
            {

                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            }

            Document document = db.Documents.Find(Id);
            if (document == null)
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
            Session["Document_Id"] = Id;
            var documentUsers = db.DocumentUsers.Where(a => a.DocumentId == Id).Include(f => f.document).Include(a=>a.CreatedBy).Include(a=>a.User);
            return View(documentUsers.OrderByDescending(a => a.CreatedAt).ToList());
        }






        [Authorize]
      //  [AccessDeniedAuthorizeattribute(ActionName = "DocumentDepartmentsCreate")]

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




            List<string> Departments_Users = db.DocumentUsers.Where(a => a.DocumentId == Id.Value).Select(a => a.UserId).Distinct().ToList();

            //All Departments Expect current :
            List<string> UsersId = db.Users.Select(a => a.Id).Except(Departments_Users).ToList();
            ViewBag.DocumentIdValue = Id.Value;


            IEnumerable<ApplicationUser> Users = db.Users.Where(a =>! a.RoleName.Equals("Master") && UsersId.Contains(a.Id));




            return View(Users);

        }


        [HttpPost]
        [Authorize]
   //     [AccessDeniedAuthorizeattribute(ActionName = "DocumentGroupsCreate")]
        public ActionResult Create(int DocumentIdValue, List<string> Users)
        {

            ViewBag.Current = "Document";

            string NotificationTime = string.Empty;
            string UserId = User.Identity.GetUserId();
            Document doc = db.Documents.Find(DocumentIdValue);


            if (Users == null)
            {
                return RedirectToAction("Index", new { @id = DocumentIdValue, @msg = "CreateError" });

            }
            if (ModelState.IsValid)
            {

                DocumentUser documentUser = null;
                foreach (string i in Users)
                {
                    documentUser = new DocumentUser()
                    {

                        UserId = i,
                        DocumentId = DocumentIdValue,
                        EnableEdit=true,
                        CreatedAt = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss"),
                        CreatedById = this.User.Identity.GetUserId()

                    };
                    db.DocumentUsers.Add(documentUser);

                    NotificationTime = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss");
                    Notification notification = null;

                   

                        notification = new Notification()
                        {

                            CreatedAt = NotificationTime,
                            Active = false,
                            UserId = i,
                            Message = "تم إضافة وثيقة جديدة  :" +""+"، رقم الوثيقة :" + doc.Name + " ، موضوع الوثيقة:" + doc.Subject +
                            " ، عنوان الوثيقة:" + doc.Address + " ،وصف الوثيقة :" + doc.Description
                           ,
                            NotificationOwnerId = UserId
                        };
                        db.Notifications.Add(notification);
              




                }
                    db.SaveChanges();
                return RedirectToAction("Index", new { @id = DocumentIdValue, @msg = "CreateSuccess" });

            }



            return RedirectToAction("Index", new { @id = DocumentIdValue });

        }

        [Authorize]
       // [AccessDeniedAuthorizeattribute(ActionName = "DocumentDepartmentsDelete")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("BadRequestError", "ErrorController");

            }
            DocumentUser documentUser = db.DocumentUsers.Include(a => a.document).Include(a => a.User).SingleOrDefault(a => a.Id == id);
            if (documentUser == null)
            {
                return RedirectToAction("HttpNotFoundError", "ErrorController");

            }
            return View(documentUser);
        }

        // POST: FormDepartments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
       // [AccessDeniedAuthorizeattribute(ActionName = "DocumentDepartmentsDelete")]
        public ActionResult DeleteConfirmed(int id)
        {
           
            string NotificationTime = string.Empty;
            string UserId = User.Identity.GetUserId();
            Document doc = db.DocumentUsers.Include(a => a.document).SingleOrDefault(a => a.Id == id).document;
            string User_Id = db.DocumentUsers.Include(a => a.document).SingleOrDefault(a => a.Id == id).UserId;
            DocumentUser documentuser = db.DocumentUsers.Find(id);
            int Form_id = documentuser.DocumentId;
            db.DocumentUsers.Remove(documentuser);

            db.SaveChanges();


            Notification notification = null;
            NotificationTime = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss");

         
                notification = new Notification()
                {

                    CreatedAt = NotificationTime,
                    Active = false,
                    UserId = User_Id,
                    Message = "تم  إزالة وثيقة    ، رقم الوثيقة :" + doc.DocumentNumber + " موضوع الوثيقة :" + doc.Subject
                    + " ،عنوان الوثيقة :" + doc.Address + "،وصف الوثيقة :" + doc.Description
                   ,
                    NotificationOwnerId = UserId
                };
                db.Notifications.Add(notification);
      
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
            DocumentUser documentUser = db.DocumentUsers.Include(a => a.document).Include(a => a.User).SingleOrDefault(a => a.Id == id);
            if (documentUser == null)
            {
                return RedirectToAction("HttpNotFoundError", "ErrorController");

            }
            return View(documentUser);
        }


        [HttpPost, ActionName("ActiveNOnActive")]
        [ValidateAntiForgeryToken]
        [Authorize]
        // [AccessDeniedAuthorizeattribute(ActionName = "DocumentDepartmentsDelete")]
        public ActionResult ActiveNOnActiveConfirm(int id)
        {
            string ActiveMode = string.Empty;
           
            string NotificationTime = string.Empty;
            string UserId = User.Identity.GetUserId();
            Document doc = db.DocumentUsers.Include(a => a.document).SingleOrDefault(a => a.Id == id).document;

            DocumentUser documentUser = db.DocumentUsers.Find(id);
            if (documentUser.EnableEdit == true)
            {
                documentUser.EnableEdit = false;
                ActiveMode = "الغاء  التعديل";
            }
            else
            {
                documentUser.EnableEdit = true;
                ActiveMode = "تفعيل التعديل";

            }
            documentUser.UpdatedAt = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss");
            db.Entry(documentUser).State = EntityState.Modified;

            int Form_id = documentUser.DocumentId;
            db.SaveChanges();


            Notification notification = null;
            NotificationTime = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss");


                notification = new Notification()
                {

                    CreatedAt = NotificationTime,
                    Active = false,
                    UserId = documentUser.UserId,
                    Message = "تمت عملية  " + ActiveMode + "   للوثيقة  ، رقم الوثيقة" + "" + doc.DocumentNumber + " موضوع الوثيقة :" + doc.Subject
                    + " ،عنوان الوثيقة :" + doc.Address + "،وصف الوثيقة :" + doc.Description
                   ,
                    NotificationOwnerId = UserId
                };
                db.Notifications.Add(notification);
           
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
            DocumentUser documentUser = db.DocumentUsers.Include(a => a.CreatedBy).Include(a => a.document).Include(a => a.User).SingleOrDefault(a => a.Id == id);
            if (documentUser == null)
            {
                return RedirectToAction("HttpNotFoundError", "ErrorController");

            }
            return View(documentUser);
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