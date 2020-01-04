using ArchiveProject2019.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ArchiveProject2019.ViewModel;
using ArchiveProject2019.HelperClasses;

namespace ArchiveProject2019.Controllers
{
    public class DashBoardController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: DashBoard


       
        [AccessDeniedAuthorizeattribute(ActionName = "DashBoard")]
        public ActionResult Index()
        {


            Informations info = new Informations();


            string CurrentUserId = this.User.Identity.GetUserId();

            //Information For master:
            if (db.Users.Find(CurrentUserId).RoleName.Equals("Master"))
            {

                info.IsMasterRole = true;


                //Departments:
                info.MainDepartmentCount = db.Departments.Where(a => a.ParentId == null).Count();
                info.AllDepartmentsCount = db.Departments.Count();
                info.LastDateDepartmentCreate = db.Departments.Count() != 0 ? db.Departments.OrderByDescending(a => a.CreatedAt).FirstOrDefault().CreatedAt : "";

                info.LastDateDepartmentUpdate = db.Departments.Count(a => a.UpdatedAt != null) != 0 ? db.Departments.OrderByDescending(a => a.UpdatedAt).FirstOrDefault().UpdatedAt : "";


                //Groups:

                info.AllGroupsCount = db.Groups.Count();
                info.LastGroupCreate = db.Groups.Count() != 0 ? db.Groups.OrderByDescending(a => a.CreatedAt).FirstOrDefault().CreatedAt : "";
                info.TotalUserInGroup = db.UsersGroups.Select(a => a.UserId).Distinct().Count();
                info.LastGroupUpdate = db.Groups.Count(a => a.UpdatedAt != null) != 0 ? db.Departments.OrderByDescending(a => a.UpdatedAt).FirstOrDefault().UpdatedAt : "";



                int HightGroupUsersC = db.UsersGroups.Count() == 0 ? 0 :


                       db.UsersGroups.Include(a => a.Group).GroupBy(a => a.Group.Name).Select(a => new
                       {

                           Key = a.Key,
                           count = a.Count()

                       }).OrderByDescending(a => a.count).FirstOrDefault().count;

                ;

                if (HightGroupUsersC != 0)
                {
                    List<string> GName = new List<string>();
                    GName = db.UsersGroups.Include(a => a.Group).GroupBy(a => a.Group.Name).Select(a => new
                    {

                        Key = a.Key,
                        count = a.Count()

                    }).Where(a => a.count == HightGroupUsersC).Select(a => a.Key).ToList();


                    info.HightGroupUsers = GName.Aggregate((a, b) => a + "," + b);

                }
                else
                {
                    info.HightGroupUsers = "";
                }



                //Forms:
                info.TotalFormsCount = db.Forms.Count();
                info.LastFormCreate = db.Forms.Count() != 0 ? db.Forms.OrderByDescending(a => a.CreatedAt).FirstOrDefault().CreatedAt : "";
                info.LastFormUpdate = db.Forms.Count(a => a.UpdatedAt != null) != 0 ? db.Departments.OrderByDescending(a => a.UpdatedAt).FirstOrDefault().UpdatedAt : "";
                info.HightFormUsing = "";

                int HightFormUsingC = db.Documents.Count() == 0 ? 0 :

                     db.Documents.Include(a => a.Form).GroupBy(a => a.Form.Name)
                     .Select(a => new
                     {

                         key = a.Key,
                         count = a.Count()
                     }).OrderByDescending(a => a.count).FirstOrDefault().count;


                if (HightFormUsingC != 0)
                {
                    List<string> FName = new List<string>();
                    FName = db.Documents.Include(a => a.Form).GroupBy(a => a.Form.Name)
                    .Select(a => new
                    {

                        key = a.Key,
                        count = a.Count()
                    }).Where(a => a.count == HightFormUsingC).Select(a => a.key).ToList();

                    info.HightFormUsing = FName.Aggregate((a, b) => a + "," + b);
                }
                else
                {
                    info.HightFormUsing = "";
                }


                //Users:
                info.TotalUserCount = db.Users.Count();
                info.LastUserCreate = db.Users.Count() != 0 ? db.Users.OrderByDescending(a => a.CreatedAt).FirstOrDefault().CreatedAt : "";
                info.LastUserUpdate = db.Users.Count(a => a.UpdatedAt != null) != 0 ? db.Users.OrderByDescending(a => a.UpdatedAt).FirstOrDefault().UpdatedAt : "";



                //Document

                info.TotalDocumentCount = db.Documents.Count();
                info.LastDocumentCreate = db.Documents.Count() != 0 ? db.Documents.OrderByDescending(a => a.CreatedAt).FirstOrDefault().CreatedAt : "";





                //Mails:
                info.TotalMailsCount = db.TypeMails.Count();
                info.LastMailsCreate = db.TypeMails.Count() != 0 ? db.TypeMails.OrderByDescending(a => a.CreatedAt).FirstOrDefault().CreatedAt : "";
                info.LastMailsUpdate = db.TypeMails.Count(a => a.UpdatedAt != null) != 0 ? db.TypeMails.OrderByDescending(a => a.UpdatedAt).FirstOrDefault().UpdatedAt : "";


                int HightMailUsingC = db.Documents.Count() == 0 ? 0 :

                    db.Documents.Include(a => a.TypeMail).GroupBy(a => a.TypeMail.Name)
                    .Select(a => new
                    {

                        key = a.Key,
                        count = a.Count()
                    }).OrderByDescending(a => a.count).FirstOrDefault().count;


                if (HightMailUsingC != 0)
                {
                    List<string> MName = new List<string>();
                    MName = db.Documents.Include(a => a.TypeMail).GroupBy(a => a.TypeMail.Name)
                    .Select(a => new
                    {

                        key = a.Key,
                        count = a.Count()
                    }).Where(a => a.count == HightMailUsingC).Select(a => a.key).ToList();

                    info.HightMailUsing = MName.Aggregate((a, b) => a + "," + b);
                }
                else
                {
                    info.HightMailUsing = "";
                }





                //Document Kind:
                info.TotalDocumentKindCount = db.Kinds.Count();
                info.LastDocumentKindCreate = db.Kinds.Count() != 0 ? db.Kinds.OrderByDescending(a => a.CreatedAt).FirstOrDefault().CreatedAt : "";
                info.LastDocumentKindUpdate = db.Kinds.Count(a => a.UpdatedAt != null) != 0 ? db.Kinds.OrderByDescending(a => a.UpdatedAt).FirstOrDefault().UpdatedAt : "";

                int HightDocumentKindUsingC = db.Documents.Include(a => a.Kind).Count(a => a.Kind != null) == 0 ? 0 :

                  db.Documents.Include(a => a.Kind).GroupBy(a => a.Kind.Name)
                  .Select(a => new
                  {

                      key = a.Key,
                      count = a.Count()
                  }).OrderByDescending(a => a.count).FirstOrDefault().count;


                if (HightDocumentKindUsingC != 0)
                {
                    List<string> MName = new List<string>();
                    MName = db.Documents.Include(a => a.Kind).GroupBy(a => a.Kind.Name)
                    .Select(a => new
                    {

                        key = a.Key,
                        count = a.Count()
                    }).Where(a => a.count == HightDocumentKindUsingC).Select(a => a.key).ToList();

                    info.HightDocumentKindUsing = MName.Aggregate((a, b) => a + "," + b);
                }
                else
                {
                    info.HightDocumentKindUsing = "";
                }



            }

            else
            {
                info.IsMasterRole = false;
            }
            //CurrentUser Informations:

            info.FullName = db.Users.Find(CurrentUserId).FullName;
            info.UserName = db.Users.Find(CurrentUserId).UserName;
            info.Email = db.Users.Find(CurrentUserId).Email == null ? "" : db.Users.Find(CurrentUserId).Email;
            info.Gender = db.Users.Find(CurrentUserId).Gender;
            
                
              int DepId=  db.Users.Include(a => a.Department).SingleOrDefault(a => a.Id.Equals(CurrentUserId)).DepartmentId == null ?
                0 :  db.Users.Include(a => a.Department).SingleOrDefault(a => a.Id.Equals(CurrentUserId)).Department.Id;


            info.DepartmentName = DepId == 0 ? "" : DepartmentListDisplay.CreateDepartmentDisplay(DepId);
            info.JobTitle = db.Users.Include(a => a.jobTitle).SingleOrDefault(a => a.Id.Equals(CurrentUserId)).JobTitleId == null ?
             "" : db.Users.Include(a => a.jobTitle).SingleOrDefault(a => a.Id.Equals(CurrentUserId)).jobTitle.Name;


            info.UserCreateAt = db.Users.Find(CurrentUserId).CreatedAt;
            info.UserUpdate = db.Users.Find(CurrentUserId).UpdatedAt == null ? "" : db.Users.Find(CurrentUserId).UpdatedAt;
            info.RoleName = db.Users.Find(CurrentUserId).RoleName;





            //User Groups
            if(!db.Users.Find(CurrentUserId).RoleName.Equals("Master"))
            {
                info.IsMasterRole = false;

                GroupsUserInformation groupsUserInformation = null;
                info.UserGroups = new List<GroupsUserInformation>();
                List<UserGroup> UserG = db.UsersGroups.Include(a => a.Group).Where(a => a.UserId.Equals(CurrentUserId)).ToList();
                foreach(UserGroup u in UserG)
                {
                    groupsUserInformation = new GroupsUserInformation();

                    groupsUserInformation.Name = u.Group.Name;
                    groupsUserInformation.UsersCount = db.UsersGroups.Where(a => a.GroupId == u.GroupId).Count();
                    info.UserGroups.Add(groupsUserInformation);
                }



            }
            else
            {
                info.IsMasterRole = true;
            }

          



            //User Favorite Form:

            if(!db.Users.Find(CurrentUserId).RoleName.Equals("Master"))
            {
                info.IsMasterRole = false;



                List<int> FavoriteFormId = db.FavouriteForms.Where(a => a.UserId.Equals(CurrentUserId)).Select(a => a.FormId).ToList(); ;


                info.FavoriteForm = new List<Form>();
                info.FavoriteForm = db.Forms.Where(a => FavoriteFormId.Contains(a.Id)).ToList();


            }
            else
            {
                info.IsMasterRole = true;

            }


            //User Basic Information:
            info.MyTotalDocument = db.Documents.Where(a => a.CreatedById.Equals(CurrentUserId)).Count();
            if(!db.Users.Find(CurrentUserId).RoleName.Equals("Master"))
            {

                
                info.MyGroupsCount = db.UsersGroups.Where(a => a.UserId.Equals(CurrentUserId)).Count();
                info.LastMyDocumentCreate = db.Documents.Where(a=>a.CreatedById.Equals(CurrentUserId)).Count() != 0 ? db.Documents.Where(a=>a.CreatedById.Equals(CurrentUserId)).OrderByDescending(a => a.CreatedAt).FirstOrDefault().CreatedAt : "0/0/0";

                info.DepartmentsCount = db.Departments.Count();
            }
            info.Company = db.Company.Find(1);

            return View(info);
        }

    
        [AccessDeniedAuthorizeattribute(ActionName = "DashBoard")]

