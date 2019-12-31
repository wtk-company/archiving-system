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
    public class DocumentGroupsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        
        [AccessDeniedAuthorizeattribute(ActionName = "DocumentGroupsIndex")]
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
            var documentGroups = db.DocumentGroups.Where(a => a.DocumentId == Id).Include(f => f.CreatedBy).Include(f => f.Group).Include(f => f.document);
            return View(documentGroups.OrderByDescending(a=>a.CreatedAt).ToList());
        }

        
        [AccessDeniedAuthorizeattribute(ActionName = "DocumentGroupsCreate")]
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


           

            List<int> Group_form = db.DocumentGroups.Where(a => a.DocumentId == Id.Value).Select(a => a.GroupId).Distinct().ToList();

            //All Departments Expect current :
            List<int> GroupsId = db.Groups.Select(a => a.Id).Except(Group_form).ToList();
            ViewBag.DocumentIdValue = Id.Value;


            IEnumerable<Group> Groups = db.Groups.Where(a => GroupsId.Contains(a.Id)).ToList();




            return View(Groups);

        }



       [HttpPost]
        
        [AccessDeniedAuthorizeattribute(ActionName = "DocumentGroupsCreate")]
        public ActionResult Create(int DocumentIdValue, List<int> Groups)
        {

            ViewBag.Current = "Document";

            List<string> UsersId = new List<string>();
            string NotificationTime = string.Empty;
            string UserId = User.Identity.GetUserId();
            Document doc = db.Documents.Find(DocumentIdValue);


            if (Groups == null)
            {
                return RedirectToAction("Index", new { @id = DocumentIdValue, @msg = "CreateError" });

            }
            if (ModelState.IsValid)
            {

                DocumentGroup documentGroup = null;
                foreach (int i in Groups)
                {
                    documentGroup = new DocumentGroup()
                    {

                        GroupId = i,
                        DocumentId = DocumentIdValue,
                      EnableEdit=true,
                      EnableReplay=true,
                      EnableSeal=true,
                      EnableRelate=true,
                        CreatedAt = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss"),
                        CreatedById = this.User.Identity.GetUserId()

                    };
                    db.DocumentGroups.Add(documentGroup);

                    NotificationTime = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss");
                    UsersId = db.UsersGroups.Where(a => a.GroupId == i).Select(a => a.UserId).ToList();
                    string GroupName = db.Groups.Find(i).Name;
                    Notification notification = null;

                    List<ApplicationUser> Users = db.Users.Where(a => UsersId.Contains(a.Id)).ToList();
                    foreach (ApplicationUser user in Users)
                    {

                        notification = new Notification()
                        {

                            CreatedAt = NotificationTime,
                            Active = false,
                            UserId = user.Id,
                            Message = "تم إضافة وثيقة جديدة للمجموعة :"+GroupName+"، رقم الوثيقة :"+doc.Name+" ، موضوع الوثيقة:"+doc.Subject+
                            " ، عنوان الوثيقة:"+doc.Address+" ،وصف الوثيقة :"+doc.Description
                           ,
                            NotificationOwnerId = UserId
                        };
                        db.Notifications.Add(notification);
                    }

                    db.SaveChanges();



                }
                return RedirectToAction("Index", new { @id = DocumentIdValue, @msg = "CreateSuccess" });

            }



            return RedirectToAction("Index", new { @id = DocumentIdValue});

        }

        
        [AccessDeniedAuthorizeattribute(ActionName = "DocumentGroupsDelete")]
        public ActionResult Delete(int? id)
        {
            ViewBag.Current = "Document";

            if (id == null)
            {
                return RedirectToAction("BadRequestError", "ErrorController");
            }
            DocumentGroup documentGroup = db.DocumentGroups.Include(a => a.document).Include(a=>a.CreatedBy).Include(a => a.Group).SingleOrDefault(a => a.Id == id);
            if (documentGroup == null)
            {
                return RedirectToAction("HttpNotFoundError", "ErrorController");

            }
            return View(documentGroup);
        }

        // POST: FormDepartments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        
        [AccessDeniedAuthorizeattribute(ActionName = "DocumentGroupsDelete")]
        public ActionResult DeleteConfirmed(int id)
        {
            ViewBag.Current = "Document";

            List<string> UsersId = new List<string>();
            string NotificationTime = string.Empty;
            string UserId = User.Identity.GetUserId();
           
            DocumentGroup documentGroup = db.DocumentGroups.Find(id);
            db.DocumentGroups.Remove(documentGroup);
            int Document_Id = documentGroup.DocumentId;
            db.SaveChanges();
            Document doc = db.Documents.Find(documentGroup.DocumentId);
            NotificationTime = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss");
            UsersId = db.UsersGroups.Where(a => a.GroupId == documentGroup.GroupId).Select(a => a.UserId).ToList();
            string GroupName = db.Groups.Find(documentGroup.GroupId).Name;
            Notification notification = null;

            List<ApplicationUser> Users = db.Users.Where(a => UsersId.Contains(a.Id)).ToList();
            foreach (ApplicationUser user in Users)
            {

                notification = new Notification()
                {

                    CreatedAt = NotificationTime,
                    Active = false,
                    UserId = user.Id,
                    Message = "تم حذف وثيقة من المجموعة :" + GroupName + "، رقم الوثيقة :" + doc.Name + " ، موضوع الوثيقة:" + doc.Subject +
                    " ، عنوان الوثيقة:" + doc.Address + " ،وصف الوثيقة :" + doc.Description
                   ,
                    NotificationOwnerId = UserId
                };
                db.Notifications.Add(notification);
            }

            db.SaveChanges();

            return RedirectToAction("Index", new { @id = Document_Id, @msg = "DeleteSuccess" });
        }




        
        [AccessDeniedAuthorizeattribute(ActionName = "DocumentGroupsActiveNonActiveEdit")]
        public ActionResult ActiveNOnActive(int? id)
        {
            ViewBag.Current = "Document";

            if (id == null)
            {
                return RedirectToAction("BadRequestError", "ErrorController");

            }
            DocumentGroup documentGroup = db.DocumentGroups.Include(a => a.document).Include(a => a.Group).SingleOrDefault(a => a.Id == id);
            if (documentGroup == null)
            {
                return RedirectToAction("HttpNotFoundError", "ErrorController");

            }
            return View(documentGroup);
        }



        [HttpPost, ActionName("ActiveNOnActive")]
        [ValidateAntiForgeryToken]
        
        [AccessDeniedAuthorizeattribute(ActionName = "DocumentGroupsActiveNonActiveEdit")]

        public ActionResult ActiveNOnActiveConfirm(int id)
        {
            ViewBag.Current = "Document";

            string ActiveMode = string.Empty;
            List<string> UsersId = new List<string>();
            string NotificationTime = string.Empty;
            string UserId = User.Identity.GetUserId();
            Document doc = db.DocumentGroups.Include(a => a.document).SingleOrDefault(a => a.Id == id).document;

            DocumentGroup documentGroup = db.DocumentGroups.Find(id);
            if (documentGroup.EnableEdit == true)
            {
                documentGroup.EnableEdit = false;
                ActiveMode = "الغاء  التعديل";
            }
            else
            {
                documentGroup.EnableEdit = true;
                ActiveMode = "تفعيل التعديل";

            }
            documentGroup.UpdatedAt = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss");
            db.Entry(documentGroup).State = EntityState.Modified;

         
            db.SaveChanges();
            int Form_id = documentGroup.DocumentId;

            UsersId = db.UsersGroups.Where(a => a.GroupId == documentGroup.GroupId).Select(a => a.UserId).ToList();
            string GroupName = db.Groups.Find(documentGroup.GroupId).Name;
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
                    Message = "تمت عملية  " + ActiveMode +" في المجموعة "+GroupName+" رقم الوثيقة :" + doc.DocumentNumber + " موضوع الوثيقة :" + doc.Subject
                    + " ،عنوان الوثيقة :" + doc.Address + "،وصف الوثيقة :" + doc.Description
                   ,
                    NotificationOwnerId = UserId
                };
                db.Notifications.Add(notification);
            }
            db.SaveChanges();


            return RedirectToAction("Index", new { @id = Form_id, @msg = "EditSuccess" });
        }






        
        [AccessDeniedAuthorizeattribute(ActionName = "DocumentGroupsActiveNonActiveReplay")]

        public ActionResult ActiveNOnActiveReplay(int? id)
        {
            ViewBag.Current = "Document";

            if (id == null)
            {
                return RedirectToAction("BadRequestError", "ErrorController");

            }
            DocumentGroup documentGroup = db.DocumentGroups.Include(a => a.document).Include(a => a.Group).SingleOrDefault(a => a.Id == id);
            if (documentGroup == null)
            {
                return RedirectToAction("HttpNotFoundError", "ErrorController");

            }
            return View(documentGroup);
        }



        [HttpPost, ActionName("ActiveNOnActiveReplay")]
        [ValidateAntiForgeryToken]
        
        [AccessDeniedAuthorizeattribute(ActionName = "DocumentGroupsActiveNonActiveReplay")]

        public ActionResult ActiveNOnActiveConfirmReplay(int id)
        {
            ViewBag.Current = "Document";

            string ActiveMode = string.Empty;
            List<string> UsersId = new List<string>();
            string NotificationTime = string.Empty;
            string UserId = User.Identity.GetUserId();
            Document doc = db.DocumentGroups.Include(a => a.document).SingleOrDefault(a => a.Id == id).document;

            DocumentGroup documentGroup = db.DocumentGroups.Find(id);
            if (documentGroup.EnableReplay == true)
            {
                documentGroup.EnableReplay = false;
                ActiveMode = "الغاء  إمكانية الرد";
            }
            else
            {
                documentGroup.EnableReplay = true;
                ActiveMode = "تفعيل إمكانية الرد";

            }
            documentGroup.UpdatedAt = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss");
            db.Entry(documentGroup).State = EntityState.Modified;


            db.SaveChanges();
            int Form_id = documentGroup.DocumentId;

            UsersId = db.UsersGroups.Where(a => a.GroupId == documentGroup.GroupId).Select(a => a.UserId).ToList();
            string GroupName = db.Groups.Find(documentGroup.GroupId).Name;
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
                    Message = "تمت عملية  " + ActiveMode + " في المجموعة " + GroupName + " رقم الوثيقة :" + doc.DocumentNumber + " موضوع الوثيقة :" + doc.Subject
                    + " ،عنوان الوثيقة :" + doc.Address + "،وصف الوثيقة :" + doc.Description
                   ,
                    NotificationOwnerId = UserId
                };
                db.Notifications.Add(notification);
            }
            db.SaveChanges();


            return RedirectToAction("Index", new { @id = Form_id, @msg = "EditSuccess" });
        }



        
        [AccessDeniedAuthorizeattribute(ActionName = "DocumentGroupsActiveNonActiveSeal")]

        public ActionResult ActiveNOnActiveSeal(int? id)
        {
            ViewBag.Current = "Document";

            if (id == null)
            {
                return RedirectToAction("BadRequestError", "ErrorController");

            }
            DocumentGroup documentGroup = db.DocumentGroups.Include(a => a.document).Include(a => a.Group).SingleOrDefault(a => a.Id == id);
            if (documentGroup == null)
            {
                return RedirectToAction("HttpNotFoundError", "ErrorController");

            }
            return View(documentGroup);
        }



        [HttpPost, ActionName("ActiveNOnActiveSeal")]
        [ValidateAntiForgeryToken]
        
        [AccessDeniedAuthorizeattribute(ActionName = "DocumentGroupsActiveNonActiveSeal")]

        public ActionResult ActiveNOnActiveConfirmSeal(int id)
        {
            ViewBag.Current = "Document";

            string ActiveMode = string.Empty;
            List<string> UsersId = new List<string>();
            string NotificationTime = string.Empty;
            string UserId = User.Identity.GetUserId();
            Document doc = db.DocumentGroups.Include(a => a.document).SingleOrDefault(a => a.Id == id).document;

            DocumentGroup documentGroup = db.DocumentGroups.Find(id);
            if (documentGroup.EnableSeal == true)
            {
                documentGroup.EnableSeal = false;
                ActiveMode = "الغاء  إمكانية التسديد";
            }
            else
            {
                documentGroup.EnableSeal = true;
                ActiveMode = "تفعيل إمكانية التسديد";

            }
            documentGroup.UpdatedAt = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss");
            db.Entry(documentGroup).State = EntityState.Modified;


            db.SaveChanges();
            int Form_id = documentGroup.DocumentId;

            UsersId = db.UsersGroups.Where(a => a.GroupId == documentGroup.GroupId).Select(a => a.UserId).ToList();
            string GroupName = db.Groups.Find(documentGroup.GroupId).Name;
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
                    Message = "تمت عملية  " + ActiveMode + " في المجموعة " + GroupName + " رقم الوثيقة :" + doc.DocumentNumber + " موضوع الوثيقة :" + doc.Subject
                    + " ،عنوان الوثيقة :" + doc.Address + "،وصف الوثيقة :" + doc.Description
                   ,
                    NotificationOwnerId = UserId
                };
                db.Notifications.Add(notification);
            }
            db.SaveChanges();


            return RedirectToAction("Index", new { @id = Form_id, @msg = "EditSuccess" });
        }




        
        [AccessDeniedAuthorizeattribute(ActionName = "DocumentGroupsActiveNonActiveRelate")]

        public ActionResult ActiveNOnActiveRelate(int? id)
        {
            ViewBag.Current = "Document";

            if (id == null)
            {
                return RedirectToAction("BadRequestError", "ErrorController");

            }
            DocumentGroup documentGroup = db.DocumentGroups.Include(a => a.document).Include(a => a.Group).SingleOrDefault(a => a.Id == id);
            if (documentGroup == null)
            {
                return RedirectToAction("HttpNotFoundError", "ErrorController");

            }
            return View(documentGroup);
        }



        [HttpPost, ActionName("ActiveNOnActiveRelate")]
        [ValidateAntiForgeryToken]
        
        [AccessDeniedAuthorizeattribute(ActionName = "DocumentGroupsActiveNonActiveRelate")]

        public ActionResult ActiveNOnActiveConfirmRelate(int id)
        {
            ViewBag.Current = "Document";

            string ActiveMode = string.Empty;
            List<string> UsersId = new List<string>();
            string NotificationTime = string.Empty;
            string UserId = User.Identity.GetUserId();
            Document doc = db.DocumentGroups.Include(a => a.document).SingleOrDefault(a => a.Id == id).document;

            DocumentGroup documentGroup = db.DocumentGroups.Find(id);
            if (documentGroup.EnableRelate == true)
            {
                documentGroup.EnableRelate = false;
                ActiveMode = "الغاء  إمكانية الربط";
            }
            else
            {
                documentGroup.EnableRelate = true;
                ActiveMode = "تفعيل إمكانية الربط";

            }
            documentGroup.UpdatedAt = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss");
            db.Entry(documentGroup).State = EntityState.Modified;


            db.SaveChanges();
            int Form_id = documentGroup.DocumentId;

            UsersId = db.UsersGroups.Where(a => a.GroupId == documentGroup.GroupId).Select(a => a.UserId).ToList();
            string GroupName = db.Groups.Find(documentGroup.GroupId).Name;
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
                    Message = "تمت عملية  " + ActiveMode + " في المجموعة " + GroupName + " رقم الوثيقة :" + doc.DocumentNumber + " موضوع الوثيقة :" + doc.Subject
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
            DocumentGroup documentGroup = db.DocumentGroups.Include(a => a.CreatedBy).Include(a => a.document).Include(a => a.Group).SingleOrDefault(a => a.Id == id);
            if (documentGroup == null)
            {
                return RedirectToAction("HttpNotFoundError", "ErrorController");

            }
            return View(documentGroup);
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