        public ActionResult NotificationsUserCount()
        {
            string CurrentUserId = this.User.Identity.GetUserId();
            int count = db.Notifications.Where(a => a.UserId.Equals(CurrentUserId)).Count();
            ViewBag.NotCount = count;
            return PartialView("_NotificationsCount");
        }

     

        [AccessDeniedAuthorizeattribute(ActionName = "DashBoard")]

        public ActionResult NotificationsUserMessage()
        {
            string CurrentUserId = this.User.Identity.GetUserId();
            List<Notification> Not = db.Notifications.Include(a=>a.NotificationOwner).Where(a => a.UserId.Equals(CurrentUserId) ).OrderByDescending(a=>a.CreatedAt).Take(5).ToList() ;
            
            return PartialView("_NotificationsMessage",Not);
        }

       

        [AccessDeniedAuthorizeattribute(ActionName = "NonSeenNotificationList")]

        public ActionResult NonSeenNotifications()
        {
            ViewBag.Current = "Notification";
            string CurrentUserId = this.User.Identity.GetUserId();
            List<Notification> Not = db.Notifications.Include(a => a.NotificationOwner).Where(a => a.UserId.Equals(CurrentUserId)).OrderByDescending(a => a.CreatedAt).ToList();



            return View(Not);
        }

       

        [HttpPost]
        [AccessDeniedAuthorizeattribute(ActionName = "NonSeenNotificationListPost")]

        public ActionResult NonSeenNotifications(List<string> NotificationId)
        {
            ViewBag.Current = "Notification";

            if (NotificationId==null)
            {
                return RedirectToAction("NonSeenNotifications");

            }

            foreach(string Id in NotificationId)
            {
                int NotId = Convert.ToInt32(Id);
                Notification not = db.Notifications.Find(NotId);

                db.Notifications.Remove(not);
            }

            db.SaveChanges();

            string CurrentUserId = this.User.Identity.GetUserId();
            List<Notification> Not = db.Notifications.Include(a => a.NotificationOwner).Where(a => a.UserId.Equals(CurrentUserId) && a.Active == false).OrderByDescending(a => a.CreatedAt).ToList();



            return View("NonSeenNotifications", Not);
        }



        

        [AccessDeniedAuthorizeattribute(ActionName = "NonSeenNotificationListAllPost")]
        public ActionResult ConvertAllToSeen()
        {
            ViewBag.Current = "Notification";
            string CurrentUserId = this.User.Identity.GetUserId();

            List<int> NotifId = db.Notifications.
                Where(a => a.UserId.Equals(CurrentUserId) && a.Active == false).Select(a=>a.Id).ToList();
            foreach(int n in NotifId)
            {
                Notification not = db.Notifications.Find(n);
                db.Notifications.Remove(not);

            }
            db.SaveChanges();

            return RedirectToAction("NonSeenNotifications");

        }








      

        [AccessDeniedAuthorizeattribute(ActionName = "DashBoard")]
        public ActionResult DocumentNotificationsUserCount()
        {

            string CurrentUserId = this.User.Identity.GetUserId();
            DateTime TodayDate = DateTime.ParseExact(DateTime.Now.ToString("yyyy/MM/dd").Replace("-", "/"), "yyyy/MM/dd", null);

            List<Document> Documents = db.Documents.Where(a => a.NotificationUserId.Equals(CurrentUserId) && a.NotificationDate != null).ToList();
            int count = Documents.Where(a => EqualDate(a.NotificationDate, TodayDate)).Count();
            ViewBag.NotCount = count;
            return PartialView("_DocumentNotificationCount");
        }


     

        [AccessDeniedAuthorizeattribute(ActionName = "DashBoard")]

        public ActionResult DocumentsNotificationUserMessage()
        {
            DateTime TodayDate = DateTime.ParseExact(DateTime.Now.ToString("yyyy/MM/dd").Replace("-", "/"), "yyyy/MM/dd", null);

            string CurrentUserId = this.User.Identity.GetUserId();
            List<Document> Documents = db.Documents.Where(a => a.NotificationUserId.Equals(CurrentUserId) && a.NotificationDate != null).ToList();
            Documents = Documents.Where(a => EqualDate(a.NotificationDate, TodayDate)).OrderByDescending(a=>a.CreatedAt).ToList();

            Notification notification = null;
            string NotificationTime = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss");
            List<Notification> NotificationList = new List<Notification>();
            foreach(Document d in Documents)
            {
                notification = new Notification()
                {


                    CreatedAt = NotificationTime,
                    Active = false,
            
                    Message  = "تنبيه للوثيقة :" + d.DocumentNumber + " "+" موضوع الوثيقة :" + d.Subject
                            + " ،عنوان الوثيقة :" + d.Address + "،وصف الوثيقة :" + d.Description

                       ,
                    NotificationOwnerId = db.Users.Find(CurrentUserId).FullName


                };


                NotificationList.Add(notification);
            }
            
            return PartialView("_DocumentsNotificationMessage", NotificationList.ToList());
        }





        [NonAction]


        public bool EqualDate(string s1, DateTime s2)
        {
            try
            {


            s1 = s1.Replace("-", "/");
            return DateTime.ParseExact(s1, "yyyy/MM/dd", null) == s2;
            }
            catch(Exception e)
            {
                return false;

            }

        }

    }
}